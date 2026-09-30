using UnityEngine;

public class CannonScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
    Animator cannonAnimator;
    public GameObject[] projectiles;

    void Start()
    {
        GetComponentInChildren<SphereCollider>().radius = detectRadius;
        cannonAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
