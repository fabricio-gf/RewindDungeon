using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSfxManager : ActorSfxManager {

	public override void PlayDeathSound() {
		SoundManager.SM.BoneSound();
	}

	public override void PlayAttackSound() {
		SoundManager.SM.BoneSound();
	}

}
