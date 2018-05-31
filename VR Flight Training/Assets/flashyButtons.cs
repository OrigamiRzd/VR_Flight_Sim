using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashyButtons : MonoBehaviour {

    public GameObject b1;

    Material m1;

	// Use this for initialization
	void Start () {
        m1 = b1.GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Z)) {
            m1.color = Color.red;
        }

	}
}
