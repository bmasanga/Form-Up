using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Health health;
    [SerializeField] Gradient gradient;
    [SerializeField] Image fill;

    
    void Start()
    {
        slider.maxValue = health.GetMaxHealth();
        slider.value = health.GetMaxHealth();
        fill.color = gradient.Evaluate(1f);
    }

    void Update()
    { 
        slider.value = health.GetCurrentHealth();
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
    
    // public void SetMaxHealth(int health)
    // {
    //     slider.maxValue = health;
    //     slider.value = health;
    // }
    
    // public void SetHealth(int health)
    // {
    //     slider.value = health;
    // }

}
