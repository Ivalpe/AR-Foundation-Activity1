using UnityEngine;

public class StartMenuController : MonoBehaviour
{
    public GameObject startMenuPanel;
    public GameObject arSessionOrigin; // Or your AR components to enable

    public void OnStartButtonClicked()
    {
        // Hide the menu
        startMenuPanel.SetActive(false);

        // Enable AR tracking/interaction if desired
        if (arSessionOrigin != null)
            arSessionOrigin.SetActive(true);
    }
}
