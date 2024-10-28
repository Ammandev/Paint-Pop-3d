using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using PathCreation;
using UnityEngine.Playables;
[System.Serializable]
public class plate
{
    public GameObject obj;
    public Rigidbody[] rigid;
    public bool ispainted = false;
    public plate(GameObject _obj)
    {
        obj = _obj;
        rigid = _obj.GetComponentsInChildren<Rigidbody>();
    }
}
public class Circle : MonoBehaviour
{
    public PathCreator pathCreator;
    public int Plates_Count=30;
    [Tooltip("increment Offset in rotation after every single shot")]
    public float rotOffset = 11.5f;
    [Tooltip("delay of plate popup animation")]
    public float wait = 0.1f;
    [HideInInspector]
    public List<plate> plates = new List<plate>();
    [Tooltip("Manual arrange plates of circle")]
    public bool Manual;
    public bool BonusSet;
    public GameObject bonuspig;
    public GameObject gun;
    public int maxhitcount = 30;
    public Transform plate_set;
 //   [HideInInspector]
    public bool inAnim = false, AllPainted = false;
    public Vector3 gun_shootpos;
    public int hitcount = 0;
    public GameObject cylinder;
    public GameObject road;
	IEnumerator co;
    IEnumerator court2(bool ispower)
    {
        hitcount++;
        yield return new WaitForSeconds(0.02f);
        SoundsScript.main.PlayAudioEffect(ispower ? 0 : 1);
        VibrationAndShake.main.DoEffect(ispower ? 0 : 1);
        inAnim = false;
        if (hitcount < maxhitcount)
        {
            AllPainted = false;

        }
        else
            AllPainted = true;
        if (AllPainted && GamePlay.InGame)
        {
            //  PowerBar.bar.Transaction = true;
            // GamePlay.current_lvl.enemies[GamePlay.main.currentcircle].FinishAnim();
            GamePlay.InGame = false;
            yield return new WaitForSeconds(GamePlay.main.enemy_expode_duraion);
            FinishAnim();
            yield return new WaitForSeconds(GamePlay.main.effect_duration);
            bonuspig.SetActive(false);
            gun.SetActive(false);
            VFXManager.main.chestanim.SetActive(true);
            ChestManagmentBonus chest = GamePlay.current_lvl.main.GetComponent<ChestManagmentBonus>();
            yield return new WaitForSeconds(2.5f);
            for (int i = 0; i < chest.CoinShowinAnimation; i++)
            {
                VFXManager.main.AnimateCoin(true);
                yield return new WaitForSeconds(0.1f);
            }
            print("called");
            GamePlay.main.Coin += chest.TotalCoins;
            yield return new WaitForSeconds(2f);
            GamePlay.main.TileFilled();
            gameObject.SetActive(false);
        }
    }
    public void DoAnim(GameObject obj,bool ispower)
    {
        if (inAnim) return;
        if (BonusSet) {
            inAnim = true;
            Vector3 lastscale2 = Vector3.one;
            SplashScript.main.PlayEffect(obj.transform.position+Vector3.down*1.25f, ispower ? GamePlay.current_lvl_mat.FillColoredWithSuperpower_mat.color : GamePlay.current_lvl_mat.FillColored_mat.color);
          //  obj.transform.DOLocalMoveZ(-0.2f, 0.1f);
            obj.transform.DOScale(lastscale2 * 1.4f, 0.1f).SetEase(Ease.InFlash).OnComplete(() => {
               // obj.transform.DOLocalMoveZ(0f, 0.3f);
                obj.transform.DOScale(lastscale2, 0.3f).SetEase(Ease.InOutBack);
            });
            print("anim played");
            if (co != null)
                StopCoroutine(co);
            co = court2(ispower);
		StartCoroutine(co);
            return;
        }
        inAnim = true;
        int index = plates.FindIndex(x => x.obj == obj);
        if (index != -1)
        {
            if (plates[index].ispainted) { inAnim = false; return; }
            plates[index].ispainted = true;
        }
        Vector3 lastscale = obj.transform.localScale;
        SplashScript.main.PlayEffect(obj.transform.position, ispower ? GamePlay.current_lvl_mat.FillColoredWithSuperpower_mat.color : GamePlay.current_lvl_mat.FillColored_mat.color);
        obj.transform.DOLocalMoveZ(-0.2f, 0.1f);
        obj.transform.DOScale(lastscale*1.2f, 0.1f).SetEase(Ease.InOutBack).OnComplete(() => {
            obj.transform.DOLocalMoveZ(0f, 0.3f);
            obj.transform.DOScale(lastscale, 0.3f).SetEase(Ease.InOutBack);
           // foreach (var item in obj.GetComponentsInChildren<MeshRenderer>())
           // {
                obj.GetComponent<MeshRenderer>().sharedMaterial = ispower ? GamePlay.current_lvl_mat.FillColoredWithSuperpower_mat : GamePlay.current_lvl_mat.FillColored_mat;
           // }
        });
            print("anim played");
        StartCoroutine(court());
        IEnumerator court()
        {
            Vector3 rot = transform.eulerAngles;
        float x = 30f / (float)Plates_Count;
            rot.z += rotOffset* x;
            transform.DORotate(rot, 0.1f);
            yield return new WaitForSeconds(0.02f);
            SoundsScript.main.PlayAudioEffect(ispower ? 0:1);
            VibrationAndShake.main.DoEffect(ispower ? 0:1);
            inAnim = false;
            foreach (var item in plates)
            {
                if (!item.ispainted)
                {
                    AllPainted = false;
                    break;
                }
                AllPainted = true;
            }
            if (AllPainted)
            {
                PowerBar.bar.Transaction = true;
                    GamePlay.current_lvl.items[GamePlay.main.currentcircle].enemy.FinishAnim();
                yield return new WaitForSeconds(GamePlay.main.enemy_expode_duraion);
                FinishAnim();
                yield return new WaitForSeconds(GamePlay.main.effect_duration);
                GamePlay.main.ActivateCube(this);
                print("Circle filled");
            }
        }
    }
    public void OnValidate()
    {
        if (BonusSet) return;
        if (Plates_Count > 40)
            Plates_Count = 40;
        float x = 30f / (float)Plates_Count;
        for (int i = 0; i < plate_set.childCount; i++)
        {
            if (i >= Plates_Count)
                plate_set.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < Plates_Count; i++)
        {
            var item = plate_set.GetChild(i);
            item.localScale = new Vector3(x, 1f, 1f);
            item.localRotation = Quaternion.Euler(0f, 0f, rotOffset * i * item.localScale.x);
            item.gameObject.SetActive(true);
        }
    }
    public void DoStartAnim()
    {
        if (BonusSet) return;
        if (Plates_Count > 40)
            Plates_Count = 40;
     /*   for (int i = 0; i < plate_set.childCount; i++)
        {
            var item = plate_set.GetChild(i);
            if (!Manual)
            {
                item.localRotation = Quaternion.Euler(0f, 0f, 0);
            }
            item.gameObject.SetActive(false);
        }*/
        for (int i = 0; i < Plates_Count; i++)
        {
            var item = plate_set.GetChild(i);
            float x = 30f / (float)Plates_Count;
            if (!Manual)
            {
            item.localScale = new Vector3(x, 1f, 1f);
                item.localRotation = Quaternion.Euler(0f, 0f, rotOffset * i * item.localScale.x);
            }
            plates.Add(new plate(item.gameObject));
            item.gameObject.SetActive(true);
        }
      //  StartCoroutine(anim());
    }
    public Color ReturnColor()
    {
       return GamePlay.current_lvl_mat.FillColoredWithSuperpower_mat.color;
    }
    IEnumerator anim()
    {
        int index = 0;
        while (index <= Plates_Count - 1)
        {
            var item = plate_set.GetChild(index);
            Vector3 currentscale = item.localScale;
            item.localScale = Vector3.one*0.1f;
            item.gameObject.SetActive(true);
            index++;
            item.DOScale(currentscale, wait);
            yield return new WaitForSeconds(wait);
        }
    }
    public void FinishAnim()
    {
        //  if (BonusSet) return;
        if (BonusSet)
        {
            foreach (var item in VFXManager.main.explodebonuspart.GetComponentsInChildren<ParticleSystem>())
            {
                item.Play();
            }
          //  VFXManager.main.explodebonuspart.transform.position = transform.position;
        }
        else
        {
            foreach (var item in VFXManager.main.explodepart.GetComponentsInChildren<ParticleSystem>())
            {
                item.Play();
            }
            VFXManager.main.explodepart.transform.position = transform.position;
            VFXManager.main.explodepart.transform.rotation = transform.rotation;
            if(cylinder)
            cylinder.SetActive(false);
        }

        plate_set.gameObject.SetActive(false);
      /*  foreach (var plate in plates)
        {
            foreach (var item in plate.rigid)
            {
                item.isKinematic = false;
                item.useGravity = false;
                item.AddExplosionForce(GamePlay.main.exp_force, transform.position, 500f);
                item.AddRelativeTorque(plate.obj.transform.up * GamePlay.main.exp_force);
            }
        }*/
    }
}
