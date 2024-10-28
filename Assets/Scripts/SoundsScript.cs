using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class AudioEffect
{
    public string name;
    [Range(0,1f)]
    public float volume = 1f;
    public AudioClip clip;
    public AudioSource source;
}
public class SoundsScript : MonoBehaviour
{
    public AudioEffect[] Effects;
    public static SoundsScript main;
    private void Awake() {main = this; }
    public void PlayAudioEffect(int _index)
    {
        if (!Setting.Music) return;
        Effects[_index].volume = Effects[_index].volume;
        Effects[_index].source.PlayOneShot(Effects[_index].clip);
    }
}