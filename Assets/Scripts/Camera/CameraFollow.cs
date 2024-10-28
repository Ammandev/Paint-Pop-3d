using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    public float smoothfollow = 0.125f;
    public Vector3 offset;
    public Vector3 rot_offset;
    private void LateUpdate()
    {
        Vector3 desiredposition = Target.position + Target.rotation*offset;
        Vector3 smoothposition = Vector3.Lerp(transform.position, desiredposition, smoothfollow);
        Quaternion smoothrotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rot_offset + Target.eulerAngles), smoothfollow);
        transform.position = smoothposition;
        transform.rotation = smoothrotation;
    }
}
