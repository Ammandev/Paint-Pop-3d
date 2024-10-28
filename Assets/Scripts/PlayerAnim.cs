using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    [Tooltip("Position of animation(idle)")]
    public Vector3 idle_pos;
    [Tooltip("Time of animation(idle)")]
    public float idle_time = 0.25f;
    [Tooltip("Position of animation(shoot)")]
    public Vector3 shoot_pos;
    [Tooltip("Time of animation(shoot)")]
    public float shoot_anim_time = 0.25f;
    [Tooltip("Position of animation(power shoot)")]
    public Vector3 power_shoot_pos;
    [Tooltip("Time of animation(power shoot)")]
    public float power_shoot_anim_time = 0.25f;
}
