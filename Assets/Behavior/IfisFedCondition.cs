using System;
using System.Diagnostics.Tracing;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IfisFed", story: "[Agent] gets [Food]", category: "Conditions", id: "c1702264e55a61a6363d9d90f9d8ebfb")]
public partial class IfisFedCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Food;
    bool collisionfood = false;
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter:" + collision.gameObject, collision.gameObject);
       //if(collision.gameObject.CompareTag("Foodprueba")) {collisionfood = true; }
      

       // // Pots obtenir un script component que tinguis a l'agent que controli les col·lisions
       // var collisionDetector = Agent.Value.GetComponent<MyCollisionDetector>();
       // Agent.Value.GetComponent.
       // if (collisionDetector != null && collisionDetector.HasHitSomething)
       // {
       //     // Reaccionem a la col·lisió detectada pel trigger del mateix Agent
       //     return Status.Success;
       // }


    }
    public override bool IsTrue()
    {
        if (collisionfood==true)
        {
            return true;
        }
        else { return false; }
    }

    public override void OnStart()
    {
       
       
        
    }

    public override void OnEnd()
    {
    }
}
