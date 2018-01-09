using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTest : MonoBehaviour {

    Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            anim.SetBool("Run", !anim.GetBool("Run"));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            anim.SetTrigger("Die");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            anim.SetTrigger("Die2");
        }
    }
}
