using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PalletMatData
{
    public Material skybox;
    public Material player_mat;
    public Material cylinder_mat;
    public Material[] circle_mats;
    public Material[] enemies_mats;
    public Material FillColored_mat;
    public Material FillColoredWithSuperpower_mat;
    public Material BulletSimple_mat;
    public Material BulletWithSuperPower_mat;
    public Material BulletSimpleCollisionDestroy_mat;
    public Material BulletWithSuperPowerCollisionDestroy_mat;
    public Material Cube1;
    public Material Cube2;
    public Material Cube3;
    public Color ProgresBarFirstColor_mat;
    public Color ProgresBarFillColor_mat;
    public Color Combomsg_mat;
    public Color Perfect_mat;
    public Color coineffect_mat;
    public Color MuzzleFireBallFire_color;
    public Color MuzzleFireBallTrail_color;
    public Color Explosion_Circle_color;
    public int Combomsgpos;
    public int Perfectmsgpos;
    public int Coineffectuipos;
}
public class PalletMat : MonoBehaviour
{
    public PalletMatData[] pallet_mat;
    public static PalletMat main;
    private void Awake()
    {
        main = this;
    }
}
