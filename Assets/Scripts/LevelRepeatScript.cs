using System.Collections;
using System.Collections.Generic;

using UnityEngine;
[System.Serializable]
public class item
{
    public Circle circle;
    public Enemy enemy;
    public CubeManagment ShootableCube;
    public GameObject cylinder;
    public GameObject cube1;
    public GameObject cube2;
    public GameObject cube3;
}
[System.Serializable]
public class PalletData
{
    //  public Material skybox;
    public item[] items;
    public ChestManagmentLevel chestm;
    //  public Circle[] circles;
    [Tooltip("Same as circle length")]
    //  public Material[] circle_mats;
    //  public Enemy[] enemies;
    //  public Material[] enemies_mats;
    public Gun player;
    //  public Material player_mat;
    //   public GameObject[] cylinder;

    //  public Material cylinder_mat;
    public GameObject main;
}

[System.Serializable]
public class StartingLevels
{
    public PalletDataScript[] levels;  // Drag and drop levels into this array
    public int[] palletmats;           // Material indexes for each level
    public PalletDataScript Boss;      // Boss level
    public PalletDataScript Bonus;     // Bonus level
}

[System.Serializable]
public class LevelSets
{
    public int RepeatStart, RepeatEnd;
    public PalletDataScript[] levels;  // Drag and drop levels into this array
    public int[] palletmats;           // Material indexes for each level
    public PalletDataScript Boss;      // Boss level
    public PalletDataScript Bonus;     // Bonus level
}

public class LevelRepeatScript : MonoBehaviour
{
    public StartingLevels listbeforeRepeat; // Levels before repeating starts
    public LevelSets[] lvls;
    public int bossInterval = 5;   // Boss every 5 levels
    public int bonusInterval = 5;  // Bonus every 5 levels, but staggered after bosses

    // Static variables
    [HideInInspector]
    public int materialIndex;
    [HideInInspector]
    public PalletDataScript currentLevel;
    [HideInInspector]
    public bool isBossLevel, isBonusLevel;
    private void Awake()
    {
        listbeforeRepeat.Bonus.gameObject.SetActive(false);
        listbeforeRepeat.Boss.gameObject.SetActive(false);
        for (int i = 0; i < listbeforeRepeat.levels.Length; i++)
        {
            listbeforeRepeat.levels[i].gameObject.SetActive(false);
        }
    }
    public void CurrentSet()
    {
        int _cur = GamePlay.CurrentLevel; // Current level in the game

        // Reset bonus and boss level flags
        isBossLevel = false;
        isBonusLevel = false;

        // Check if the current level is within the listbeforeRepeat array
        if (_cur < listbeforeRepeat.levels.Length)
        {
            int localLevel = _cur + 1; // Local level in listbeforeRepeat

            // Check for Boss level in StartingLevels
            if (localLevel % bossInterval == 0)
            {
                currentLevel = listbeforeRepeat.Boss;
                isBossLevel = true;
            }
            // Check for Bonus level in StartingLevels
            else if ((localLevel + 1) % bonusInterval == 0|| isBonusLevel)
            {
                currentLevel = listbeforeRepeat.Bonus;
                isBonusLevel = true;
            }
            // Otherwise, assign a regular level from listbeforeRepeat
            else if (listbeforeRepeat.levels.Length > 0)
            {
                int levelIndex = Random.Range(0, listbeforeRepeat.levels.Length);
                materialIndex = listbeforeRepeat.palletmats[levelIndex % listbeforeRepeat.palletmats.Length];

                currentLevel = listbeforeRepeat.levels[_cur];
            }
        }
        else
        {
            // Loop through level sets for repeating logic
            for (int i = 0; i < lvls.Length; i++)
            {
                if (_cur >= lvls[i].RepeatStart && _cur <= lvls[i].RepeatEnd)
                {
                    int localLevel = _cur - lvls[i].RepeatStart + 1;

                    // Check for Boss level in LevelSets
                    if (localLevel % bossInterval == 0)
                    {
                        currentLevel = lvls[i].Boss;
                        isBossLevel = true;
                    }
                    // Check for Bonus level in LevelSets
                    else if ((localLevel + 1) % bonusInterval == 0)
                    {
                        currentLevel = lvls[i].Bonus;
                        isBonusLevel = true;
                    }
                    // Otherwise, assign a regular level from LevelSets
                    else if (lvls[i].levels.Length > 0)
                    {
                        int levelIndex = Random.Range(0, lvls[i].levels.Length);
                materialIndex = lvls[i].palletmats[levelIndex % lvls[i].palletmats.Length];
                        currentLevel = lvls[i].levels[levelIndex];
                    }

                    break;
                }
            }
        }
    }
}
