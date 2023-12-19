using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[AddComponentMenu("UI/SliderSimplifier", 34)]
public class SliderSimplifier : MonoBehaviour
{
    [Header("References")]
    public Slider slider;
    public Image SliderBG;

    private Image SliderHandle;
    private Image SliderFill;


    [Header("Configurations")]
    public Color BackgroundColor = Color.white;
    public Color FillColor = Color.white;
    public Color HandleColor = Color.white;
    public bool useHandle = true;
    [Space]
    [Tooltip("Click the settings in the top right corner of the script and click 'Set value' to set the value of the slider.")]
    public float value;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        if (slider == null)
        {
            slider = GetComponentInChildren<Slider>();
        }
        if (slider != null)
        {
            Transform bg = slider.transform.Find("Background");
            
            if(bg != null)
            {
                SliderBG = bg.GetComponent<Image>();
            }
                
        }
        if (slider == null)
        {
            Debug.LogError("Error! Slider non existant. Manual Reference required");
            return;
        }
        Setup();
    }
    [ContextMenu("Manual Setup")]
    void Setup()
    {
        SliderHandle = slider.handleRect.GetComponent<Image>();
        SliderFill = slider.fillRect.GetComponent<Image>();
    }
    [ContextMenu("Set Value")]
    void SetValue()
    {
        slider.value = value;
    }
    // Update is called once per frame
    void Update()
    {
        SliderBG.color = BackgroundColor;
        SliderHandle.color = HandleColor;
        SliderFill.color = FillColor;

        if (useHandle)
        {
            SliderHandle.gameObject.SetActive(true);
        }
        else
        {
            SliderHandle.gameObject.SetActive(false);
        }
    }
}
