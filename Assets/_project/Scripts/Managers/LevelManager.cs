using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameDirector gameDirector;
    public List<Level> levelPrefabs;
    public int currentLevelNo;
    private Level _currentLevel;
    public void RestartLevelManager()
    {
        DeleteCurrentLevel();//Mevcut level siliniyor.
        CreateNewLevel();//Yeni level oluþturuluyor.
    }

    public void StopLevel()
    {
        _currentLevel.StopLevel();
    }

    private void CreateNewLevel() 
    {
        _currentLevel = Instantiate(levelPrefabs[currentLevelNo - 1]); //Level 1'den baþladýðý için. - levelPrefabs Instantiate
        _currentLevel.transform.position = Vector3.zero;
        _currentLevel.StartLevel(this);

    }

    private void DeleteCurrentLevel()
    {
        if(_currentLevel)
        {
            Destroy(_currentLevel.gameObject);
        } 
    }
}
