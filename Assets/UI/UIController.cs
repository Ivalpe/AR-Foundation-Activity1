using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation; 

public class UIController : MonoBehaviour
{
    [Header("UI Components")]
    
    [SerializeField] private CanvasGroup startMenuCanvasGroup;
    [SerializeField] private GameObject startMenu;
    public GameObject deathScreen;
    public GameObject winScreen;

    [Header("AR Components")]
    
    [SerializeField] private ARSession arSession;

    //[Header("Scene")]
    //public Object gameplayScene;
    [Header("Audio Components")]
    public AudioClip yaySFX;
    public AudioClip wastedSFX, startButtonSFX;
    public AudioSource audioSource;

    private void Start()
    {
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
            
            //restart the game?
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
        if(deathScreen && deathScreen.activeSelf) deathScreen.SetActive(false);
        if(winScreen && winScreen.activeSelf)
        {
            winScreen.SetActive(false);
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
