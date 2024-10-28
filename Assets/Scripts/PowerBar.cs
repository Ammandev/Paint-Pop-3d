using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerBar : MonoBehaviour
{
    public TextMeshProUGUI powercounttext;
    public Image[] powerfill;
    private float powerfilltimer = 0.0f;
    [Tooltip("stay filled power duration after it turn red per power")]
    public float FilledStayDurationPerPower = 0.05f;
    [Tooltip("speed in which power start unloading while not filled")]
    public float UnloadingSpeed = 0.2f;
    [Tooltip("Maximum plate painted needed to fill power")]
    public int FillMaxPoint = 5;
    //public GameObject Usebutton;
    private int _power = 0;
    [Tooltip("check is power filled")]
    public bool PowerFilled;
    public bool starttimer;
    public bool Transaction;
    public float LerpSpeed = 0.5f;
	IEnumerator co;
    public int PowerPoints;
    public int Power { get { return _power; }set { if (_power != value) UpdatePower(value); } }
    public static PowerBar bar;
    private void Awake()
    {
        bar = this;
    }
    private void Start()
    {
        //   Usebutton.SetActive(false);
        // warning.SetActive(false);
        if (GamePlay.ReturnFromGameWin)
        {
            Power = PlayerPrefs.GetInt("Power", 0);
            print(Power);
        }
        PlayerPrefs.SetInt("Power", 0);
        UpdatePower(Power);
    }
    private void Update()
    {
        Image currentimage = GetFillImg();
        if (starttimer && !Transaction)//some wait before reseting power to zero
        {
            powerfilltimer += Time.deltaTime;
            if (powerfilltimer >= FilledStayDurationPerPower)
            {
                powerfilltimer = 0.0f;
                if (PowerPoints > 0)
                {
                    Power--;
                    PowerPoints--;
                }
                if (PowerPoints <= 0 || !PowerFilled)
                {
                    // Usebutton.SetActive(false);
                    PowerFilled = false;
                    GamePlay.CurrentCombo = 1;
                    // Power = 0;
        in_coroutine = true;
                    starttimer = false;
                    SplashScript.main.ResetLight();
                    print("power finished");
                }
            }
        }
        currentimage.fillAmount = Mathf.Lerp(currentimage.fillAmount, currentfillamount, LerpSpeed);
    }
    float currentfillamount = 0.0f;
    public void UpdatePower(int v)
    {
        _power = v;
        if (GamePlay.InGame)
        {
            PlayerPrefs.SetInt("Power", _power);
        }
        float _va = (float)(v%FillMaxPoint) / (float)FillMaxPoint;
    //    warning.SetActive(_va < powerfill[0].fillAmount && PowerFilled);
        currentfillamount = _va;//animation using dotween with 0.3second duration to move bar value to new value(_va)
    }
    public void StopPower(bool v)
    {
        PowerFilled = v;
    }
    public void UsePower()
    {
            if (PowerFilled) return;
            if (Power < FillMaxPoint) return;
        if (!PowerFilled)
        {
            GamePlay.CurrentCombo++;
        }
        PowerPoints = FillMaxPoint+(Power%FillMaxPoint);
        PowerFilled = true;
     //   warning.SetActive(false);
        SplashScript.main.DimLight();
        powerfilltimer = 0.0f;
        //   Usebutton.SetActive(false);
        starttimer = true;
    }
    public bool in_coroutine = false;
    public void StartUnloadingPower()
    {
       // if (Power >= FillMaxPoint) return;
        if (PowerFilled) return;
        if (Power <= 0) return;
        if (Power%FillMaxPoint == 0) return;
        if (in_coroutine) return;
        in_coroutine = true;
        co = unloadingcourt();
		StartCoroutine(co);
        //   starttimer = true;
        //  powerfilltimer = FilledStayDuration;
    } 
    public void StopUnloadingPower()
    {
       // if (Power >= FillMaxPoint) return;
        if (PowerFilled) return;
        if (!in_coroutine) return;
        in_coroutine = false;
        if (co != null)
                StopCoroutine(co);
        co = null;
    }
    public IEnumerator unloadingcourt()
    {
        while (in_coroutine)
        {
            if (Power <= 0 || PowerFilled || Power % FillMaxPoint == 0)
            {
                in_coroutine = false;
                yield return null;
            }
            else if (!Transaction)
                Power--;
            yield return new WaitForSeconds(UnloadingSpeed);
        }
    }
    public void PlayerPower(bool v)
    {
        starttimer = v && PowerFilled;
    }
    public Image GetFillImg()
    {
        if (Power >= FillMaxPoint * 5)
        {
            powerfill[0].fillAmount = 1f;
            powerfill[1].fillAmount = 1f;
            powerfill[2].fillAmount = 1f;
            powerfill[3].fillAmount = 1f;
            powerfill[4].fillAmount = 1f;
        powercounttext.text = 5.ToString();
            return powerfill[5];
        } 
        if (Power >= FillMaxPoint * 4)
        {
            powerfill[0].fillAmount = 1f;
            powerfill[1].fillAmount = 1f;
            powerfill[2].fillAmount = 1f;
            powerfill[3].fillAmount = 1f;
            powerfill[5].fillAmount = 0f;
            powercounttext.text = 4.ToString();
            return powerfill[4];
        }
        if (Power >= FillMaxPoint * 3)
        {
            powerfill[0].fillAmount = 1f;
            powerfill[1].fillAmount = 1f;
            powerfill[2].fillAmount = 1f;
            powerfill[4].fillAmount = 0f;
            powerfill[5].fillAmount = 0f;
            powercounttext.text = 3.ToString();
            return powerfill[3];
        }
        else if (Power >= FillMaxPoint * 2)
        {
            powerfill[0].fillAmount = 1f;
            powerfill[1].fillAmount = 1f;
            powerfill[3].fillAmount = 0f;
            powerfill[4].fillAmount = 0f;
            powerfill[5].fillAmount = 0f;
            powercounttext.text = 2.ToString();
            return powerfill[2];
        }
        else if (Power >= FillMaxPoint)
        {
            powerfill[0].fillAmount = 1f;
            powerfill[2].fillAmount = 0f;
            powerfill[3].fillAmount = 0f;
            powerfill[4].fillAmount = 0f;
            powerfill[5].fillAmount = 0f;
            powercounttext.text =1.ToString();
            return powerfill[1];
        }
        else
        {
            powerfill[1].fillAmount = 0f;
            powerfill[2].fillAmount = 0f;
            powerfill[3].fillAmount = 0f;
            powerfill[4].fillAmount = 0f;
            powerfill[5].fillAmount = 0f;
            powercounttext.text =0.ToString();
            return powerfill[0];
        }
    }
}
