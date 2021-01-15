using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationValue
{
    public static InformationValue _instance = null;

    private InformationValue() { }

    public static InformationValue Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new InformationValue();
            }

            return _instance;
        }
    }

    public float player_MaxHealth = 100f;
    public float player_MaxStamina = 100f;
    
        
}
