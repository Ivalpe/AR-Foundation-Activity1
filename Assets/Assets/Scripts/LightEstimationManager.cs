using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(Light))]
public class LightEstimationManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Range(0, 1)]
    public float testLight = 0.5f;

    public ARCameraManager m_ARCamManager;
    public TextMeshProUGUI brightnessText;
    public TextMeshProUGUI colorTempText;
    Light m_Light;
    public float? brightness { get; private set; } // question mark allows a variable to be either its data type, or null (AKA makes it "nullable")

    public float? colorTemp { get; private set; } 

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
        m_Light.intensity = testLight; //for editor
        DisplayDebugInfo();
        
    }

    void GetFrameLightInfo(ARCameraFrameEventArgs args)
    {
        //Debug.Log("Current Light Estimation: " + args.lightEstimation.averageBrightness);
        if (args.lightEstimation.averageBrightness.HasValue)
        {
            brightness = args.lightEstimation.averageBrightness.Value;
            m_Light.intensity = brightness.Value;  
        }
        else
        {
          
            brightness = null;
        }

        if (args.lightEstimation.averageColorTemperature.HasValue)
        {
            colorTemp = args.lightEstimation.averageColorTemperature.Value;
            m_Light.colorTemperature = colorTemp.Value;
        }
        else
        {
            colorTemp = null;
        }

        

        
    }

    void DisplayDebugInfo()
    {
        brightnessText.text = brightness.HasValue ? brightness.ToString() : "Brightness Info Unavailable";
        colorTempText.text = colorTemp.HasValue ? colorTemp.ToString() : "Color Temperature Info Unavailable";
    }


}