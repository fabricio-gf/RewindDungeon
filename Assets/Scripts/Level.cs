using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level {
	[System.Serializable]
	public class PosList {
		public List<Pos> pos;

		public PosList() {
			pos = new List<Pos>();
		}
	}

	[System.Serializable]
	public class ActorList {
		public List<Actor> actors;

		public ActorList() {
			actors = new List<Actor>();
		}	
	}

	public int timeLimit;

	[SerializeField]
	public PosList wallList;

	[SerializeField]
	public PosList collectibleList;

	[SerializeField]
	public ActorList playerCharacterList;

	[SerializeField]
	public ActorList enemyList;

	public List<Pos> walls {
		get {
			return wallList.pos;
		}
	}

	public List<Pos> collectibles {
		get {
			return collectibleList.pos;
		}
	}

	public List<Actor> enemies {
		get {
			return enemyList.actors;
		}
	}

	public List<Actor> playerCharacters {
		get {
			return playerCharacterList.actors;
		}
	}

	public Level() {
		wallList = new PosList();
		collectibleList = new PosList();
		playerCharacterList = new ActorList();
		enemyList = new ActorList();
	}

}