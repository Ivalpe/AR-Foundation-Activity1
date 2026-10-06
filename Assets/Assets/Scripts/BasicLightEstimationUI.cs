using UnityEngine.UI;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;


/// <summary>
/// A simple UI controller to display basic light estimation information.
/// </summary>
[RequireComponent(typeof(BasicLightEstimation))]
public class BasicLightEstimationUI : MonoBehaviour
{
    [Tooltip("The UI Text element used to display the estimated ambient intensity in the physical environment.")]
    [SerializeField]
    TextMeshProUGUI m_AmbientIntensityText;


    /// <summary>
    /// The UI Text element used to display the estimated ambient intensity value.
    /// </summary>
    public TextMeshProUGUI ambientIntensityText
    {
        get => m_AmbientIntensityText;
        set => m_AmbientIntensityText = value;
    }

    /*
     * Equivalent to:
     * public TextMeshProUGUI ambientIntensityText
        {
            get 
            {
                return m_AmbientIntensityText;
            }
            set 
            {
                m_AmbientIntensityText = value;
            }
        }
     */

    [Tooltip("The UI Text element used to display the estimated ambient color in the physical environment.")]
    [SerializeField]
    TextMeshProUGUI m_AmbientColorText;

    /// <summary>
    /// The UI Text element used to display the estimated ambient color in the scene.
    /// </summary>
    public TextMeshProUGUI ambientColorText
    {
        get => m_AmbientColorText;
        set => m_AmbientColorText = value;
    }

    void Awake()
    {
        m_BasicLightEstimation = GetComponent<BasicLightEstimation>();
    }

    void Update()
    {
        SetUIValue(m_BasicLightEstimation.brightness, ambientIntensityText);

        //Display color temperature or color correction if supported
        if (m_BasicLightEstimation.colorTemperature != null)
            SetUIValue(m_BasicLightEstimation.colorTemperature, ambientColorText);
        else if (m_BasicLightEstimation.colorCorrection != null)
            SetUIValue(m_BasicLightEstimation.colorCorrection, ambientColorText);
        else
            SetUIValue<float>(null, ambientColorText);
    }
        
    void SetUIValue<T>(T? displayValue, TextMeshProUGUI text) where T : struct
    {
        if (text != null)
            text.text = displayValue.HasValue ? displayValue.Value.ToString(): "Unavailable";
    }

    BasicLightEstimation m_BasicLightEstimation;
}

