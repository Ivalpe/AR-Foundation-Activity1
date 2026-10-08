using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class LifeManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] float hp = 10.0f;
    [SerializeField] GameObject deathScreen;
    [SerializeField] GameObject winScreen;

    public WaveControl waveScript;
    public AudioClip oofSFX, wastedSFX;
    AudioSource audioSource;
    AudioSource musicSource;
    GameObject camObj;

    public float knockForce = 100.0f;
    bool inCooldown = false;
    public float dmgCooldown = 2.0f;

    bool isDead = true;

    void Start()
    {
        
        camObj = Camera.main.gameObject;
        audioSource = camObj.GetComponent<AudioSource>();
        musicSource = GameObject.FindGameObjectWithTag("BMG").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camPos = camObj.transform.position;
        transform.SetPositionAndRotation(camPos, Quaternion.identity);

        if (Keyboard.current.f1Key.wasPressedThisFrame)
        {
            hp = 0;
            OnPlayerDeath();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        

        if (collision.gameObject.CompareTag("Enemy"))
        {
            //Debug.Log("hit enemy");
            if (hp > 0)
            {
                if (!inCooldown){
                    //TakeDamage(1.0f);
                    StartCoroutine(TakeDamage(1.0f, collision.gameObject));
                    Debug.Log("-1 de vida, life remaining: " + hp);
                }
                
                
            }
            else
            {
                
                OnPlayerDeath();
            }
            //Destroy(collision.gameObject);
        }
    }

    private void OnWin()
    {
        if (waveScript.win == true) winScreen.SetActive(true);
    }

    IEnumerator TakeDamage(float dmg, GameObject enemy)
    {
        hp--;
        inCooldown = true;
        //KnockBack(enemy, knockForce);

        Rigidbody rb = enemy.GetComponent<Rigidbody>();
        Vector3 knockDir = (-enemy.transform.forward.normalized + enemy.transform.up.normalized) * knockForce;

        audioSource.pitch = Random.Range(0.7f, 1.5f);
        audioSource.PlayOneShot(oofSFX);

        rb.isKinematic = false;
        rb.AddForce(knockDir, ForceMode.Force);

        yield return new WaitForSeconds(1.0f);
        rb.isKinematic = true;

        
        yield return new WaitForSeconds(dmgCooldown);

        inCooldown = false;
    }

    void OnPlayerDeath()
    {
        deathScreen.SetActive(true);
        musicSource.volume = 0.3f;
        audioSource.PlayOneShot(wastedSFX);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies)
        {
            NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
            agent.isStopped = true;
        }
        isDead = true;
    }
}
