using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level", order = 1)]
public class Level : ScriptableObject {

	[System.Serializable]
	public class EnemyInstance {
		public EnemyType enemyType;
		public Position position;
		public List<Actor.Action> plan;
	}

	public enum PlayerClass {
		ARCHER,
		THIEF,
		WARRIOR
	}

	public enum EnemyType {
		TEST_ENEMY,
		SKELETON
	}

	public string title;
    public int index;
    public string spritePath;
	public int timeLimit;
	public List<PlayerClass> classes;
	public List<Position> walls;
    public Position button;
    public Position door;
	public List<Position> spawnPoints;
    public List<Position> coins;
    public Position exit;
	public List<EnemyInstance> enemies;
    public TextAsset dialogue;

}
