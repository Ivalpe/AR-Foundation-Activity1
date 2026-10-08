using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("UI Components")]
    
    [SerializeField] private CanvasGroup startMenuCanvasGroup;
    public GameObject startMenu;
    public GameObject deathScreen;
    public GameObject winScreen;
    public TextMeshProUGUI waveText;
    public GameObject lifeBar;

    [Header("AR Components")]
    
    [SerializeField] private ARSession arSession;
    [SerializeField] WaveControl waveScript;
    [SerializeField] LifeManager lifeManager;


    //[Header("Scene")]
    //public Object gameplayScene;
    [Header("Audio Components")]
    public AudioClip yaySFX;
    public AudioClip wastedSFX, startButtonSFX;
    public AudioSource audioSource;
   

    private void Awake()
    {
       if(waveScript != null) waveScript.enabled = false;
    }

    private void Start()
    {
        lifeBar.SetActive(false);
        waveScript.gameObject.SetActive(false);
        
        audioSource = Camera.main.gameObject.GetComponent<AudioSource>();
        if (startMenu != null)
        {
            //startMenuCanvasGroup.alpha = 1f;
            //startMenuCanvasGroup.interactable = true;
            //startMenuCanvasGroup.blocksRaycasts = true;
            startMenu.SetActive(true);
            

        }

        if (deathScreen != null) deathScreen.SetActive(false);
        if(winScreen != null) winScreen.SetActive(false);
        
        if (arSession != null)
        {
            arSession.enabled = false;
        }

        waveText.text = "";
        
    }

   

 
    public void OnStartButtonClicked()
    {
        
        if (startMenu != null)
        {
            audioSource.PlayOneShot(startButtonSFX);
            //startMenuCanvasGroup.alpha = 0f;          
            //startMenuCanvasGroup.interactable = false;  
            //startMenuCanvasGroup.blocksRaycasts = false;
            //if(gameplayScene != null) SceneManager.LoadScene(gameplayScene.name);
            startMenu.SetActive(false);
            waveScript.gameObject.SetActive(true);
            waveScript.enabled = true;
            lifeBar.SetActive(true);

            //restart the game?
            lifeManager.InitializePlayer();
            waveScript.Restart();
        }
        else
        {
            Debug.LogError("Falta assignar el 'Start Menu Canvas Group' a l'inspector!");
        }

        
        if (arSession != null)
        {
            arSession.enabled = true;
            Debug.Log("AR Session activada correctament.");
        }
        else
        {
            Debug.LogWarning("Falta assignar l' 'AR Session' a l'inspector.");
        }
    }

    public void OnStartMenuButtonClicked()
    {

        GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>().volume = 1.0f;

        GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapon");
        foreach (GameObject w in weapons)
        {
            //NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
            //agent.isStopped = true;
            
            Destroy(w);
        }
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject e in enemies)
        {
            //NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
            //agent.isStopped = true;
            Destroy(e);
        }

        if (deathScreen && deathScreen.activeSelf) deathScreen.SetActive(false);
        if(winScreen && winScreen.activeSelf)
        {
            winScreen.SetActive(false);
        }

        if(lifeBar && lifeBar.activeSelf)
        {
            lifeBar.SetActive(false);
        } 
            

        if (startMenu && !startMenu.activeSelf)
        {
            startMenu.SetActive(true);
        }
        arSession.enabled = false;
    }

    
    public void OnExitButtonClicked()
    {
        Application.Quit();
    }

 
}
