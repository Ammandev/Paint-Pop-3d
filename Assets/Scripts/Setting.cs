using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : MonoBehaviour
{
    public static bool Music = true,Vibration = true; 
    public void ChangeMusic(bool v)
    {
        Music = v;
    } 
    public void ChangeVib(bool v)
    {
        Vibration = v;
    }
}
