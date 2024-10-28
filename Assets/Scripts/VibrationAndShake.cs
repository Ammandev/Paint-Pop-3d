using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ShakeEffect
{
    public string name;
    public int vibrate_milliseconds = 60;
    public float Shake_Cam_Intensity = 1f;
}
public class VibrationAndShake : MonoBehaviour
{
    public ShakeEffect[] Effects;
    public ShakeMe cam_shake;
    public static VibrationAndShake main;
    private void Awake()
    {
        main = this;
    }
    public void DoEffect(int _index)
    {
        cam_shake.ShakeCam(Effects[_index].Shake_Cam_Intensity);
        if (!Setting.Vibration) return;
        Vibration.Vibrate(Effects[_index].vibrate_milliseconds);
    }
}
