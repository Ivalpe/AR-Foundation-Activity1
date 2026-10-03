using System;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    public string favFood = "Food";

    public AudioClip omnomSFX;
    public AudioClip[] animalSFX;
    AudioClip eatSFX;
    AudioSource animalSource;
    Animator animator;
    Rigidbody rb;

    public bool airborne = false;
    float speed = 0.0f;

    private void Awake()
    {
        currentHealth = maxHealth;
        animalSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        speed = Vector3.Magnitude(rb.linearVelocity);
        Debug.Log($"{gameObject.name} speed = {speed}");
        //animator.SetFloat("speed", speed);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Fruit") 
            || collision.gameObject.CompareTag("Meat") 
            || collision.gameObject.CompareTag("Sweet"))
        {
            if (currentHealth <= 0) return;

            if (collision.gameObject.CompareTag(favFood))
            {
                currentHealth -= 50f; //2-shot
                int rand = UnityEngine.Random.Range(0, animalSFX.Length);
                eatSFX = animalSFX[rand];
            }
            else
            {
                currentHealth -= 20.0f; //5-shot
                eatSFX = omnomSFX;
            }


            if (currentHealth <= 0)
            {
                Die();
            }

            animalSource.PlayOneShot(eatSFX);

            animator.SetTrigger("Ate");

            collision.gameObject.SetActive(false);

        }
    }
    

    private void Die()
    {
        Destroy(gameObject);
    }
}
