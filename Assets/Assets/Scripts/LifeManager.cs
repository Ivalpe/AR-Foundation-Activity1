using System.Collections;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] float hp = 10.0f;
    [SerializeField] GameObject deathScreen;
    [SerializeField] GameObject winScreen;

    public WaveControl waveScript;
    public AudioClip oofSFX;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("oncollisionentrer");
        if (  /*collision.gameObject.tag==("Enemy")*/       collision.gameObject.CompareTag("Enemy"))
        {
           
            if (hp > 0)
            {
                TakeDamage(1.0f);
                Debug.Log("-1 de vida"+ hp);
                
            }
            else
            {
                deathScreen.SetActive(true);
            }
            Destroy(collision.gameObject);
        }
    }

    private void OnWin()
    {
        if (waveScript.win == true) winScreen.SetActive(true);
    }

    IEnumerator TakeDamage(float dmg)
    {
        hp--;
        audioSource.PlayOneShot(oofSFX);
        yield return new WaitForSeconds(1.0f);
    }
}
