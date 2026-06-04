using System;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    public LevelManager levelManager;
    public Player player;
    public AudioManager audioManager;

    public UIManager uiManager;

    public GameState gameState;

    private void Start()
    {
        uiManager.ShowMainMenu();
        gameState = GameState.MainMenu;
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

    public void LoadNextLevel()
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
        gameState = GameState.GamePlay;
        levelManager.RestartLevelManager();//Level manager sýfýrlansýn
        player.RestartPlayer();//player sýfýrlansýn
        audioManager.PlayAmbiantSound();
    }

    public void PlayerDied()
    {
        levelManager.StopLevel();
        LevelFailed();
    }

    public void LevelCompleted()
    {
        gameState = GameState.WinUI;
        audioManager.PlayVictoryAS();
        audioManager.StopAmbiantSound();
        uiManager.ShowWinUI(3);
    }
        
    void LevelFailed()
    {
        gameState = GameState.LoseUI;
        uiManager.ShowFailUI(3);
        audioManager.PlayFailAS();
        audioManager.StopAmbiantSound();
    }
}

public enum GameState
{
    MainMenu,
    GamePlay,
    WinUI,
    LoseUI,
}
