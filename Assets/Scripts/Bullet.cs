using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Bullet : MonoBehaviour
{
    public bool FireBall;
    public GameObject effect;
    public MeshRenderer rend;
  //  public ParticleSystem[] fireballpart;
  //  public ParticleSystem[] firetrailpart;
    public Rigidbody rigid;
    private void Start()
    {
        //   Destroy(gameObject, 5f);
     /*   if (FireBall)
        {
            foreach (var item in fireballpart)
            {
                var main2 = item.main;
                main2.startColor = GamePlay.current_lvl_mat.MuzzleFireBallFire_color;
            } 
            foreach (var item in firetrailpart)
            {
                var main2 = item.main;
                main2.startColor = GamePlay.current_lvl_mat.MuzzleFireBallTrail_color;
            }
        }*/
    }
    public void DestroyME()
    {
        if (FireBall)
            VFXManager.main.UnQueueFireBullet(gameObject);
        else
            VFXManager.main.UnQueueBullet(gameObject);
            Gun.bullet = null;
        Collided = false;
    }
    public void DoMove(Vector3 dir)
    {
        Invoke("DestroyME", 2f);
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        rigid.AddForce(dir,ForceMode.Force);
    }
    bool Collided = false;
    private void OnCollisionEnter(Collision other)
    {
        if (Collided) return;
        if (!GamePlay.InGame) return;
        if (other.gameObject.CompareTag("Plate"))
        {
            if (other.gameObject.GetComponentInParent<Circle>() != GamePlay.current_circle) return;
        //    GamePlay.current_circle.DoAnim(other.gameObject.transform.parent.gameObject);
            GamePlay.current_circle.DoAnim(other.gameObject, FireBall);
            if (GamePlay.IsBonusLevel)
            {
                float delay = 0.0f;
                for (int i = 0; i < GamePlay.main.pigbonus.CoinsOnShoot; i++)
                {
                    VFXManager.main.AnimateCoin(false, delay+=0.1f);
                }
            }
            else
                VFXManager.main.Animate();
            if (FireBall)
                VFXManager.main.AnimateFire(other.GetContact(0).point);
            Collided = true;
            DestroyME();
            print("collision entered");
        }else  
        if (other.gameObject.CompareTag("Obstacle"))
        {
            GamePlay.main.IsPerfectShoot = false;
            if (FireBall)
            {
            SoundsScript.main.PlayAudioEffect(2);
                Destroy(other.gameObject);
            }
            else
            {
                GameObject _effect = Instantiate(effect, other.GetContact(0).point, Quaternion.identity);
                _effect.GetComponent<CollisionBall>().setMat(FireBall ? GamePlay.current_lvl_mat.BulletWithSuperPowerCollisionDestroy_mat:GamePlay.current_lvl_mat.BulletSimpleCollisionDestroy_mat);
                foreach (Transform o in _effect.transform)
                {
                    o.GetComponent<Rigidbody>().AddForce(Vector3.forward * 20, ForceMode.Impulse);
                }
                print("GameOver");
                GamePlay.main.GameLose();
            }
            Collided = true;
            DestroyME();
        }
        else if (other.gameObject.CompareTag("PigOutside"))
        {
            Collided = true;
            DestroyME();
        }
        else if (other.gameObject.CompareTag("ShootableCube"))
        {
            //    GamePlay.current_circle.DoAnim(other.gameObject.transform.parent.gameObject);
            GamePlay.current_cube.DoAnim(FireBall);
                VFXManager.main.Animate();
            if (FireBall)
                VFXManager.main.AnimateFire(other.GetContact(0).point);
            Collided = true;
            DestroyME();
            print("collision entered");
        }
    }
    public void SetMat(Material mat)
    {
        rend.sharedMaterial = mat;
    }
}
