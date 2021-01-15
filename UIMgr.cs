using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMgr : MonoBehaviour
{

    private PlayerController _Player = null;
    public Slider _HealthBar = null;
    public Slider _StaminaBar = null;

    public float health_MaxGauge = 0f;
    public float health_Gauge = 0f;
    public float Stamina_MaxGauge = 0f;
    public float Stamina_Gauge = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _Player = FindObjectOfType<PlayerController>();

        health_MaxGauge = InformationValue.Instance.player_MaxHealth;

        _HealthBar.maxValue = health_MaxGauge;
        _HealthBar.value = _HealthBar.maxValue;


        Stamina_MaxGauge = InformationValue.Instance.player_MaxStamina;
        _StaminaBar.maxValue = Stamina_MaxGauge;
        _StaminaBar.value = _StaminaBar.maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (_Player.runing == true)
        {
            Stamina_Gauge -= 10f * Time.deltaTime;
            
        }
        else
        {
            if(Stamina_Gauge >= 100f)
            {
                Stamina_Gauge = Stamina_MaxGauge;
            }
            else
            {
                Stamina_Gauge += 10f * Time.deltaTime;
            }
        }

        _StaminaBar.value = Stamina_Gauge;
        _HealthBar.value = health_Gauge;
    }

    
}
