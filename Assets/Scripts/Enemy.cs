using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Vector3 rotspeed;
    bool finished = false;

    void Update()
    {
        if (!GamePlay.InGame|| finished) return;
        transform.Rotate(rotspeed);
    }
    public void FinishAnim()
    {
        finished = true;
        foreach (var item in GetComponentsInChildren<Rigidbody>())
        {
            item.isKinematic = false;
            item.useGravity = false;
            item.AddExplosionForce(GamePlay.main.exp_force, transform.position, 500f);
            item.AddRelativeTorque(item.transform.parent.up * GamePlay.main.exp_force);
        }
    }
}
