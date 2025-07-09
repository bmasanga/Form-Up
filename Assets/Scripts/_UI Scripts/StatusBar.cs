using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Gradient gradient;
    [SerializeField] Image fill;

    private IStatusProvider statusProvider;
    
    void Awake()
    {
        statusProvider = GetComponent<IStatusProvider>();
    }

    void Start()
    {
        if (statusProvider != null)
        {
            slider.maxValue = statusProvider.GetMaxValue();
            slider.value = statusProvider.GetCurrentValue();
            fill.color = gradient.Evaluate(slider.normalizedValue);
        }
    }

    void Update()
    {
        if (statusProvider != null)
        {
            slider.value = statusProvider.GetCurrentValue();
            fill.color = gradient.Evaluate(slider.normalizedValue);
        }
    }

}
