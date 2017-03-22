﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class GameController : MonoBehaviour {
    public GameObject currentLevel;
    private GameData data;
    private int numberOfLevelsInGame = 3;

	void Awake()
    {
        Load();
    }

    public void Save()
    {
		BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gameProgress.dat");

        LevelController currentLevelController = currentLevel.GetComponent<LevelController>();
		LevelData level = new LevelData();

		if (currentLevelController == null) 
		{
			level.ID = 0;
			level.hasBeenCompleted = false;
			level.hasMapPiece = false;
			level.mapPieceWasFound = false;
		} 
		else 
		{
			level.ID = currentLevelController.getID();
			level.hasBeenCompleted = currentLevelController.getHasBeenCompleted();
			level.hasMapPiece = currentLevelController.gethasMapPiece();
			level.mapPieceWasFound = currentLevelController.getMapPieceFound();
		}

        data.levels[level.ID] = level;

        currentLevelController.print();

        bf.Serialize(file, data);
        file.Close();
    }

	public int getCurrentLevelIndex()
	{
		if (currentLevel == null) 
		{
			Debug.Log ("SOMETHING THAT SHOULDN'T HAVE HAPPENED HAPPENED - CHECK GAME MANAGER");
			return -1;
		} 
		else 
		{
			return currentLevel.GetComponent<LevelController> ().getID ();
		}
	}

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/gameProgress.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameProgress.dat", FileMode.Open);
            data = (GameData)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            initializeGameData();
        }
    }

    private void initializeGameData()
    {
        data = new GameData();
        data.levels = new List<LevelData>();
        
        for (int i = 0; i < numberOfLevelsInGame; i++)
        {
            LevelData level = new LevelData();
            level.init(i);
            data.levels.Add(level);
        }

        Save();
    }
}

[Serializable]
class GameData
{
    public List<LevelData> levels;
} 

[Serializable]
class LevelData
{
    public int ID;
    public bool hasBeenCompleted;
    public bool hasMapPiece;
    public bool mapPieceWasFound;

    public void init(int ID)
    {
        this.ID = ID;
        hasBeenCompleted = false;
        mapPieceWasFound = false;
    }
}
