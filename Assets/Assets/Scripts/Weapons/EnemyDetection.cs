using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public float detectRadius = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<SphereCollider>().radius = detectRadius;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {

        }
    }
}
