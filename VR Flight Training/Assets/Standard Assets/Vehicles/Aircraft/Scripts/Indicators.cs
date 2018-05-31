using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Vehicles.Aeroplane;
using UnityEngine;

// changes the status of the indicators such as text and dials

public class Indicators : MonoBehaviour
{





	[Header ("Reference Objects")]
	public GameObject planeBody;
	public GameObject ground;
	public GameObject runway20_centerline;
	public GameObject runway09_centerline;
	public AeroplaneController flightScript;

	[Header ("Indicators")]
	public  TextMesh airSpeedText;
	public  TextMesh resultsText;
    

	public GameObject airspeedNeedle;
	public GameObject altitudeNeedle;
	public GameObject altitudeNeedleShort;
	public GameObject headingWheelLarge;


	private Rigidbody planeRB;

	private bool landed = false;
	private float[] previousVerticalSpeed = { 0, 0 };
	private bool previousVerticalSpeedBufferIndex = false;
	private float finalVerticalSpeed;
	private float finalHorizontalOffset;


	// Use this for initialization
	void Start ()
	{
		planeRB = planeBody.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		//Debug.Log("airSpeedText: "+ planeBody.transform.InverseTransformDirection(planeBody.GetComponent<Rigidbody>().velocity).z*knotConverted);
		//Debug.Log("YVelocity: "+ planeBody.GetComponent<Rigidbody>().velocity.y*feetPerMin);
//		flightScript.Airspeed = (int) (planeBody.transform.InverseTransformDirection (planeRB.velocity).z*knotConverted);
//		flightScript.VerticalSpeed = (int) (planeRB.velocity.y * feetPerMin);
//		//Debug.Log (planeBody.GetComponent<Rigidbody> ().velocity.y);
//		flightScript.Altitude = (int)(planeBody.transform.position.y*meters2Ft)-restingAlt;


		// update the display of the indicators
		airSpeedText.text = ("Airspeed: " + Mathf.Round (flightScript.ForwardSpeed) + " knots\n" +
		"Vertical Speed: " + Mathf.Round (flightScript.VerticalSpeed) + " ft/min\n" +
		"Altitude: " + Mathf.Round (flightScript.Altitude) + " ft\n" +
		"Heading: " + Mathf.Round (flightScript.PitchAngle) + " deg\n");
		
		airspeedNeedle.transform.localEulerAngles = new Vector3 (flightScript.ForwardSpeed * 1f - 90, -90, 90);
		altitudeNeedle.transform.localEulerAngles = new Vector3 (flightScript.Altitude * (360f / 1000) - 90, -90, 90);
		altitudeNeedleShort.transform.localEulerAngles = new Vector3 (flightScript.Altitude * (360f / 10000) - 90, -90, 90);
		headingWheelLarge.transform.localEulerAngles = new Vector3 (flightScript.PitchAngle + 90, -90, 90);


		if (ground.GetComponent<LandingDetection> ().landedOrNo) {
			if (!landed) {
				
				if (previousVerticalSpeedBufferIndex)
					finalVerticalSpeed = previousVerticalSpeed [0];
				else
					finalVerticalSpeed = previousVerticalSpeed [1];


				finalHorizontalOffset = runway09_centerline.transform.InverseTransformPoint (planeRB.transform.position).x * 3.28084f;
				landed = true;
			}
				
			resultsText.text = "Results:\nVertical speed" + Mathf.Round (finalVerticalSpeed) + " ft/min\nDistance from centerline: " + Mathf.Round (finalHorizontalOffset) + " ft";
		} else {
			resultsText.text = "";
		}

		// update previous vertical speed buffer
		// this allows us to save the last vertical speed not effected by the physics collision
		if (previousVerticalSpeedBufferIndex) {
			previousVerticalSpeed [0] = flightScript.VerticalSpeed;
			previousVerticalSpeedBufferIndex = false;
		} else {
			previousVerticalSpeed [1] = flightScript.VerticalSpeed;
			previousVerticalSpeedBufferIndex = true;
		}


		if (!landed) {
			// warn about speed on approach
			speedWarning ();
		}

	}


	void speedWarning ()
	{
        
		if (flightScript.ForwardSpeed < 60) {
			resultsText.text = "SPEED UP! SPEED UP!";
		} else if (flightScript.ForwardSpeed < 100) {
			resultsText.text = "Speed up.";
		} else if (flightScript.ForwardSpeed > 155) {
			resultsText.text = "SLOW DOWN!";
		} else if (flightScript.ForwardSpeed > 180) {
			resultsText.text = "Slow down.";
		} else {
			resultsText.text = "";
		}
	}
}
