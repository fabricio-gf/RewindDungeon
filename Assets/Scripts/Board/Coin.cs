using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    public ScoreManager Score;

    public float flipTime;
    public float spinTime;
    public float animDelay;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Score.AddCoin();
            Destroy(gameObject);
        }

    }

    void Start() {
        Score = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        StartSpin();
    }

    void StartSpin() {
        iTween.RotateBy(
            gameObject,
            iTween.Hash(
                "z", 0.5f,
                "time", spinTime,
                "delay", animDelay,
                "loop", "none",
                "easetype", "easeInOutCubic",
                "oncomplete", "StartFlip"));
    }

    void StartFlip() {
        iTween.RotateBy(
            gameObject,
            iTween.Hash(
                "y", 0.5f,
                "time", flipTime,
                "looptype", "none",
                "easetype", "easeInOutBack",
                "oncomplete", "StartSpin"));
    }
}
