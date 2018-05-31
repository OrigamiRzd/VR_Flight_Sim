using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Was used for detecting landings. Functionality has moved to AircraftFlightDynamics temporarily
/// </summary>

public class LandingDetection : MonoBehaviour
{
	//	public GameObject ground;
	public bool landedOrNo = false;
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	void OnCollisionEnter (Collision c)
	{
		Debug.Log ("Landed collision");
//		if (c.gameObject.tag == "ground") {
		landedOrNo = true;
//		}
	}
}
