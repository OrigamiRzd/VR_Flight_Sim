using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Aeroplane;

public class HUDController : MonoBehaviour {

	public AeroplaneController flightScript;

	// HUD elements
	public Transform horizon_forward;
	//public Transform HSI_round;
	public Transform HSI_tape;

	// original positions of HUD elements
	private Vector3 horizon_forward_orig_pos;
	private Vector3 horizon_forward_orig_rot;
	private Vector3 HSI_round_orig_pos;
	private Vector3 HSI_round_orig_rot;
	private Vector3 HSI_tape_orig_pos;

	// scale of movements
	[Tooltip("How far the attitude indicator moves")]
	public float attitudeScale = .005f;
	public float attitudeOffset = .01f;
	public float headingTapeScale = .00068f;


	// Use this for initialization
	void Start () {
		// set the original positions of the HUD elements
		horizon_forward_orig_pos = horizon_forward.localPosition;
		horizon_forward_orig_rot = horizon_forward.localEulerAngles;
		//HSI_round_orig_pos = HSI_round.localPosition;
		//HSI_round_orig_rot = HSI_round.localEulerAngles;
		HSI_tape_orig_pos = HSI_tape.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		horizon_forward.localPosition = horizon_forward_orig_pos + new Vector3(0,-flightScript.Altitude* attitudeScale+attitudeOffset,0);
		horizon_forward.localEulerAngles = horizon_forward_orig_rot + new Vector3 (0,0,-flightScript.RollAngle);
		//HSI_round.localEulerAngles = HSI_round_orig_rot + new Vector3 (0, 0, flightScript.Heading);
		HSI_tape.localPosition = HSI_tape_orig_pos + new Vector3 (0,0,flightScript.Heading * headingTapeScale);
	}
}
