using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    
    public bool inArea = false;
    public Transform target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<SphereCollider>().radius = GetComponentInParent<CannonScript>().detectRadius;
    }

    // Update is called once per frame
    void Update()
    {
        if (!target)inArea = false;
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            inArea = true;
            target = other.gameObject.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            //inArea = false;
            target = null;
        }
    }
}
