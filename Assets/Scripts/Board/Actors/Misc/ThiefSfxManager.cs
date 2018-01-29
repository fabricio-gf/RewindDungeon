using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefSfxManager : ActorSfxManager {

	public override void PlayDeathSound() {
		SoundManager.SM.DeathMaleSound();
	}
	
}
