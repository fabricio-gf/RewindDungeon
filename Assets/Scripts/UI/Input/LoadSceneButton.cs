using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour {

    public GameData DataManager;
    public ScoreManager Score;
    public GameLoader Loader;

    public void NextLevel()
    {
        DataManager.SetScore(Loader.SelectedLevel.index, Score.CurrentScore);
        Loader.SelectedLevel = FindNextLevel(Loader.SelectedLevel);
        DataManager.UnlockLevel(Loader.SelectedLevel.index);
        if (Loader.SelectedLevel != null)
            StartCoroutine(WaitForLoad());
        //else
        //last level
    }

    IEnumerator WaitForLoad()
    {
        yield return null;
        SceneManager.LoadScene("Cutscene");
    }

    public Level FindNextLevel(Level level)
    {
        int index = level.index + 1;
        Level nextLevel = null;
        if (index < 20)
            nextLevel = Resources.Load("Levels/T" + index) as Level;

        return nextLevel;
    }

    public void RestartLevel()
    {
        DataManager.SetScore(Loader.SelectedLevel.index, Score.CurrentScore);
        Level aux = FindNextLevel(Loader.SelectedLevel);
        DataManager.UnlockLevel(aux.index);
        ResetRoom();
    }

	public void ResetRoom() {
		GameManager.GM.ResetRoom();
	}
}
