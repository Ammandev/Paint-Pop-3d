using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FPSCounter : MonoBehaviour
{
    /* Assign this script to any object in the Scene to display frames per second */

    public float updateInterval = 0.5f; //How often should the number update
    public TextMeshProUGUI fpstext;
    float accum = 0.0f;
    int frames = 0;
    float timeleft;
    float fps;


    // Use this for initialization
    void Start()
    {
        timeleft = updateInterval;
    }

    // Update is called once per frame
    void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        // Interval ended - update GUI text and start new interval
        if (timeleft <= 0.0)
        {
            // display two fractional digits (f2 format)
            fps = (accum / frames);
            timeleft = updateInterval;
            accum = 0.0f;
            frames = 0;
        }
        fpstext.text = fps.ToString("F2") + "FPS";
    }
}