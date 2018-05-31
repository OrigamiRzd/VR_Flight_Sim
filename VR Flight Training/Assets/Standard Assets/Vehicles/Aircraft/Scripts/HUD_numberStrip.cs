using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Vehicles.Aeroplane;
using UnityEngine;

public class HUD_numberStrip : MonoBehaviour
{

	public enum NumberType {airspeed, altitude};

	public NumberType typeOfNumber = NumberType.airspeed;
	public float decimalPlace;

	private AeroplaneController flightScript;
	private Vector3 initPos;
	private float movementMultiplier;

	public float test;
	private Vector3 directionVector = new Vector3(Mathf.Cos(20),Mathf.Sin(20),0);

	// Use this for initialization
	void Start ()
	{
		flightScript = GameObject.FindGameObjectWithTag ("AircraftJet").GetComponent <AeroplaneController> ();
		initPos = transform.localPosition;
		movementMultiplier = -0.003333333f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (typeOfNumber == NumberType.airspeed) {
			transform.localPosition = initPos + new Vector3 (0, ((flightScript.ForwardSpeed / decimalPlace) % 10) * movementMultiplier, 0);
		} else if (typeOfNumber == NumberType.altitude) {
			transform.localPosition = initPos + new Vector3 (0, ((flightScript.Altitude / decimalPlace) % 10) * movementMultiplier, 0);
		}
	}
}
