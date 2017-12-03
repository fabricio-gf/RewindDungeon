using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level {
	public int timeLimit;
	public List<Pair<int, int>> walls;
	public List<Pair<int, int>> collectibles;
	public List<Pair<int, int>> spawnPos;
	public List<PlayerCharacter> characters;
	public List<Actor> enemies;
}