using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDisplay : MonoBehaviour
{
    
    [Header("General")]
    [SerializeField] Color32 goodStatusColor = new Color32 (255, 255, 255, 200);
    [SerializeField] Color32 okStatusColor = new Color32 (150, 150, 0, 200);
    [SerializeField] Color32 badStatusColor = new Color32 (255, 100, 0, 200);
    [SerializeField] Color32 criticalStatusColor = new Color32 (255, 0, 0, 200);
    
    [Header("Shield Meter")]
    [SerializeField] Shield shield;
    [SerializeField] Slider shieldMeter;
    [SerializeField] TextMeshProUGUI shieldStatus;

    [Header("Health Meter")]
    [SerializeField] Health health;
    [SerializeField] Slider healthMeter;
    [SerializeField] Image healthMeterFillImage;
    [SerializeField] TextMeshProUGUI healthStatus;
    
    [Header("Cooldown Meter")]
    [SerializeField] Cannon cannon;
    [SerializeField] Slider cooldownMeter;
    [SerializeField] Image cooldownMeterFillImage;
    [SerializeField] TextMeshProUGUI heatStatus;
    // [SerializeField] Color32 lowHeatColor = new Color32 (0, 150, 0, 200);
    // [SerializeField] Color32 mediumHeatColor = new Color32 (150, 150, 0, 200);
    // [SerializeField] Color32 highHeatColor = new Color32 (255, 100, 0, 200);
    // [SerializeField] Color32 overHeatedColor = new Color32 (255, 0, 0, 200);
   


    // void Awake()
    // {
    //     cooldownMeterFillImage = cooldownMeter.fillRect.GetComponent<Image>();
    // }

    void Start()
    {
        cooldownMeter.maxValue = cannon.GetMaxHeat();
        shieldMeter.maxValue = shield.GetMaxHP();
        healthMeter.maxValue = health.GetMaxHealth();
    }


    void Update()
    {
        shieldMeter.value = shield.GetCurrentHP();
        healthMeter.value = health.GetCurrentHealth();
        cooldownMeter.value = cannon.GetCurrentHeat();
        
        SetShieldStatus();
        SetHealthStatus();
        SetCooldownStatus();
    }

    void SetShieldStatus()
    {
        bool deactivatedState = shield.GetisActive();

        if (!deactivatedState)
        {
            shieldStatus.text = "OFFLINE";
            shieldStatus.color = criticalStatusColor;
        }
        else
        {
            shieldStatus.text = "ACTIVE";
            shieldStatus.color = goodStatusColor;
        }
    }
    
    void SetHealthStatus()
    {

        if (healthMeter.value < .25f * healthMeter.maxValue)
        {
            healthStatus.text = "CRITICAL";
            healthStatus.color = criticalStatusColor;
            healthMeterFillImage.color = criticalStatusColor;
        }
        else if (healthMeter.value <= .75f * healthMeter.maxValue)
        {
            healthStatus.text = "DAMAGED";
            healthStatus.color = okStatusColor;
            healthMeterFillImage.color = okStatusColor;

        }
        else
        {
            healthStatus.text = "NORMAL";
            healthStatus.color = goodStatusColor;
            healthMeterFillImage.color = goodStatusColor;
        }
    }
    
    
    void SetCooldownStatus()
    {
        bool overheatedState = cannon.GetOverheatedState();

        if (overheatedState)
        {
            cooldownMeterFillImage.color = criticalStatusColor;
            heatStatus.text = "OVERHEATED";
            heatStatus.color = criticalStatusColor;
        }
        else if (cooldownMeter.value < 0.25f * cooldownMeter.maxValue)
        {
            cooldownMeterFillImage.color = goodStatusColor;
            heatStatus.text = "NORMAL";
            heatStatus.color = goodStatusColor;
        }
        else if (cooldownMeter.value <= 0.75f * cooldownMeter.maxValue)
        {
            cooldownMeterFillImage.color = okStatusColor;
        }
        else
        {
            cooldownMeterFillImage.color = badStatusColor;
        }

    }
}
