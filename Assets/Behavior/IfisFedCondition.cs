using System;
using System.Diagnostics.Tracing;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IfisFed", story: "[Agent] gets [Food]", category: "Conditions", id: "c1702264e55a61a6363d9d90f9d8ebfb")]
public partial class IfisFedCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Food;

    Collider detectfood;
    public override bool IsTrue()
    {
        if (detectfood == true)
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
