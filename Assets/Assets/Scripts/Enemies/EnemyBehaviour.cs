using System;
using System.Collections;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    public string favFood = "Food";

    public AudioClip omnomSFX;
    public AudioClip[] animalSFX;
    AudioClip eatSFX;
    AudioSource animalSource;
    Animator animator;
    //Rigidbody rb;
    GameObject target;

    private GameObject normalSkin, fnafSkin;

    NavMeshAgent agent;

    public bool airborne = false;

    LightEstimationManager lightEstScript;
    [SerializeField] private float lightTreshold = 0.2f;
    
    
    float speed = 0.0f;

    private void Start()
    {
        currentHealth = maxHealth;
        animalSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        //rb = GetComponent<Rigidbody>(); --> rb isn't used with navmesh
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindWithTag("MainCamera");
        lightEstScript = GameObject.FindAnyObjectByType<LightEstimationManager>();

        normalSkin = transform.Find("root").gameObject;
        if (normalSkin)
        {
            normalSkin.SetActive(true); 
            //Debug.Log(gameObject.name + " normal skin FOUND");
        }
        else Debug.Log("Normal skin not found in " + gameObject.name);
        
        fnafSkin = transform.Find("fnafRoot").gameObject;
        if (fnafSkin)
        {
            //Debug.Log(gameObject.name + " FNAF skin FOUND");
            fnafSkin.SetActive(false);
        }
        else Debug.Log("FNAF skin not found in " + gameObject.name);


        BehaviorGraphAgent behaviourGraph = GetComponentInChildren<BehaviorGraphAgent>();
        if (behaviourGraph != null)
        {
            behaviourGraph.BlackboardReference.SetVariableValue("Target", target);
            GameObject targetVar;
            behaviourGraph.BlackboardReference.GetVariableValue("Target", out targetVar);
            Debug.Log("Behaviour Graph Agent component FOUND, set target to " +  targetVar.name);
            

        }
        else
        {
            
            Debug.Log("Behaviour Graph Agent component not found");
        }
    }

    void Update()
    {
        //if (lightEstScript) Debug.Log("Light object: " + lightEstScript.gameObject.name);

        if (agent && agent.enabled)
        {
            if (target)
            {
                agent.SetDestination(target.transform.position);

            }
            speed = Vector3.Magnitude(agent.velocity);

            
        }
        else
        {
            //Debug.Log("WARNING: NavMeshAgent Component in " + gameObject.name + " is disabled or missing")
            speed = Vector3.Magnitude(GetComponent<Rigidbody>().linearVelocity);
            //Debug.Log(gameObject.name + " speed: " + speed);
        }

        if (animator) animator.SetFloat("speed", speed);

        float lightEst;
        if(!lightEstScript.brightness.HasValue)
        {
            lightEst = lightEstScript.testLight;
        }
        else
        {
            lightEst = lightEstScript.brightness.Value;
        }

        if(lightEst <= lightTreshold)
        {
            animator.enabled = false;
            normalSkin.SetActive(false);
            fnafSkin.SetActive(true);
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<CapsuleCollider>().enabled = true;
        }
        else
        {
            animator.enabled = true;
            normalSkin.SetActive(true);
            fnafSkin.SetActive(false);
            GetComponent<BoxCollider>().enabled = true;
            GetComponent<CapsuleCollider>().enabled = false;
        }
            

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Fruit") 
            || collision.gameObject.CompareTag("Meat") 
            || collision.gameObject.CompareTag("Sweet"))
        {
            if (currentHealth <= 0) return;

            if (collision.gameObject.CompareTag(favFood))
            {
                currentHealth -= 50f; //2-shot
                if (animalSFX.Length > 0)
                {
                    int rand = UnityEngine.Random.Range(0, animalSFX.Length);
                    eatSFX = animalSFX[rand];
                }
                else
                {
                    Debug.Log("WARNING: " + gameObject.name + " noises SFX array is empty");
                }
            }
            else
            {
                currentHealth -= 20.0f; //5-shot
                eatSFX = omnomSFX;
                
            }


            if(animator)animator.SetTrigger("Ate");
            else
            {
                //Debug.Log("WARNING: Animator on " + gameObject.name + " not found");
            }

            if(animalSource && eatSFX) animalSource.PlayOneShot(eatSFX);
            collision.gameObject.SetActive(false);

            if (currentHealth <= 0)
            {
                StartCoroutine(GotFed());
            }

            

        }
    }
    

    IEnumerator GotFed()
    {
        if(agent && agent.enabled) agent.isStopped = true;
        yield return new WaitForSeconds(3.0f);
      
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
