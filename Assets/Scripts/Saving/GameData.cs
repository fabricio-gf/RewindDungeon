using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Saves and loads data for levels unlocked and score of each level
/// </summary>
public class GameData : MonoBehaviour {

    //the player progress text file
    public TextAsset file;

    //data class to use in JSON methods
    Progress data;
    //arrays to store and change information about the player's progress
    public int[] LevelsUnlocked;
    public int[] LevelsScore;

    //stream writer for editing the text file
    private StreamWriter writer;

    private void Awake()
    {
        data = new Progress();
        //loads the progress at the start of the scene
        LoadFromJSON();
    }

    /// <summary>
    /// Saves any changes to the player's progress into the text file, using json
    /// </summary>
    public void SaveAsJSON()
    {
        data.LevelsUnlocked = this.LevelsUnlocked;
        data.LevelsScore = this.LevelsScore;

        writer = File.CreateText(AssetDatabase.GetAssetPath(file));
        writer.WriteLine(JsonUtility.ToJson(data));
        writer.Flush();
        writer.Close();

        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(file));
    }

    /// <summary>
    /// Reads from the save text file using json and loads the information into the scene
    /// </summary>
    public void LoadFromJSON()
    {

        data = JsonUtility.FromJson<Progress>(file.ToString());

        LevelsUnlocked = new int[data.LevelsUnlocked.Length];
        LevelsScore = new int[data.LevelsScore.Length];

        Array.Copy(data.LevelsUnlocked, this.LevelsUnlocked, LevelsUnlocked.Length);
        Array.Copy(data.LevelsScore, this.LevelsScore, LevelsScore.Length);

        //loads info into the levels scene
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
}
