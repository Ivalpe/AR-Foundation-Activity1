using UnityEngine;

public class Lifecollider : MonoBehaviour
{
    private Vector3 requiredLocalPos;
    private Quaternion requiredLocalRot;

    void Awake()
    {
        requiredLocalPos = transform.localPosition;
        requiredLocalRot = transform.localRotation;
       
    }

    void Update()
    {
        
            //print("The transform has changed!");
            //transform.localPosition = requiredLocalPos;
            transform.localRotation = requiredLocalRot;
            //transform.hasChanged = false;
        
    }
}
