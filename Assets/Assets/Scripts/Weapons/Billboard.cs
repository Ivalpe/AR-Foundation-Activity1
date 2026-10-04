using UnityEngine;

public class Billboard : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Vector3 targetAngle = Vector3.zero;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.LookAt(Camera.main.transform);
        //targetAngle = transform.localEulerAngles;
        //targetAngle.y = -targetAngle.y;
        //transform.localEulerAngles = targetAngle;
    }
}
