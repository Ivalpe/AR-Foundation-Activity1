using UnityEngine;
using UnityEngine.XR.ARFoundation;
public class LightEstimationManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Range(0,1)]
    public float testLight = 0.5f;

    public ARCameraManager m_ARCamManager;
    Light m_Light;

    public float? brightness { get; private set; } // question mark allows a variable to be either its data type, or null (AKA makes it "nullable")

    void OnEnable()
    {
        m_ARCamManager.frameReceived += GetFrameLightInfo;
    }

    void OnDisable()
    {
        m_ARCamManager.frameReceived -= GetFrameLightInfo;
    }
    void Start()
    {
        m_Light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        m_Light.intensity = testLight;
    }

    void GetFrameLightInfo(ARCameraFrameEventArgs args)
    {
        //Debug.Log("Current Light Estimation: " + args.lightEstimation.averageBrightness);
        if (args.lightEstimation.averageBrightness.HasValue)
        {
            brightness = args.lightEstimation.averageBrightness.Value;
            m_Light.intensity = brightness.Value;
            //Debug.Log("Current Frame Light Brightness: " + brightness);
        }
        else
        {
            //Debug.Log("Unable to compute average brightness");
            brightness = null;
        }
    }
}
