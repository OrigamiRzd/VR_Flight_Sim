using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLights : MonoBehaviour {

    public List<GameObject> buttons;

    //List<Material> matButton;
    Material mat;
    Color b1;
	// Use this for initialization
	void Start () {
        //matButton = new List<Material>();
        /*
        foreach (GameObject x in buttons) {
            matButton.Add(x.GetComponent<Renderer>().material);
        }
        */
        mat = buttons[0].GetComponent<Renderer>().material;
        //b1 = mat.color;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Z)){
            //mat = matButton[0];
            //mat.SetColor("_EmissionColor", Color.red);
            mat.color = Color.blue;
            Debug.Log("whats");
        }
        else {
            //mat.color = b1;
        }
        
	}
}
