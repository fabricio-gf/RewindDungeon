using UnityEngine;
using System.Collections;
 
public class Billboard : MonoBehaviour {
    public Camera cam;

    void Start() {
    	cam = Camera.main;
    }
 
    void Update() {
        transform.LookAt(
        	transform.position + cam.transform.rotation * Vector3.forward,
            cam.transform.rotation * Vector3.up);
    }
}