using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Level", menuName = "Level", order = 1)]
public class Level : ScriptableObject {

	public enum PlayerClass {
		ARCHER,
		THIEF,
		WARRIOR
	}

	public string title;
	public int timeLimit;
	public List<PlayerClass> classes;
	public List<Position> walls;
	public List<Position> spawnPoints;

}
