using NUnit.Framework;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] float hp = 10.0f;
    //[SerializeField] GameObject deathScreen;
    //[SerializeField] GameObject winScreen;
    [SerializeField] UIController uiScript;

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
        musicSource = GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>();
        //InitializePlayer();

    }

    public void InitializePlayer()
    {
        
        isDead = false;
        hp = 10f;
        waveScript.currentWave = 0;
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

        Slider lifeBar;
        lifeBar = uiScript.lifeBar.GetComponentInChildren<Slider>();
        if (!lifeBar) lifeBar = uiScript.lifeBar.GetComponent<Slider>();

        if (lifeBar) 
        { 
            lifeBar.value = hp; 
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
        if (waveScript.win == true)
        {
            uiScript.winScreen.SetActive(true);
            uiScript.lifeBar.SetActive(false);
        }
    }

    IEnumerator TakeDamage(float dmg, GameObject enemy)
    {
        hp--;
        inCooldown = true;
        //KnockBack(enemy, knockForce);

        Rigidbody rb = enemy.GetComponent<Rigidbody>();
        Vector3 knockDir = (-enemy.transform.forward.normalized + enemy.transform.up.normalized) * knockForce;

        audioSource.pitch = UnityEngine.Random.Range(0.7f, 1.5f);
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
        
        if (!isDead)
        {
           
            uiScript.deathScreen.SetActive(true);
            uiScript.lifeBar.SetActive(false);
            musicSource.volume = 0.3f;
            audioSource.PlayOneShot(wastedSFX);
            isDead = true;
        }


    }
}
