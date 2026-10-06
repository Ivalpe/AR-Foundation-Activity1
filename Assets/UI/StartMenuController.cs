using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation; 

public class StartMenuController : MonoBehaviour
{
    [Header("UI Components")]
    
    [SerializeField] private CanvasGroup startMenuCanvasGroup;

    [Header("AR Components")]
    
    [SerializeField] private ARSession arSession;

    [Header("Scene")]
    public Object gameplayScene;

    private void Start()
    {
        
        if (startMenuCanvasGroup != null)
        {
            startMenuCanvasGroup.alpha = 1f;
            startMenuCanvasGroup.interactable = true;
            startMenuCanvasGroup.blocksRaycasts = true;
        }

        
        if (arSession != null)
        {
            arSession.enabled = false;
        }
    }

 
    public void OnStartButtonClicked()
    {
        
        if (startMenuCanvasGroup != null)
        {
            startMenuCanvasGroup.alpha = 0f;          
            startMenuCanvasGroup.interactable = false;  
            startMenuCanvasGroup.blocksRaycasts = false;
            if(gameplayScene != null) SceneManager.LoadScene(gameplayScene.name);
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
}
