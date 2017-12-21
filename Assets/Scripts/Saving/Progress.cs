using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Progress{

    //since there is no bool in json, levels unlocked is of type int, where 0 = false and 1 = true
    public int[] LevelsUnlocked;
    //levels score can be 1, 2 or 3, indicating how many coins has the player collected in that level
    public int[] LevelsScore;
}
