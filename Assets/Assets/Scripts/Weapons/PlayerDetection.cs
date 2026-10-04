using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerDetection : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Canvas reloadPopUp;
    private CannonScript cannonScript;
    
    void Start()
    {
        cannonScript = GetComponentInParent<CannonScript>();
        reloadPopUp = cannonScript.gameObject.GetComponentInChildren<Canvas>(true);
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 screenPos = Vector2.zero;


        
        if (reloadPopUp == null)
        {
            reloadPopUp = GetComponentInChildren<Canvas>(true); // IF THE TRUE ARGUMENT ISN'T PASSED, IT IGNORES DISABLED COMPONENTS!!
        }

        
        else
        {
            // Raycast hit nothing in open space
            if (reloadPopUp != null)
            {
                reloadPopUp.gameObject.SetActive(false);
            }
        }

        // 3. Handle Reload Input
        if (reloadPopUp != null && reloadPopUp.enabled)
        {
            if (Pointer.current.IsPressed() && cannonScript.GetCurrentAmmo() < cannonScript.maxAmmo)
            {
                cannonScript.SetCurrentAmmo(cannonScript.GetCurrentAmmo() + 1);

                // Hide popup automatically once reloaded
                if (cannonScript.GetCurrentAmmo() >= cannonScript.maxAmmo)
                {
                    reloadPopUp.enabled = false;
                }
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("MainCamera") && cannonScript.GetCurrentAmmo() < 0)
        {
            reloadPopUp.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("MainCamera") && reloadPopUp.gameObject.activeSelf)
        {
            reloadPopUp.gameObject.SetActive(false);
        }
    }
    bool ScreenClickOrTap(ref Vector2 screenPos)
    {
        //NEW INPUT SYSTEM --> Pointer unifies input from mouse, touch and stylus ! 

        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            screenPos = Pointer.current.position.ReadValue();
        }

        return false;
        //OLD INPUT SYSTEM!
        //if (Input.GetMouseButtonDown(0))
        //{

        //    screenPos = Input.mousePosition;
        //    return true;
        //}

        //if (Input.touchCount > 0)
        //{
        //    //touch input
        //    Touch touch = Input.GetTouch(0);
        //    screenPos = touch.position;
        //    return true;
        //}

        //NEW INPUT SYSTEM HAS "POINTER", which unifies mouse, stylus and touch input (GGs)

    }
}
