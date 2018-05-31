using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Aeroplane
{
    [RequireComponent(typeof (AeroplaneController))]
    public class AeroplaneUserControl4Axis : MonoBehaviour
    {
        // these max angles are only used on mobile, due to the way pitch and roll input are handled
        public float maxRollAngle = 80;
        public float maxPitchAngle = 80;

        // reference to the aeroplane that we're controlling
        private AeroplaneController m_Aeroplane;
        private float m_Throttle;
        private bool m_AirBrakes;
        private float m_Yaw;

        public static float Roll;
        public static float Pitch;
        public static float Yaw;
        public static float ThrottleL;
        public static float ThrottleR;

        //public Transform puck;
        //public Transform puckTarget;

        private int yoke = 1;
        private int pedals = 2;
        private int Logitech3DPro = 3;

        private bool use3DPro = false;
        public bool useTracker = false;


        private void Awake()
        {
            // Set up the reference to the aeroplane controller.
            m_Aeroplane = GetComponent<AeroplaneController>();
        }

        void Start()
        {
            string[] names = Input.GetJoystickNames();
            //Debug.Log(names.Length);
            for (int i = 0; i < names.Length; i++)
            {
                if (names[i] == "Saitek Pro Flight Rudder Pedals")
                {
                    pedals = i + 1;
                    //Debug.Log("rudder found " + i + "\t" + names[i]);
                }
                else if (names[i] == "Saitek Pro Flight Yoke")
                {
                    yoke = i + 1;
                    //Debug.Log("yoke found " + i + "\t" + names[i]);
                }
                else if (names[i] == "Logitech Extreme 3D")
                {
                    use3DPro = true;
                    Logitech3DPro = i + 1;
                }
            }

            //puckTarget.rotation = puck.rotation;


        }

        private void FixedUpdate()
        {
            // Read input for the pitch, yaw, roll and throttle of the aeroplane.
            float roll = Input.GetAxisRaw("Joy" + yoke + "Axis1");
            float pitch = (-1)*Input.GetAxisRaw("Joy" + yoke + "Axis2");
            m_AirBrakes = CrossPlatformInputManager.GetButton("Fire1");
            m_Yaw = (-1)*Input.GetAxisRaw("Joy" + pedals + "Axis3");
            m_Throttle = (((-1)*Input.GetAxisRaw("Joy" + yoke + "Axis3")));
            float airBrakes = (Input.GetAxisRaw("Joy" + pedals + "Axis1"));
            if(airBrakes < 0)
            {
                m_AirBrakes = false;
            }
            else
            {
                m_AirBrakes = true;
            }
#if MOBILE_INPUT
        AdjustInputForMobileControls(ref roll, ref pitch, ref m_Throttle);
#endif
            // Pass the input to the aeroplane
            m_Aeroplane.Move(roll, pitch, m_Yaw, m_Throttle, m_AirBrakes);
            //Debug.Log("Roll out:\t" +roll);
            //Debug.Log("Pitch Perfect:\t" + pitch);
            //Debug.Log("Yaw Yaw:\t" + m_Yaw);
            //Debug.Log("Many thrusts:\t" + m_Throttle);
            //Debug.Log("airbrakes:\t" + airBrakes);

        }


        private void AdjustInputForMobileControls(ref float roll, ref float pitch, ref float throttle)
        {
            // because mobile tilt is used for roll and pitch, we help out by
            // assuming that a centered level device means the user
            // wants to fly straight and level!

            // this means on mobile, the input represents the *desired* roll angle of the aeroplane,
            // and the roll input is calculated to achieve that.
            // whereas on non-mobile, the input directly controls the roll of the aeroplane.

            float intendedRollAngle = roll*maxRollAngle*Mathf.Deg2Rad;
            float intendedPitchAngle = pitch*maxPitchAngle*Mathf.Deg2Rad;
            roll = Mathf.Clamp((intendedRollAngle - m_Aeroplane.RollAngle), -1, 1);
            pitch = Mathf.Clamp((intendedPitchAngle - m_Aeroplane.PitchAngle), -1, 1);
        }
    }
}
