using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScript : MonoBehaviour
{
    public float Splash_scale = 1f;
    public ParticleSystem[] color_part;
    public ParticleSystem main_splash;
    public Vector3 offset;
    [Tooltip("u can adjust darkness here")]
    public float effect_intensity;
    public Light directionlight;
    private float pr_intensity;
    public static SplashScript main;
    public Color lightnormalcolor;
    public Color lighteffectcolor;
    private void Awake()
    {
        main = this;
    }
    void Start()
    {
        main_splash.transform.localScale = Vector3.one * Splash_scale;
        pr_intensity = directionlight.intensity;
        directionlight.color = lightnormalcolor;
    }
    public void PlayEffect(Vector3 _point,Color color)
    {
        main_splash.transform.localScale = Vector3.one * Splash_scale;
        transform.position = _point+ offset;
        foreach (var item in color_part)
        {
            var _main = item.main;
            _main.startColor = color;
        }
        main_splash.Play();
    }
    public void DimLight()
    {
        directionlight.intensity = effect_intensity;
        directionlight.color = lighteffectcolor;
    }
    public void ResetLight()
    {
        directionlight.intensity = pr_intensity;
        directionlight.color = lightnormalcolor;
    }
}
