using DG.Tweening;
using PathCreation;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Gun : MonoBehaviour
{
	public float Speed = 3f;
	public float pathduration2 = 3f;

    [Tooltip("delay to shoot next bullet")]
    public float fire_rate = 0.1f;
	public Transform shoot_point;
	[Tooltip("Current bullet which gun just shoot u can access before it hit and destroyed")]
    public static Bullet bullet;
    public MeshRenderer meshRenderer;
	public ParticleSystem powerfire;
	public Transform body;
	float timer = 0.0f;
	bool inAnim = false;
			bool pressed =false;
    PlayerAnim anim_val;
	PlayerMove PlayerMove;
	//private Animator anim;
    private void Start()
	{
	//	anim = GetComponentInChildren<Animator>();
        anim_val = FindObjectOfType<PlayerAnim>();
		PlayerMove = GetComponent<PlayerMove>();
		DoIdleAnim();
	}

    public void MoveTo()
	{
		float followtimer = 0.0f;
		inAnim = true;
		Vector3 point = GamePlay.current_circle.transform.TransformPoint(GamePlay.current_circle.gun_shootpos);
		if (GamePlay.current_circle.pathCreator != null)
		{
			DOTween.To(() => followtimer, x => followtimer = x, 37f, pathduration2).OnUpdate(() =>
		{
			Vector3 followpoint = GamePlay.current_circle.pathCreator.path.GetPointAtDistance(followtimer, EndOfPathInstruction.Stop);
			followpoint.y = point.y;
			transform.position = followpoint;
			 Vector3 angles = GamePlay.current_circle.pathCreator.path.GetRotationAtDistance(followtimer, EndOfPathInstruction.Stop).eulerAngles;
			angles.z = 0;
			transform.rotation = Quaternion.Euler(angles);
		}).OnComplete(() => {
            Vector3 direction = new Vector3(GamePlay.current_circle.transform.position.x - point.x, 0, GamePlay.current_circle.transform.position.z - point.z).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = lookRotation;
            transform.DOMove(point, 0.1f).OnComplete(() =>
            {
                inAnim = false;
                PowerBar.bar.Transaction = false;
				if(GamePlay.current_circle.road)
				GamePlay.current_circle.road.SetActive(false);
            });
        });
			print("path following");
		}
		else
		{
            Vector3 direction = new Vector3(GamePlay.current_circle.transform.position.x - transform.position.x, 0, GamePlay.current_circle.transform.position.z - transform.position.z).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = lookRotation;
            transform.DOMove(point,1f/ PlayerMove.MovementSpeed).OnComplete(() =>
			{
				inAnim = false;
				PowerBar.bar.Transaction = false;
			});
			print("target following");
        }
    }
    Sequence mySequence;
	bool isup = false;
    public void DoIdleAnim()
	{
		isup = !isup;
		mySequence = DOTween.Sequence();
		mySequence.Append(body.DOLocalMove(anim_val.idle_pos* (isup?-1f:1f), anim_val.idle_time).SetEase(Ease.Flash).OnComplete(() => DoIdleAnim()));
    }
	public void DoShootAnim()
	{
		body.DOLocalMove(anim_val.shoot_pos, anim_val.shoot_anim_time / 2f).SetEase(Ease.Flash).OnComplete(()=>
		body.DOLocalMove(Vector3.zero, anim_val.shoot_anim_time/2f).SetEase(Ease.Flash));
    }
	public void DoPowerShootAnim()
	{
		body.DOLocalMove(anim_val.power_shoot_pos, anim_val.power_shoot_anim_time/2f).SetEase(Ease.Flash).OnComplete(() =>
        body.DOLocalMove(Vector3.zero, anim_val.power_shoot_anim_time / 2f).SetEase(Ease.Flash));
    }
	public void CancelIdleAnim()
	{
		mySequence.Kill();
    }
    private void Update()
	{
		if (!GamePlay.InGame) return;
		if (GamePlay.main.CanShoot() && bullet == null&& !inAnim)
		{
			if (!PowerBar.bar.PowerFilled)
			{
                if (GamePlay.main.IsPressed && !pressed)//first press it called once u clicked
                {
                    timer = 0.0f;
                    PowerBar.bar.StopUnloadingPower();
                    pressed = true;
                    //	CancelIdleAnim();
                }
                if (!GamePlay.main.IsPressed && pressed)//it will be called u stop pressing
                {
                    pressed = false;
                    //	DoIdleAnim();
                }
			}
			else
			{
                if (PowerBar.bar.PowerFilled && !pressed)//first press it called once u clicked
                {
                    timer = 0.0f;
                    PowerBar.bar.StopUnloadingPower();
                    pressed = true;
                    //	CancelIdleAnim();
                }
                if (!PowerBar.bar.PowerFilled && pressed)//it will be called u stop pressing
                {
                    pressed = false;
                    //	DoIdleAnim();
                }
            }

			if (pressed)//this will execute in every frame when mouse is down
            {
             //   GamePlay.main.starttimer = false;
                timer -= Time.deltaTime;
				if (timer <= 0.0f)//if power filled then bullet will shoot similar to circle power color index
					CreateAmmo(PowerBar.bar.PowerFilled? GamePlay.current_lvl_mat.BulletWithSuperPower_mat: GamePlay.current_lvl_mat.BulletSimple_mat);
            }
            else
			{
				PowerBar.bar.StartUnloadingPower();
            }
        }
	}
	public void CreateAmmo(Material _mat)//Ammo will be create and shoot in forward direction on call
    {
		print("ammo created");
		GameObject _bullet = PowerBar.bar.PowerFilled ? VFXManager.main.GetFireBullet():  VFXManager.main.GetBullet();
		_bullet.transform.position = shoot_point.position;
		_bullet.transform.rotation = shoot_point.rotation;
		_bullet.SetActive(true);
		bullet = _bullet.GetComponent<Bullet>();
		bullet.DoMove(shoot_point.forward*Speed);
		bullet.SetMat(_mat);
		timer = fire_rate;
        if (!PowerBar.bar.PowerFilled)
            PowerBar.bar.Power++;
		//anim.CrossFadeInFixedTime(GamePlay.main.PowerFilled ? "power_shoot" : "shoot", 0.01f);
		if (PowerBar.bar.PowerFilled) {
            var _main = powerfire.main;
            _main.startColor = GamePlay.current_circle.ReturnColor();
			powerfire.Play();
		}

        if (PowerBar.bar.PowerFilled) DoShootAnim(); else DoPowerShootAnim();
	}
}
