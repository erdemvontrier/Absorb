using System;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    public LevelManager levelManager;
    public Player player;

    private void Start()
    {
        RestartLevel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            RestartLevel();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            LoadNextLevel();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            LoadPreviousLevel();
        }
    }

    private void LoadPreviousLevel()
    {
        levelManager.currentLevelNo--;
        //Eðer current level 1'e eþit veya 1den küçük yapýlmaya çalýþýlýrsa current level 1'e eþitlensin
        if(levelManager.currentLevelNo <= 1)
        {
            levelManager.currentLevelNo = 1;
        }
        RestartLevel();
    }

    private void LoadNextLevel()
    {
        levelManager.currentLevelNo++;
        //Eðer current level max ise veya dahada büyütülmeye çalýþýlýyorsa max level'da kalsýn
        if (levelManager.currentLevelNo >= levelManager.levelPrefabs.Count)
        {
            levelManager.currentLevelNo = levelManager.levelPrefabs.Count;
        }
        RestartLevel();
    }

    public void RestartLevel()
    {
        levelManager.RestartLevelManager();//Level manager sýfýrlansýn
        player.RestartPlayer();//player sýfýrlansýn
    }

    public void PlayerDied()
    {
        levelManager.StopLevel();
        LevelFailed();
    }

    public void LevelCompleted()
    {
        print("Level Completed");
        Invoke(nameof(LoadNextLevel),2);
    }

    void LevelFailed()
    {
        print("Level Failed");
        Invoke(nameof(RestartLevel),4);
    }
}
