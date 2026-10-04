using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using Unity.XR.CoreUtils;

public class CannonScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
    Animator cannonAnimator;
    public GameObject[] projectiles;
    EnemyDetection colliderScript;
    public float detectRadius = 10f;
    float shootTimer = 0;
    public float shootInterval = 1.0f;
    public float shootDelay = 0;
    public Transform shootPoint;
    public float shootForce = 200.0f;
    public float verticalShootForce = 0.2f;

    [SerializeField] public int maxAmmo = 10;
    [SerializeField] private bool startOutOfAmmo = false;
    int currentAmmo;
    

    Vector3 shootDir = Vector3.zero;
    GameObject cannon;

    [SerializeField] private float maxCamDist = 50.0f;

    Canvas reloadPopUp;

    AudioSource cannonSource;
    public AudioClip[] shootSFX;

    void Start()
    {
        colliderScript = GetComponentInChildren<EnemyDetection>();
        cannonAnimator = GetComponentInChildren<Animator>();
        cannonSource = GetComponentInChildren<AudioSource>();
        cannon = cannonAnimator.gameObject;

        if (startOutOfAmmo) currentAmmo = 0;
        else currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 screenPos = Vector2.zero;

        
        if (reloadPopUp == null)
        {
            reloadPopUp = GetComponentInChildren<Canvas>(true);
        }

        
        if (IsVisible(this.gameObject, out Vector3 closestPoint))
        {
            
            float distToCam = Vector3.Distance(Camera.main.transform.position, closestPoint);

            if (distToCam <= maxCamDist)
            {
                if (reloadPopUp && currentAmmo <= 0)
                {
                    reloadPopUp.gameObject.SetActive(true);
                }
            }
            else
            {
                // Too far away -> disable popup
                if (reloadPopUp) reloadPopUp.gameObject.SetActive(false);
            }
        }
        else
        {
            // Not visible on screen -> disable popup
            if (reloadPopUp != null)
            {
                reloadPopUp.gameObject.SetActive(false);
            }
        }

        // Handle Reload Input
        if (reloadPopUp && reloadPopUp.gameObject.activeSelf)
        {
            if (ScreenClickOrTap(ref screenPos) && currentAmmo < maxAmmo)
            {
                if (RayHitVisibleTarget(this.gameObject))
                {
                    currentAmmo++;
                    if (currentAmmo >= maxAmmo)
                    {
                        reloadPopUp.gameObject.SetActive(false);
                    }
                }
                
                //Debug.Log("current ammo of " + gameObject.name + ": " + currentAmmo);

                
            }
        }





        shootTimer += Time.deltaTime;
        if (colliderScript.inArea)
        {
            if (cannon && colliderScript.target)
            {
                shootDir = (colliderScript.target.position - shootPoint.position).normalized + Vector3.up * verticalShootForce; //--> this is too precise!

                Vector3 lookDir = new Vector3(shootDir.x, 0f, shootDir.z); //so that the cannon doesn't tilt vertically

                if (lookDir != Vector3.zero)
                {

                    Quaternion q = Quaternion.Slerp(cannon.transform.localRotation, Quaternion.LookRotation(lookDir), Time.deltaTime);
                    cannon.transform.SetLocalPositionAndRotation(cannon.transform.localPosition, q);
                }

            }

            if (shootTimer >= shootInterval && currentAmmo > 0)
            {

                StartCoroutine(Shoot());

                shootTimer = 0;

            }

        }
        
    }

    IEnumerator Shoot()
    {
        cannonAnimator.SetTrigger("Shoot");

        yield return new WaitForSeconds(shootDelay);

        int randObj = Random.Range(0, projectiles.Length);
        GameObject projectile = Instantiate(projectiles[randObj], shootPoint);
        currentAmmo--;


        int randSFX = Random.Range(0, shootSFX.Length);

        if (cannonSource)
        {
            cannonSource.pitch = Random.Range(0.5f, 2.0f);
            cannonSource.PlayOneShot(shootSFX[randSFX]);
        }
 

        //cannonAnimator.gameObject.transform.LookAt(colliderScript.target);
        projectile.transform.SetParent(null);
        projectile.GetComponent<Rigidbody>().AddForce(shootDir * shootForce, ForceMode.Force);
        Destroy(projectile, 5f);

        
        
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    bool ScreenClickOrTap(ref Vector2 screenPos)
    {
        //OLD INPUT SYSTEM
        //if (Input.GetMouseButtonDown(0))
        //{

        //    screenPos = Input.mousePosition;
        //    return true;
        //}

        //if(Input.touchCount > 0)
        //{
        //    //touch input
        //    Touch touch = Input.GetTouch(0);
        //    screenPos = touch.position;
        //    return true;
        //}

        //NEW INPUT SYSTEM --> Pointer unifies input from mouse, touch and stylus ! 

        if(Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            screenPos = Pointer.current.position.ReadValue();
        }

        return false;
    }

    public bool IsVisible(GameObject go, out Vector3 closestPoint)
    {
        Renderer rend = go.GetComponentInChildren<Renderer>();
        if (rend == null || Camera.main == null)
        {
            closestPoint = this.transform.position;
            return false;
        }
            

        
        //bounds = the object's axis-aligned bounding box
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        // Test if the object's AABB intersects the frustum
        bool isVisible = GeometryUtility.TestPlanesAABB(planes, rend.bounds);

        //.ClosestPoint(Vector3 point) returns the closest point of the AABB to another given  point

        closestPoint = rend.bounds.ClosestPoint(Camera.main.transform.position);

        
        return isVisible;
    }

    public bool RayHitVisibleTarget(GameObject targetGO)
    {
        //Viewport coordinates are normalized and relative to the camera. The bottom-left of the camera is (0,0); the top-right is (1,1).
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, maxCamDist))
        {
            if (hit.collider.gameObject.transform.IsChildOf(targetGO.transform) && hit.collider.gameObject.GetComponent<Renderer>())
            {
                return true;
            }
            
        }
        return false;
    }

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    public void SetCurrentAmmo(int newAmmo)
    {
        currentAmmo = newAmmo;
    }
}
