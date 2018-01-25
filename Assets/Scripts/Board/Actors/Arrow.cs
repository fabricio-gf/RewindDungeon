using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    public Actor archerParent;
    public float speed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Button"))
        {
            //faz alguma coisa
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        archerParent.SetReady();
    }
}
