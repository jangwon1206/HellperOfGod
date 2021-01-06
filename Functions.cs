using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Functions : MonoBehaviour
{
    public void Ani_Play(Animator ani, string name , bool value)
    {
        ani.SetBool(name, value);
    }

}
