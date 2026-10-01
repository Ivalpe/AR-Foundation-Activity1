using UnityEngine;

public class CannonScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
    Animator cannonAnimator;
    public GameObject[] projectiles;
    EnemyDetection colliderScript;
    public float detectRadius = 10f;
    float shootTimer = 0;
    public float shootInterval = 1.0f;
    public Transform shootPoint;
    public float shootForce = 2.0f;

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
                shootDir = (colliderScript.target.position - shootPoint.position).normalized + Vector3.up * 0.2f; //--> this is too precise!
                

                Quaternion q = Quaternion.Slerp(cannon.transform.localRotation, Quaternion.LookRotation(shootDir), Time.deltaTime);
                cannon.transform.SetLocalPositionAndRotation(cannon.transform.localPosition, q);
            }

            if(shootTimer >= shootInterval)
            {

                Shoot();

            }
            
        }
    }

    void Shoot()
    {
        cannonAnimator.SetTrigger("Shoot");

        int randObj = Random.Range(0, projectiles.Length);
        GameObject projectile = Instantiate(projectiles[randObj], shootPoint);


        int randSFX = Random.Range(0, shootSFX.Length);
        cannonSource.pitch = Random.Range(0.5f, 2.0f);
        cannonSource.PlayOneShot(shootSFX[randSFX]);

        //cannonAnimator.gameObject.transform.LookAt(colliderScript.target);
        projectile.transform.SetParent(null);
        projectile.GetComponent<Rigidbody>().AddForce(cannon.transform.forward * shootForce, ForceMode.Force);
        Destroy(projectile, 5f);

        
        shootTimer = 0;
    }
}
