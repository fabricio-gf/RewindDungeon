using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSfxManager : ActorSfxManager {

	public override void PlayDeathSound() {
		SoundManager.SM.DeathFemaleSound();
	}

	public override void PlayAttackSound() {
		SoundManager.SM.BowSound();
	}

}
