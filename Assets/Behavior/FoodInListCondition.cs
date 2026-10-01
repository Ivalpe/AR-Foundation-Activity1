using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "foodInList", story: "The [food] the enemy touched is in the [foods] list", category: "Conditions", id: "479c0a7e824a44aaf6f7be581949ec3b")]
public partial class FoodInListCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Food;
    [SerializeReference] public BlackboardVariable<List<GameObject>> Foods;

    public override bool IsTrue()
    {
        return true;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
