using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldBar : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] OldShield shield;
    [SerializeField] Gradient gradient;
    [SerializeField] Image fill;

    
    void Start()
    {
        slider.maxValue = shield.GetMaxHP();
        slider.value = shield.GetMaxHP();
        fill.color = gradient.Evaluate(1f);
    }

    void Update()
    { 
        slider.value = shield.GetCurrentHP();
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
