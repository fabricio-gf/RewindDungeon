using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActorSfxManager : MonoBehaviour {

	public abstract void PlayDeathSound();
	public virtual void PlayAttackSound() {}

}
