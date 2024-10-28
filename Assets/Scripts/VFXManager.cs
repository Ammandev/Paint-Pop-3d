using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class VFXManager : MonoBehaviour
{
    public GameObject coin2Prefab;
    public GameObject coinPrefab;
    public GameObject combotext;
    public GameObject perfecttext;
    public GameObject fireprefab;
    public GameObject explodepart;
    public GameObject explodecubepart;
    public GameObject chestanim;
    public GameObject explodebonuspart;
    public RectTransform _startpos;
    public RectTransform _coinstartpos;
    public RectTransform _coinstartchestpos;
    public RectTransform _cointargettpos;
    public Transform _parent;
    Queue<RectTransform> CoinsQueue = new Queue<RectTransform>();
    Queue<RectTransform> StarsQueue = new Queue<RectTransform>();
    Queue<GameObject> fireQueue = new Queue<GameObject>();
    Queue<GameObject> bulletQueue = new Queue<GameObject>();
    Queue<GameObject> bulletfireQueue = new Queue<GameObject>();
    public float duration;

    public Ease easeType;
    public float coinduration;
    public Ease coineaseType;
    public static VFXManager main;
    Vector3 comboanchorpos,perfectanchorpos;
    private void Awake() { main = this;  }
    private void Start()
    {
        comboanchorpos = combotext.GetComponent<RectTransform>().anchoredPosition;
        perfectanchorpos = perfecttext.GetComponent<RectTransform>().anchoredPosition;
        chestanim.SetActive(false);
    }
    public void PrepareStars()
    {
        combotext.SetActive(false);
        perfecttext.SetActive(false);
        RectTransform Star;
        for (int i = 0; i < 6; i++)
        {
            Star = Instantiate(coinPrefab).GetComponent<RectTransform>();
            Star.anchoredPosition = _startpos.anchoredPosition;
            Star.transform.localScale = Vector3.one;
            Star.transform.SetParent(_parent);
            Star.gameObject.SetActive(false);
            Star.GetComponentInChildren<TextMeshProUGUI>().color = GamePlay.current_lvl_mat.coineffect_mat;
            StarsQueue.Enqueue(Star);
        }
        RectTransform Star2;
        for (int i = 0; i < 10; i++)
        {
            Star2 = Instantiate(coin2Prefab).GetComponent<RectTransform>();
            Star2.anchoredPosition = _startpos.anchoredPosition;
            Star2.transform.localScale = Vector3.one;
            Star2.transform.SetParent(_parent);
            Star2.gameObject.SetActive(false);
            CoinsQueue.Enqueue(Star2);
        }
        GameObject fire;
        for (int i = 0; i <10; i++)
        {
            fire = Instantiate(fireprefab);
            fire.transform.localScale = Vector3.one;
            fire.transform.parent = transform;
            fire.gameObject.SetActive(false);
            fireQueue.Enqueue(fire);
        }
        CreateBullets();
    }
    public void CreateBullets()
    {
        GameObject fire;
        for (int i = 0; i < 8; i++)
        {
            fire = Instantiate(GamePlay.main.balls[0]);
            fire.transform.parent = transform;
            fire.gameObject.SetActive(false);
            bulletQueue.Enqueue(fire);
        }
        GameObject fire2;
        for (int i = 0; i < 12; i++)
        {
            fire2 = Instantiate(GamePlay.main.balls[1]);
            fire2.transform.parent = transform;
            fire2.gameObject.SetActive(false);
            bulletfireQueue.Enqueue(fire2);
        }
    }
    public void Animate()
    {
        if (StarsQueue.Count > 0)
        {
            RectTransform Star = StarsQueue.Dequeue();
            Star.gameObject.SetActive(true);
            Star.transform.localScale = Vector3.zero;
            Star.anchoredPosition = _startpos.anchoredPosition;
            GamePlay.main.NewScore+=(GamePlay.CurrentLevel+1)*(PowerBar.bar.PowerFilled? GamePlay.CurrentCombo:1);
            Star.GetComponentInChildren<TextMeshProUGUI>().text = "+" + (GamePlay.CurrentLevel + 1) * (PowerBar.bar.PowerFilled ? GamePlay.CurrentCombo : 1) 
                * (GamePlay.main.IsPerfectShoot ? GamePlay.main.PerfectMultiple : 1);
            if (PowerBar.bar.PowerFilled)
                AnimateCombo();
            if (GamePlay.main.IsPerfectShoot)
                Animateperfectshoot();
          GamePlay.main.IsPerfectShoot = false;
            Star.transform.DOScale(Vector3.one * 2f, duration/2f).SetEase(easeType);
            Star.DOAnchorPosY(GamePlay.current_lvl_mat.Coineffectuipos+ _startpos.anchoredPosition.y, duration).SetEase(easeType).OnComplete(() =>
            {
                Star.gameObject.SetActive(false);
                StarsQueue.Enqueue(Star);
            });
        }
    } 
    public void AnimateCoin(bool v= false, float delay= 0.0f)
    {
        if (CoinsQueue.Count > 0)
        {
            RectTransform Star = CoinsQueue.Dequeue();
            Star.gameObject.SetActive(true);
            Star.transform.localScale = Vector3.one * 0.75f;
            Star.anchoredPosition = v ? _coinstartchestpos.anchoredPosition : _cointargettpos.anchoredPosition;
            GamePlay.main.Coin += 1;///socre 
            Star.transform.DOScale(Vector3.one, coinduration).SetDelay(delay).SetEase(easeType);
            Star.DOAnchorPos(_cointargettpos.position, coinduration).SetDelay(delay).SetEase(coineaseType).OnComplete(() =>
            {
                Star.gameObject.SetActive(false);
                CoinsQueue.Enqueue(Star);
            });
        }
    }
    Sequence mySequence;
    Sequence mySequence3;
    public void AnimateCombo()
    {
        combotext.SetActive(false);
        if (mySequence == null)
            combotext.transform.localScale = Vector3.zero;
        else
        {
            combotext.GetComponent<RectTransform>().anchoredPosition = comboanchorpos;
            mySequence.Kill();
            mySequence3.Kill();
        }
        combotext.SetActive(true);
        combotext.GetComponentInChildren<TextMeshProUGUI>().text = GamePlay.main.ComboText + GamePlay.CurrentCombo;
        mySequence = DOTween.Sequence();
        mySequence3 = DOTween.Sequence();
        mySequence.Append(
        combotext.transform.DOScale(Vector3.one * 1f, 0.075f).SetEase(easeType));
        mySequence3.Append(
        combotext.GetComponent<RectTransform>().DOAnchorPosY(GamePlay.current_lvl_mat.Combomsgpos + comboanchorpos.y, 0.15f).SetEase(easeType).OnComplete(() =>
        {
            combotext.SetActive(false);
            combotext.GetComponent<RectTransform>().anchoredPosition = comboanchorpos;
        }));
    } 
    Sequence mySequence2;
    public void Animateperfectshoot()
    {
        perfecttext.SetActive(false);
        if (mySequence2 == null)
            perfecttext.transform.localScale = Vector3.zero;
        else
            mySequence2.Kill();
        perfecttext.SetActive(true);
        perfecttext.GetComponentInChildren<TextMeshProUGUI>().text = GamePlay.main.PerfectShootText +
           +GamePlay.main.PerfectMultiple* (GamePlay.CurrentLevel + 1);
        mySequence2 = DOTween.Sequence();
        perfecttext.transform.DOScale(Vector3.one * 1f, 0.2f).SetEase(easeType);
        mySequence2.Append(
        perfecttext.GetComponent<RectTransform>().DOAnchorPosY(GamePlay.current_lvl_mat.Perfectmsgpos + perfectanchorpos.y, 0.5f).SetEase(easeType).OnComplete(() =>
        {
            perfecttext.SetActive(false);
            perfecttext.GetComponent<RectTransform>().anchoredPosition = perfectanchorpos;
        }));
        print("perfect shot");
    }
    public void AnimateFire(Vector3 pos)
    {
        if (fireQueue.Count > 0)
        {
            GameObject Star = fireQueue.Dequeue();
            Star.gameObject.SetActive(true);
            Star.GetComponent<ParticleSystem>().Play();
            Star.transform.position = pos;
            StartCoroutine(court());
            IEnumerator court()
            {
                yield return new WaitForSeconds(0.7f);
                Star.gameObject.SetActive(false);
                fireQueue.Enqueue(Star);
            }
        }
    }
    public GameObject GetBullet()
    {
        if (bulletQueue.Count > 0)
        {
            return bulletQueue.Dequeue();
        }
        else
            return Instantiate(GamePlay.main.balls[0]);
    }
    public void UnQueueBullet(GameObject bullet)
    {
        bullet.gameObject.SetActive(false);
        bulletQueue.Enqueue(bullet);
    }
    public GameObject GetFireBullet()
    {
        if (bulletfireQueue.Count > 0)
        {
            return bulletfireQueue.Dequeue();
        }
        else
            return Instantiate(GamePlay.main.balls[1]);
    }
    public void UnQueueFireBullet(GameObject bullet)
    {
        bullet.gameObject.SetActive(false);
        bulletfireQueue.Enqueue(bullet);
    }
}
