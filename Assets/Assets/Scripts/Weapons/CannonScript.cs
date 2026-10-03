using UnityEngine;
using System.Collections;

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

    Vector3 shootDir = Vector3.zero;
    GameObject cannon;


    AudioSource cannonSource;
    public AudioClip[] shootSFX;

    void Start()
    {
        colliderScript = GetComponentInChildren<EnemyDetection>();
        cannonAnimator = GetComponentInChildren<Animator>();
        cannonSource = GetComponentInChildren<AudioSource>();
        cannon = cannonAnimator.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;
        if (colliderScript.inArea)    
        {
            if (cannon && colliderScript.target)
            {
                shootDir = (colliderScript.target.position - shootPoint.position).normalized + Vector3.up * verticalShootForce; //--> this is too precise!

                Vector3 lookDir = new Vector3(shootDir.x, 0f, shootDir.z); //so that the cannon doesn't tilt vertically

                if (lookDir != Vector3.zero) {

                    Quaternion q = Quaternion.Slerp(cannon.transform.localRotation, Quaternion.LookRotation(lookDir), Time.deltaTime);
                    cannon.transform.SetLocalPositionAndRotation(cannon.transform.localPosition, q);
                }

            }

            if(shootTimer >= shootInterval)
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
}
