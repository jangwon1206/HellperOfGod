using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr : MonoBehaviour
{
    public GameObject _first_Point = null;
    public GameObject _Seceond_Point = null;
    public GameObject _third_Point = null;

    private float first_Point_Value = 0f;
    private float Seceond_Point_Value = 0f;
    private float third_Point_Value = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SkillOn(_first_Point,ref first_Point_Value);
        SkillOn(_Seceond_Point,ref Seceond_Point_Value);
        SkillOn(_third_Point,ref third_Point_Value);
    }

    void SkillOn(GameObject skillPoint, ref float value)
    {
        if (skillPoint.activeSelf == true)
        {
            if (value >= 180f)
            {
                value = 0f;
            }

            skillPoint.transform.rotation = Quaternion.Euler(0f, 0f, value);

            value += Time.deltaTime*50f;
        }
        else
        {
            value = 0f;
        }
    }
}
