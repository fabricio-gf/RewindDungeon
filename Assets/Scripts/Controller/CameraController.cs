using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {

	public float aspectRatioW = 16.0f;
	public float aspectRatioH = 9.0f;

	void Start() {
		Camera camera = GetComponent<Camera>();
		camera.aspect = aspectRatioW / aspectRatioH;
	}

}