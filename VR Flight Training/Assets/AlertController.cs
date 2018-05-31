using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AlertController : MonoBehaviour
{
    public Material Green;
    public Material Yellow;
    public Material Red;
    public Material Grey;

    public static bool started = false;

    public static bool smallLooked = false;
    public static bool medLooked = false;
    public static bool circlesLooked = false;
    public static bool centerLooked = false;
    public static bool rightLooked = false;

    GameObject[] smallButtons;
    GameObject[] mediumButtons;
    GameObject[] circles;
    GameObject[] centerScreen;
    GameObject[] rightScreen;

    // Use this for initialization
    void Start()
    {
        smallButtons = GameObject.FindGameObjectsWithTag("SmallButtons");
        mediumButtons = GameObject.FindGameObjectsWithTag("MediumButtons");
        circles = GameObject.FindGameObjectsWithTag("Circles");
        centerScreen = GameObject.FindGameObjectsWithTag("CenterScreen");
        rightScreen = GameObject.FindGameObjectsWithTag("RightScreen");
        setObjectColor(smallButtons, Green);
        setObjectColor(mediumButtons, Green);
        setObjectColor(circles, Green);
        setObjectColor(centerScreen, Green);
        setObjectColor(rightScreen, Green);
    }

    void setObjectColor(GameObject[] obj, Material mat)
    {
        for (int i = 0; i < obj.Length; i++)
        {
            obj[i].GetComponent<Renderer>().material = mat;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.H))
        {
            if (!started)
            {
                StartCoroutine(smallButtonController());
                StartCoroutine(mediumButtonController());
                StartCoroutine(circleButtonController());
                StartCoroutine(rightScreenController());
                StartCoroutine(centerScreenController());
            }

            started = true;

            /*if (started)
            {
                StartCoroutine(smallButtonController());
                StartCoroutine(mediumButtonController());
                StartCoroutine(circleButtonController());
                StartCoroutine(rightScreenController());
                StartCoroutine(centerScreenController());
            }*/
        }
    }

    int smallButtonCounter = 0;
    bool smallPlayed;
    IEnumerator smallButtonController()
    {
        smallButtonCounter++;
        if (smallLooked)
        {
            smallButtonCounter = 0;
            setObjectColor(smallButtons, Green);
            GameObject.Find("SmallButtonSound").GetComponent<AudioSource>().Stop();
            smallLooked = false;
            smallPlayed = false;
        }
        if(smallButtonCounter > 4000)
        {
            if (!smallPlayed)
            {
                GameObject.Find("SmallButtonSound").GetComponent<AudioSource>().Play();
                smallPlayed = true;
            }
            setObjectColor(smallButtons, Red);
        }
        else if(smallButtonCounter > 1200)
        {
            setObjectColor(smallButtons, Yellow);
        }
        yield return null;
    }

    int mediumButtonCounter = 0;
    bool mediumPlayed;
    IEnumerator mediumButtonController()
    {
        mediumButtonCounter++;
        if (medLooked)
        {
            mediumButtonCounter = 0;
            setObjectColor(mediumButtons, Green);
            GameObject.Find("MediumButtonSound").GetComponent<AudioSource>().Stop();
            medLooked = false;
            mediumPlayed = false;
        }
        if (mediumButtonCounter > 5000)
        {
            if (!mediumPlayed)
            {
                GameObject.Find("MediumButtonSound").GetComponent<AudioSource>().Play();
                mediumPlayed = true;
            }
            setObjectColor(mediumButtons, Red);
        }
        else if (mediumButtonCounter > 1000)
        {
            setObjectColor(mediumButtons, Yellow);
        }
        yield return null;
    }

    int circleButtonCounter = 0;
    bool circlePlayed;
    IEnumerator circleButtonController()
    {
        circleButtonCounter++;
        if (circlesLooked)
        {
            circleButtonCounter = 0;
            setObjectColor(circles, Green);
            GameObject.Find("CirclesSound").GetComponent<AudioSource>().Stop();
            circlesLooked = false;
            circlePlayed = false;
        }
        if (circleButtonCounter > 3000)
        {
            if (!circlePlayed)
            {
                GameObject.Find("CirclesSound").GetComponent<AudioSource>().Play();
                circlePlayed = true;
            }
            setObjectColor(circles, Red);
        }
        else if (circleButtonCounter > 2000)
        {
            setObjectColor(circles, Yellow);
        }
        yield return null;
    }

    int rightScreenCounter = 0;
    bool rightPlayed;
    IEnumerator rightScreenController()
    {
        rightScreenCounter++;
        if (rightLooked)
        {
            rightScreenCounter = 0;
            setObjectColor(rightScreen, Green);
            GameObject.Find("RightScreenSound").GetComponent<AudioSource>().Stop();
            rightLooked = false;
            rightPlayed = false;
        }
        if (rightScreenCounter > 2250)
        {
            if (!rightPlayed)
            {
                GameObject.Find("RightScreenSound").GetComponent<AudioSource>().Play();
                rightPlayed = true;
            }
            setObjectColor(rightScreen, Red);
        }
        else if (rightScreenCounter > 750)
        {
            setObjectColor(rightScreen, Yellow);
        }
        yield return null;
    }

    int centerScreenCounter = 0;
    bool centerPlayed;
    IEnumerator centerScreenController()
    {
        centerScreenCounter++;
        if (centerLooked)
        {
            centerScreenCounter = 0;
            setObjectColor(centerScreen, Green);
            GameObject.Find("CenterScreenSound").GetComponent<AudioSource>().Stop();
            centerLooked = false;
            centerPlayed = false;
        }
        if (centerScreenCounter > 1500)
        {
            if (!centerPlayed)
            {
                GameObject.Find("CenterScreenSound").GetComponent<AudioSource>().Play();
                centerPlayed = true;
            }
            setObjectColor(centerScreen, Red);
        }
        else if (centerScreenCounter > 500)
        {
            setObjectColor(centerScreen, Yellow);
        }
        yield return null;
    }
}
