using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using PathCreation;
public class CubeManagment : MonoBehaviour
{
    public int TotalShoot=5;
    public float duration = 1f;
    public float scale = 2f;
    public GameObject cube;
    public Transform point;
    public bool inAnim = false, AllPainted = false;
    private int shots;
    public void EnableIt()
    {
        inAnim = true;
        cube.SetActive(true);
        cube.transform.DOMove(point.position, duration);
        cube.transform.DOScale(Vector3.one*scale, duration).OnComplete(() => inAnim = false);
        if (TotalShoot <= 0)
        {
                AllPainted = true;
            StartCoroutine(court());
            IEnumerator court()
            {
                foreach (var item in VFXManager.main.explodecubepart.GetComponentsInChildren<ParticleSystem>())
                {
                    item.Play();
                }
                VFXManager.main.explodecubepart.transform.localScale = Vector3.one * scale;
                VFXManager.main.explodecubepart.transform.position = cube.transform.position;
                VFXManager.main.explodecubepart.transform.rotation = cube.transform.rotation;
                cube.SetActive(false);
                yield return new WaitForSeconds(GamePlay.main.effect_duration);
                GamePlay.main.TileFilled();
                print("cube filled");
            }
        }
    }
    public void DoAnim(bool ispower)
    {
        if (inAnim) return;
        inAnim = true;
        Vector3 lastscale = Vector3.one*scale;
        SplashScript.main.PlayEffect(transform.position, ispower ? GamePlay.current_lvl_mat.FillColoredWithSuperpower_mat.color : GamePlay.current_lvl_mat.FillColored_mat.color);
        cube.transform.DOLocalMoveZ(-0.2f, 0.1f);
        cube.transform.DOScale(lastscale * 1.2f, 0.1f).SetEase(Ease.InOutBack).OnComplete(() => {
            cube.transform.DOLocalMoveZ(0f, 0.3f);
            cube.transform.DOScale(lastscale, 0.3f).SetEase(Ease.InOutBack);
            // foreach (var item in obj.GetComponentsInChildren<MeshRenderer>())
            // {
            // }
        });
        shots++;
        StartCoroutine(court());
        IEnumerator court()
        {
            yield return new WaitForSeconds(0.02f);
            SoundsScript.main.PlayAudioEffect(ispower ? 0 : 1);
            VibrationAndShake.main.DoEffect(ispower ? 0 : 1);
            inAnim = false;
                AllPainted = shots>=TotalShoot;
            if (AllPainted)
            {
                yield return new WaitForSeconds(GamePlay.main.enemy_expode_duraion);
                foreach (var item in VFXManager.main.explodecubepart.GetComponentsInChildren<ParticleSystem>())
                {
                    item.Play();
                }
                VFXManager.main.explodecubepart.transform.localScale = Vector3.one * scale;
                VFXManager.main.explodecubepart.transform.position = cube.transform.position;
                VFXManager.main.explodecubepart.transform.rotation = cube.transform.rotation;
                cube.SetActive(false);
                yield return new WaitForSeconds(GamePlay.main.effect_duration);
                GamePlay.main.TileFilled();
                print("cube filled");
            }
        }
    }
}
