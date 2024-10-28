using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfectShotTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            GamePlay.main.IsPerfectShoot = true;
        }
    }
}
