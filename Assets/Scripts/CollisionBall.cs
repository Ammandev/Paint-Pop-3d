using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBall : MonoBehaviour
{
    public MeshRenderer[] meshes;
    public void setMat(Material mat)
    {
        foreach (var item in meshes)
        {
            item.sharedMaterial = mat;
        }
    }
}