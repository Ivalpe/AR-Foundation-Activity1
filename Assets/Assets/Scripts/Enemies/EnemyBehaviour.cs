using NUnit;
using System;
using System.Collections;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;
using UnityEngine.XR.ARSubsystems;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    public string favFood = "Food";

    public AudioClip omnomSFX;
    public AudioClip[] animalSFX;
    public AudioClip[] fnafSFX;
    AudioClip eatSFX;
    AudioSource animalSource, musicSource;
    Animator animator;
    //Rigidbody rb;
    GameObject target;

    private GameObject normalSkin, fnafSkin;

    NavMeshAgent agent;
    float fadeTimer = 0;
    public float fadeDuration = 1.0f;

    

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
        musicSource = target.GetComponent<AudioSource>();
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


        //BehaviorGraphAgent behaviourGraph = GetComponentInChildren<BehaviorGraphAgent>();
        //if (behaviourGraph != null)
        //{
        //    behaviourGraph.BlackboardReference.SetVariableValue("Target", target);
        //    GameObject targetVar;
        //    behaviourGraph.BlackboardReference.GetVariableValue("Target", out targetVar);
        //    Debug.Log("Behaviour Graph Agent component FOUND, set target to " +  targetVar.name);
            

        //}
        //else
        //{
            
        //    Debug.Log("Behaviour Graph Agent component not found");
        //}
    }

    void Update()
    {
        //if (lightEstScript) Debug.Log("Light object: " + lightEstScript.gameObject.name);

        if (agent && agent.enabled)
        {
            NavMeshHit hit;
            if (!agent.isOnNavMesh)
            {
                if (NavMesh.SamplePosition(transform.position, out hit, 30.0f, NavMesh.AllAreas))
                {
                    agent.Warp(hit.position);
                }
                else if (target != null)
                {
                    //Vector3 fallbackPos = new Vector3(transform.position.x, target.transform.position.y - 1.5f, transform.position.z);
                    //agent.enabled = false;
                    //transform.position = fallbackPos;
                    //agent.enabled = true;
                }
            }

            if (target /*&& agent.isOnNavMesh*/)
            {
                agent.SetDestination(target.transform.position);

            }
            speed = Vector3.Magnitude(agent.velocity);

            
        }
        else
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                speed = Vector3.Magnitude(rb.linearVelocity);
            }
        }

        if (animator) animator.SetFloat("speed", speed);

        float? lightEst = 0.5f;
        if(lightEstScript != null)
        {
            

            if (!lightEstScript.brightness.HasValue)
            {

                lightEst = lightEstScript.testLight;
            }
            else
            {
                lightEst = lightEstScript.brightness.Value;
            }
            
            
            

            if (lightEst <= lightTreshold)
            {
                animator.enabled = false;
                normalSkin.SetActive(false);
                fnafSkin.SetActive(true);
                GetComponent<BoxCollider>().enabled = false;
                GetComponent<CapsuleCollider>().enabled = true;
                FadePitch(1.0f, 0.5f);
                
            }
            else
            {
                animator.enabled = true;
                normalSkin.SetActive(true);
                fnafSkin.SetActive(false);
                GetComponent<BoxCollider>().enabled = true;
                GetComponent<CapsuleCollider>().enabled = false;
                FadePitch(0.5f, 1.0f);
                
            }
            
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
                    int rand = 0; 
                    if (normalSkin.activeSelf)
                    {
                        rand = UnityEngine.Random.Range(0, animalSFX.Length);
                        eatSFX = animalSFX[rand];
                    }
                    else if (fnafSkin.activeSelf)
                    {
                        UnityEngine.Random.Range(0, fnafSFX.Length);
                        eatSFX = fnafSFX[rand];
                    }
                    //else eatSFX = omnomSFX; //default fallback
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

            if(animalSource && eatSFX) 
                animalSource.PlayOneShot(eatSFX);
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

    void FadePitch(float initialVal, float finalVal)
    {
        //fadeTimer += Time.deltaTime;

        //if (fadeDuration <= 0) fadeDuration = 0.01f;

        //float t = Mathf.Min((fadeTimer / fadeDuration), 1.0f);
        //float newPitch = finalVal;
        //if (t >= finalVal)
        //{
        //    fadeTimer = 0;
        //}
        //else
        //{
        //    newPitch = initialVal + t * finalVal;
        //    Debug.Log("new pitch: " + newPitch);
        // 
        //}
        //WHAT IS HAPPENING HERE (´T o T`)

        //if(musicSource && musicSource.isPlaying)
        //{
        //    musicSource.pitch = newPitch;
        //}

        //thank you Mathf library T w T
        float newPitch = Mathf.MoveTowards(musicSource.pitch, finalVal, fadeDuration * Time.deltaTime);
        //Debug.Log("new pitch: " + newPitch);
        musicSource.pitch = newPitch;
    }
}
