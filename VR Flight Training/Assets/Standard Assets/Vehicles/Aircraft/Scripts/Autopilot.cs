using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

// controls the functionality of the autopilot in the plane

public class Autopilot : NetworkBehaviour
{

	[SyncVar]
	public bool netHdgHold;
	[SyncVar]
	public bool netAltHold;
	[SyncVar]
	public bool netSpdHold;
	[SyncVar]
	public bool netRings;

	// altitude hold keeps the airplane at a certain distance below the targe altitude
	public bool altitudeHold = false;
	public bool headingHold = false;
	public bool airspeedHold = false;
	public float altitude = 0;
	public float heading = 0;
	public float airspeed = 0;

	public float targetAtitude = 2.5f;
	public float targetRoll = 0;

	// landing-specific
	public float runwayHeading = 88.3f;
	public float turningRadius = 4000;

	private GameObject airplane;
	public TextMesh DebugText;
	public MeshRenderer localAuthVis;

	public Material defaultMat;
	public Material redMat;

	public GameObject ringsObject;
	public bool ringsVisible = false;

	[Header ("Start parameters")]
	public float initialVelocity = 150;
	public bool startWithAutopilotEnabled = true;

	// Use this for initialization
	void Start ()
	{
		airplane = transform.root.gameObject;
		if (startWithAutopilotEnabled) {
			altitudeHold = true;
			airspeedHold = true;
			headingHold = true;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (Input.GetButtonDown ("ringsToggle")) {
			toggleRings ();
		}

		headingHold = netHdgHold;
		altitudeHold = netAltHold;
		airspeedHold = netSpdHold;
		ringsVisible = netRings;

		if (!hasAuthority) {
//			DebugText.text = "no autopilot authority";

//			updateAutoPilotColors ();
			localAuthVis.material = defaultMat;
		} else {
//			DebugText.text = "authority!";
//			netHdgHold = headingHold;
//			netAltHold = altitudeHold;
//			netSpdHold = airspeedHold;
			localAuthVis.material = redMat;
		}

	}


	// toggle a certain autopilot button
	// before calling this method, you need to get authority of the autopilot gameobject
	// input strings: airspeedHoldButton, altitudeHoldButton, headingHoldButton

	public void toggleAutoPilotButton (string s)
	{
//		Debug.Log ("toggle autopilot button");
//		if (!isServer) {
//			Debug.Log ("tried to get auth");
//			CmdGiveMeAuthority (this.gameObject);
//		}

		if (!hasAuthority) {
			Debug.Log ("can't do that because autopilot obj doesn't have auth.");
			return;
		} else {
			if (s == "airspeedHoldButton") {
				Debug.Log ("airspeed");
				if (airspeedHold) {
					Debug.Log ("airspeed false");
					airspeedHold = false;
				} else {
					Debug.Log ("airspeed true");
					airspeedHold = true;
				}
			} else if (s == "altitudeHoldButton") {
				Debug.Log ("alt");
				if (altitudeHold) {
					altitudeHold = false;
				} else {
					altitudeHold = true;
				}
			} else if (s == "headingHoldButton") {
				Debug.Log ("hdg");
				if (headingHold) {
					Debug.Log ("hdg false");
					headingHold = false;
				} else {
					Debug.Log ("hdg true");
					headingHold = true;
				}
			}

//		CmdUpdateAutopilotButtons ();

		}

//		if (!isServer) {
//			CmdReturnAuthority (this.gameObject);
//		}
	}

	public void serverToggleAutopilotButton (string s)
	{
		if (!isServer) {
			Debug.Log ("wasn't server");
			return;
		}
		
		if (s == "airspeedHoldButton") {
			if (netSpdHold) {
				netSpdHold = false;
			} else {
				netSpdHold = true;
			}
		} else if (s == "altitudeHoldButton") {
			if (netAltHold) {
				netAltHold = false;
			} else {
				netAltHold = true;
			}
		} else if (s == "headingHoldButton") {
			if (netHdgHold) {
				netHdgHold = false;
			} else {
				netHdgHold = true;
			}
		}
	}

	public void serverToggleHdgButton ()
	{
		if (!isServer) {
			return;
		}
		if (netHdgHold) {
			netHdgHold = false;
		} else {
			netHdgHold = true;
		}
	}

	void OnChangeHdgHold (bool hdgHoldParam)
	{
		headingHold = hdgHoldParam;
	}



	[Command]
	public void CmdUpdateAutopilotButtons ()
	{
		//Debug.Log ("update net");
		netHdgHold = headingHold;
		netAltHold = altitudeHold;
		netSpdHold = airspeedHold;
	}

	public void toggleRings ()
	{
		CmdToggleRings ();
	}

	[Command]
	public void CmdToggleRings ()
	{
		if (ringsObject.activeInHierarchy) {
			ringsObject.SetActive (false);
			netRings = false;
		} else {
			ringsObject.SetActive (true);
			netRings = true;
		}
		RpcToggleRings ();
	}

	[ClientRpc]
	public void RpcToggleRings ()
	{
		ringsObject.SetActive (!netRings);
	}

	//	[Command]
	//	public void CmdGiveMeAuthority (GameObject obj)
	//	{
	//		NetworkIdentity objId = obj.GetComponent<NetworkIdentity> ();
	//		objId.AssignClientAuthority (connectionToClient);
	//	}
	//
	//	[Command]
	//	public void CmdReturnAuthority (GameObject obj)
	//	{
	//		NetworkIdentity objId = obj.GetComponent<NetworkIdentity> ();
	//		objId.RemoveClientAuthority (connectionToClient);
	//	}
}
