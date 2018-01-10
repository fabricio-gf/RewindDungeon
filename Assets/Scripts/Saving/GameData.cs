using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Saves and loads data for levels unlocked and score of each level
/// </summary>
public class GameData : MonoBehaviour {

    public string saveFileName;
    private string saveFilePath;

    //data class to use in JSON methods
    Progress data;
    //arrays to store and change information about the player's progress
    public int[] LevelsUnlocked;
    public int[] LevelsScore;

    //stream writer for editing the text file
    private StreamWriter writer;

    //object that has the levels buttons as children
    public Transform LevelsPanel;
    Transform[] LevelsButtons;

    private void Awake()
    {
        data = new Progress();

        LevelsButtons = new Transform[LevelsPanel.childCount];
        for (int i = 0; i < LevelsPanel.childCount; i++)
        {
            LevelsButtons[i] = LevelsPanel.GetChild(i);
        }

        saveFilePath =
            Application.persistentDataPath
            + "/" + saveFileName + ".json";
    }

    private void Start()
    {
        //loads the progress at the start of the scene
        LoadFromJSON();
        print("loaded");
    }

    private void Update()
    {
        //TESTING METHOD
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("saving");
            SaveAsJSON();
            print("saved");
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            print("loading");
            LoadFromJSON();
            print("loaded");
        }
    }

    /// <summary>
    /// Saves any changes to the player's progress into the text file, using json
    /// </summary>
    public void SaveAsJSON()
    {
        data.LevelsUnlocked = this.LevelsUnlocked;
        data.LevelsScore = this.LevelsScore;

        // writer = File.CreateText(AssetDatabase.GetAssetPath(file));
        writer = File.CreateText(saveFilePath);
        writer.WriteLine(JsonUtility.ToJson(data));
        writer.Flush();
        writer.Close();

        // AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(file));
    }

    /// <summary>
    /// Reads from the save text file using json and loads the information into the scene
    /// </summary>
    public void LoadFromJSON()
    {
        if (!File.Exists(saveFilePath)) {
            SaveAsJSON();
        }
        string fileText = System.IO.File.ReadAllText(saveFilePath);
        data = JsonUtility.FromJson<Progress>(fileText);

        LevelsUnlocked = new int[data.LevelsUnlocked.Length];
        LevelsScore = new int[data.LevelsScore.Length];

        Array.Copy(data.LevelsUnlocked, this.LevelsUnlocked, LevelsUnlocked.Length);
        Array.Copy(data.LevelsScore, this.LevelsScore, LevelsScore.Length);

        for(int i = 0; i < LevelsUnlocked.Length; i++)
        {
            //this comparison exists because for serialization, LevelsUnlocked must be an array of int instead of bool
            LevelsButtons[i].GetComponent<LevelButton>().IsUnlocked = LevelsUnlocked[i] == 1 ? true : false;
            LevelsButtons[i].GetComponent<LevelButton>().Score = LevelsScore[i];
            LevelsButtons[i].GetComponent<LevelButton>().UpdateUI();
        }
    }

    /// <summary>
    /// Clears all the player's progress and saves to the text file
    /// </summary>
    public void ClearProgress()
    {
        for(int i = 0; i < LevelsUnlocked.Length; i++)
        {
            LevelsUnlocked[i] = 0;
            LevelsScore[i] = 0;
        }
        LevelsUnlocked[0] = 1;
        SaveAsJSON();
    }

    public void UnlockLevel(int index)
    {
        LevelsUnlocked[index] = 1;
        SaveAsJSON();
    }

    public void SetScore(int index, int score)
    {
        if(score < 0 || score > 3)
        {
            Debug.Log("Invalid Score");
            return;
        }
        if (score > LevelsScore[index])
        {
            LevelsScore[index] = score;
            SaveAsJSON();
        }
    }
}
