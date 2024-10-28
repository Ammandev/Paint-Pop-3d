using UnityEngine;
using EZCameraShake;

public class ShakeMe : MonoBehaviour
{
    public float magn, rough, fadeIn, fadeOut;
    public void ShakeCam(float _magn)
    {
        magn = _magn;
        CameraShaker.Instance.ShakeOnce(magn, rough, fadeIn, fadeOut*magn);
    }
}
