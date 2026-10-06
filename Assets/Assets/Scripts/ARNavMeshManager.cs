using UnityEngine;
using Unity.AI.Navigation;
using System.Collections;

public class ARNavMeshManager : MonoBehaviour
{
    [SerializeField] private NavMeshSurface navMeshSurface;
    [SerializeField] private float updateInterval = 1.0f; //how often it updates

    //bake the nav mesh (since it updates as the user moves the device)
    private Coroutine navMeshBake;

    public bool HasNavMesh { get; private set; } = false;


    private void Awake()
    {
        if (navMeshSurface == null)
            navMeshSurface = GetComponent<NavMeshSurface>();
        
    }
    //public void GetNavy(int y)
    //{
    //    y = (int)navMeshSurface.transform.position.y;
    //    Debug.Log("AA"+(int)navMeshSurface.transform.position.y);

    //}

    private void OnEnable()
    {
        navMeshBake = StartCoroutine(nameof(BakeNavMesh));
    }

    private void OnDisable()
    {
        if(navMeshBake != null) StopCoroutine(navMeshBake);
    }

    private IEnumerator BakeNavMesh()
    {
        yield return null;

        while (true)
        {
            if (navMeshSurface != null)
            {
                if (navMeshSurface.navMeshData == null)
                {
                    //build nav mesh
                    navMeshSurface.BuildNavMesh();

                    //if (navMeshSurface.navMeshData != null)
                    //{
                    //    HasNavMesh = true;
                    //}
                }
                //else
                //{

                //    AsyncOperation asyncUpdate = navMeshSurface.UpdateNavMesh(navMeshSurface.navMeshData);


                //    while (!asyncUpdate.isDone)
                //    {
                //        yield return null;
                //    }

                //    HasNavMesh = true;
                //}

                else
                {
                    //update nav mesh with new detected planes
                    navMeshSurface.UpdateNavMesh(navMeshSurface.navMeshData);
                }
                //communicate that nav mesh has been created
                HasNavMesh = true;

            }

            yield return new WaitForSeconds(updateInterval);
        }
    }
}
