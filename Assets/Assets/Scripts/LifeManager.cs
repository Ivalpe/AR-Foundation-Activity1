using UnityEngine;

public class LifeManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] int lives = 3;
    [SerializeField ]GameObject deathScreen;
    [SerializeField] GameObject winScreen;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if(lives>0) lives--;
            else
            {
                deathScreen.SetActive(true);
            }
            Destroy(collision.gameObject);
        }
    }
}
