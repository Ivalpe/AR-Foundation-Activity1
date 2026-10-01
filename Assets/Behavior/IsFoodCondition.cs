using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "isFood", story: "GameObject that collided with enemy has a food tag", category: "Conditions", id: "a7e1be4ae666d11b1579cf7bebf57fe4")]
public partial class IsFoodCondition : Condition
{

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
