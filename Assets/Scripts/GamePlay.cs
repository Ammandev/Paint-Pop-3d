using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class GamePlay : MonoBehaviour
{
    [Tooltip("ball models which gonna spawn from gun index same as mat")]
    public GameObject[] balls;
   // [Tooltip("Maximum plate painted needed to fill power")]
  //  public float gun_move_speed = 0.5f;
    [Tooltip("Effect force to explode circle plates after finished")]
    public float exp_force = 100f;
    [Tooltip("Effect duration to explode circle plates after finished")]
    public float enemy_expode_duraion = 1f;
    public float effect_duration = 2f;
    ///Gui animation
    public Ease easeIn, easeOut;
    public float moveTime = 0.25f, delayTime = 0.2f;
    ///end
    public TextMeshProUGUI Level_count, current_lvltxt, next_lvltxt;
    public TextMeshProUGUI bonusleveltimer;
    public TextMeshProUGUI coin_count;
    public TextMeshProUGUI score_count;
    public TextMeshProUGUI BestScore_count;
    public TextMeshProUGUI combotext, perfecttext;
    public TextMeshProUGUI leveltext1, leveltext2;
    private int _score = 0,targetscore,targetcoin, startscore = 0, _coin = 0;
    [Tooltip("Current Score which is updating on hit")]
    public int NewScore { get { return targetscore; } set { if (targetscore != value) UpdateScore(value); } }
    public int Coin { get { return targetcoin; } set { if (targetcoin != value) UpdateCoin(value); } }
    public bool IsPerfectShoot;
    public GameObject Confeti;
    public GameObject MainMenu;
    public GameObject startbut;
    public GameObject scorebut;
    public GameObject GameOverUI;
    public GameObject powerui;
    public GameObject win, lose, bonuslose;
    public GameObject winui;
    public GameObject continuebut, retrybut, levelbar, bonuslevelbar;
    public Image lvlfill, bonuslvlfill, fillbg;
    [HideInInspector]
    public int currentcircle, currentpowerx;
    public CameraFollow follow;
    public LevelRepeatScript levelRepeat;
    public static int CurrentLevel, BestScore,CurrentCombo=1;
    public static bool GameStart,InGame;
    [Tooltip("u can access directly to current level by using (GamePlay.current_lvl)")]
    public static PalletData current_lvl;
    public static PalletMatData current_lvl_mat;
    [Tooltip("u can access directly to current circle by using (GamePlay.current_circle)")]
    public static Circle current_circle;
    public static CubeManagment current_cube;
    public static GamePlay main;
    [Tooltip("text which will appear on combox")]
    [Multiline]
    public string ComboText = "Combo x";
    [Tooltip("texts which will appears on perfect shot(random will show)")]
    [Multiline]
    public string PerfectShootText;
    [Tooltip("score multiple x(current lvl)")]
    public int PerfectMultiple = 3;
    [HideInInspector]
    public PigManagment pigbonus;
    public static bool IsBonusLevel=false;
    public static bool ReturnFromGameWin=false;
    public bool IsPressed;
    public CountEffectSimpleScore countEffect;
    public CountEffectCoinScore countEffect2;
    private bool inCubeshoot = false;
    private void Awake()
    {
        main = this;
        BestScore = PlayerPrefs.GetInt("BestScore", 0);
        //LoadPrefs();
        levelRepeat.CurrentSet();
        current_lvl = levelRepeat.currentLevel.data;
        IsBonusLevel = levelRepeat.isBonusLevel|| levelRepeat.isBossLevel;
        current_lvl_mat = PalletMat.main.pallet_mat[levelRepeat.materialIndex % PalletMat.main.pallet_mat.Length];///////////////////////here u can set random or sequence based pallet selection
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 300;
    }
    public void ResetGame()//for editor use only
    {
        NewScore = 0;
        SceneManager.LoadScene(0);
    }
    void Start()
    {
        Confeti.gameObject.SetActive(false);
        levelbar.SetActive(false);
        bonuslevelbar.SetActive(false);
        continuebut.SetActive(false);
        retrybut.SetActive(false);
        startbut.gameObject.SetActive(true);
        scorebut.gameObject.SetActive(false);
        powerui.SetActive(false);
        moveInImg(MainMenu);
        moveOutImg(GameOverUI);
        Level_count.text = "Level: " + (CurrentLevel+1).ToString();
        leveltext2.text= leveltext1.text = "LEVEL " + (CurrentLevel+1).ToString();
        if (IsBonusLevel)
        {
            Level_count.text = leveltext2.text = leveltext1.text = "Bonus Level";
            pigbonus = current_lvl.main.GetComponent<PigManagment>();
        }
        current_lvltxt.text = (CurrentLevel+1).ToString();
        next_lvltxt.text = (CurrentLevel+2).ToString();
        BestScore_count.text = "Best Score: "+ BestScore.ToString();
        CurrentCombo = 1;
        current_lvl.main.SetActive(true);
        RenderSettings.skybox = current_lvl_mat.skybox;
        //setup color
        current_circle = current_lvl.items[0].circle;
        current_cube = current_lvl.items[0].ShootableCube;
        foreach (var item in current_lvl.items)
        {
            item.circle.DoStartAnim();
        }
        for (int i = 0; i < current_lvl.items.Length; i++)
        {
            foreach (var item in current_lvl.items[i].circle.plates)
            {
                foreach (var item2 in item.obj.GetComponentsInChildren<MeshRenderer>())
                {
                    item2.sharedMaterial = current_lvl_mat.circle_mats[i];
                }
            }
        }
        for (int i = 0; i < current_lvl.items.Length; i++)
        {
            foreach (var item2 in current_lvl.items[i].enemy.GetComponentsInChildren<MeshRenderer>())
            {
                item2.sharedMaterial = current_lvl_mat.enemies_mats[i];
            }
        }
        current_lvl.player.meshRenderer.sharedMaterial = current_lvl_mat.player_mat;
        foreach (var item in current_lvl.items)
        {
            item.cylinder.GetComponent<MeshRenderer>().sharedMaterial = current_lvl_mat.cylinder_mat;
            if (IsBonusLevel) break;
            item.cube1.GetComponent<MeshRenderer>().sharedMaterial = current_lvl_mat.Cube1;
            item.cube2.GetComponent<MeshRenderer>().sharedMaterial = current_lvl_mat.Cube2;
            item.cube3.GetComponent<MeshRenderer>().sharedMaterial = current_lvl_mat.Cube3;
        }
        fillbg.color = current_lvl_mat.ProgresBarFirstColor_mat;
        lvlfill.color = current_lvl_mat.ProgresBarFillColor_mat;
        combotext.color = current_lvl_mat.Combomsg_mat;
        perfecttext.color = current_lvl_mat.Perfect_mat;
        foreach (var item in VFXManager.main.explodepart.GetComponentsInChildren<ParticleSystem>())
        {
            var _main = item.main;
            _main.startColor = current_lvl_mat.Explosion_Circle_color;
        }
        VFXManager.main.PrepareStars();
        //
        follow.Target = current_lvl.player.transform;
        GameStart =InGame = false;
        NewScore = PlayerPrefs.GetInt("LastScore",0);
        startscore = NewScore;
        UpdateScore(NewScore);
        lvlfill.fillAmount = 0;
        if(IsBonusLevel)
        timer = pigbonus.TimeToGameOver;
        _coin= Coin = PlayerPrefs.GetInt("Coin", 0);
    }
    float timer = 60f;
    private void Update()
    {
        if (IsBonusLevel&& InGame)
        {
            if (timer <= 0.0f)
            {
                GameWin(true);
                bonusleveltimer.text = "00:00";
                return;
            }
            timer -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(timer / 60F);
            int seconds = Mathf.FloorToInt(timer - minutes * 60);

            bonusleveltimer.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        }
            _score = Mathf.RoundToInt(Mathf.Lerp(_score, targetscore, 5f));
            score_count.text = _score.ToString();
            _coin = Mathf.RoundToInt(Mathf.Lerp(_coin, targetcoin, 5f));
        coin_count.text = _coin.ToString();
    }
    public void PlayGame()
    {
        if (GameStart) return;
        GameStart = true;
        startbut.gameObject.SetActive(false);
        scorebut.gameObject.SetActive(true);
        powerui.SetActive(!IsBonusLevel);
        levelbar.SetActive(!IsBonusLevel);
        bonuslevelbar.SetActive(IsBonusLevel);
        InGame = true;
    }
    public void UpdateScore(int v)
    {
        targetscore = v;
        if (!DOTween.IsTweening(score_count.transform))
        {
            score_count.transform.DOScale(countEffect.VolumeIncrease, countEffect.SpeedCount / 2)
                .OnComplete(() => score_count.transform.DOScale(1.0f, countEffect.SpeedCount / 2));
        }
        UpdateLvlProg();
    }
    public void UpdateCoin(int v)
    {
        targetcoin = v;
        if (!DOTween.IsTweening(coin_count.transform))
        {
            coin_count.transform.DOScale(countEffect2.VolumeIncrease, countEffect2.SpeedCount / 2)
                .OnComplete(() => coin_count.transform.DOScale(1.0f, countEffect2.SpeedCount / 2));
        }
        UpdateLvlProg();
    }
    public void UpdateLvlProg()
    {
        int cur_score = NewScore - startscore;
        int totalcircle = 0;
        foreach (var item in current_lvl.items)
        {
            totalcircle += item.circle.plates.Count;
        }
        if (IsBonusLevel)
        {
            totalcircle = current_lvl.items[0].circle.maxhitcount;
            cur_score = current_lvl.items[0].circle.hitcount;
            float _va = (float)cur_score / (float)(totalcircle);
            bonuslvlfill.DOFillAmount(_va, 0.3f);//animation using dotween with 0.3second duration to move bar value to new value
        }
        else
        {
            float _va = (float)cur_score / (float)(totalcircle * (CurrentLevel + 1));
            lvlfill.DOFillAmount(_va, 0.3f);//animation using dotween with 0.3second duration to move bar value to new value
        }
    }
    public bool CanShoot()
    {
        if (inCubeshoot)
            return !current_cube.inAnim && !current_cube.AllPainted;
        else 
            return !current_circle.inAnim && !current_circle.AllPainted;
    }
    bool previouspower=false;
    public void ActivateCube(Circle prev_circle)
    {
        prev_circle.gameObject.SetActive(false);
        current_lvl.items[currentcircle].ShootableCube.EnableIt();
        previouspower = PowerBar.bar.PowerFilled;
        inCubeshoot = true;
     //   PowerBar.bar.StopPower(false);
    }
    //will update bar fill amount on power value change
    //this function will be call as after circle filled
    public void TileFilled()
    {
        inCubeshoot = false;
        currentcircle++;
        for (int i = 0; i < current_lvl.items.Length; i++)
        {
            current_lvl.items[i].enemy.gameObject.SetActive(currentcircle <= i);
        }
        Circle circle = null;
        if (currentcircle <= current_lvl.items.Length-1)
            circle = current_lvl.items[currentcircle].circle;
        if (circle == null)
        {
            if (!IsBonusLevel)
            {
                VFXManager.main.chestanim.SetActive(true);
                StartCoroutine(court3());
                IEnumerator court3()
                {
                    yield return new WaitForSeconds(2f);
                    for (int i = 0; i < 10; i++)
                    {
                        VFXManager.main.AnimateCoin(true);
                        yield return new WaitForSeconds(0.1f);
                    }
                    GamePlay.main.Coin += current_lvl.chestm.TotalCoins;
                    GameWin();
                }
            }else 
                    GameWin();
        }
        else
        {
            if (!IsBonusLevel)
            {
                VFXManager.main.chestanim.SetActive(true);
                StartCoroutine(court3());
                IEnumerator court3()
                {
                    yield return new WaitForSeconds(2f);
                    for (int i = 0; i < 10; i++)
                    {
                        VFXManager.main.AnimateCoin(true);
                        yield return new WaitForSeconds(0.1f);
                    }
                    GamePlay.main.Coin += current_lvl.chestm.TotalCoins;
                    yield return new WaitForSeconds(0.5f);
                    VFXManager.main.chestanim.SetActive(false);
                    if (PowerBar.bar.PowerFilled)
                    {
                        CurrentCombo++;
                    }
                    current_circle = circle;
                    current_cube = current_lvl.items[currentcircle].ShootableCube;
                    current_lvl.player.MoveTo();
                    //  if (!PowerBar.bar.PowerFilled)
                    //  PowerBar.bar.Power = 0;
                    UpdateLvlProg();
                }
            }

        }
    }
    //this function will be call as all circle tiles filled
    public void GameWin(bool v=false)
    {
        winui.SetActive(!IsBonusLevel);
        moveInImg(GameOverUI);
        SoundsScript.main.PlayAudioEffect(4);
            if (PlayerPrefs.GetInt("RealLevel", 0) == CurrentLevel)
            {
                CurrentLevel++;
            }
            else
            {
                CurrentLevel = PlayerPrefs.GetInt("RealLevel", 0);
            }
            SavePrefs();
            if (NewScore > BestScore)
                BestScore = NewScore;
            PlayerPrefs.SetInt("LastScore", NewScore);
        if(IsBonusLevel)
        {
            IsBonusLevel = false;
        }
        PlayerPrefs.SetInt("Coin", Coin);
        InGame = false;
        win.SetActive(true);
        powerui.SetActive(false);
        lose.SetActive(false);
        Confeti.SetActive(true);
        if (v)
        {
                ReturnFromGameWin = false;
            IsBonusLevel = false;
            win.SetActive(false);
            lose.SetActive(false);
            bonuslose.SetActive(true);
            StartCoroutine(court3());
            IEnumerator court3()
            {
                yield return new WaitForSeconds(2f);
                SceneManager.LoadScene(1);

            }
        }
        else
        StartCoroutine(court());
        IEnumerator court()
        {
                ReturnFromGameWin = true;
            yield return new WaitForSeconds(2f);
            continuebut.SetActive(true);
        }
    }
    public void Continue()
    {
        if(IsBonusLevel)
            SceneManager.LoadScene(0);
        else
            SceneManager.LoadScene(1);
    } 
    public void Restart()
    {
            SceneManager.LoadScene(0);
    }
    //this function will be call just after ball collided with obstacles
    public void GameLose()
    {
                ReturnFromGameWin = false;
        moveInImg(GameOverUI);
        VibrationAndShake.main.DoEffect(2);
        SoundsScript.main.PlayAudioEffect(3);
        InGame = false;
        win.SetActive(false);
        lose.SetActive(true);
        powerui.SetActive(false);
        PlayerPrefs.SetInt("LastScore", 0);
        StartCoroutine(court());
        IEnumerator court()
        {
            yield return new WaitForSeconds(2f);
            retrybut.SetActive(true);
        }
    }
    public void OnPress(bool v)
    {
        IsPressed = v;

    }
    ///UI Animation Using dotween

    public void moveInImg(GameObject rect)
    {
        rect.GetComponent<CanvasGroup>().alpha = 0f;
        MoveUI(rect.GetComponent<RectTransform>(), easeIn, true);
    }
    public void moveOutImg(GameObject rect)
    {
        MoveUI(rect.GetComponent<RectTransform>(), easeOut, false);
    }
    public void MoveUI(RectTransform _traansform, Ease ease, bool v)
    {
        _traansform.GetComponent<CanvasGroup>().DOFade(v ? 1 : 0, moveTime).SetDelay(delayTime).SetEase(ease).SetUpdate(true).OnComplete(() => _traansform.gameObject.SetActive(v));
    }
    ///Save and load value to use them later for again play 
    public void LoadPrefs()
    {
        CurrentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
        BestScore = PlayerPrefs.GetInt("BestScore", 0);
    }
    public void SavePrefs()
    {
        PlayerPrefs.SetInt("CurrentLevel", CurrentLevel);
        PlayerPrefs.SetInt("RealLevel", CurrentLevel);
        PlayerPrefs.SetInt("BestScore", BestScore);
    }
}
