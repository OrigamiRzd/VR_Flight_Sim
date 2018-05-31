using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using UnityEngine;

public class DataRecorder : MonoBehaviour {
    
    private static string currentFilePath;
    public static string secondaryFilePath = @"C:\Users\vel\Desktop\Pupil Recordings\2018_04_24\031\UnityGazeExport.csv";

    private static PupilSettings Settings
    {
        get { return PupilSettings.Instance; }
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            if (isRecording)
            {
                StopRecording();
            }
            if (currentFilePath != null)
            {
                createHeatmap();
            }
            else
            {
                if (secondaryFilePath != null)
                {
                    currentFilePath = secondaryFilePath;
                    createHeatmap();
                }
            }
        }
	}

    private static bool isRecording = false;
    public static void StartRecording()
    {
        var _p = Settings.recorder.GetRecordingPath().Substring(2);

        PupilTools.Send(new Dictionary<string, object> {
            { "subject","recording.should_start" },
             {
                "session_name",
                _p
            }
        });

        isRecording = true;

        recordingString = "Timestamp,HitObject,HitPointX,HitPointY,HitPointZ\n";
    }

    private static string recordingString;

    public static void StopRecording()
    {

        PupilTools.Send(new Dictionary<string, object> { { "subject", "recording.should_stop" } });

        isRecording = false;
    }

    public static void AddToRecording(RaycastHit hit, Vector3 hitSpot)
    {
        //Vector3 hitSpot = GameObject.Find("mustang").transform.worldToLocalMatrix.MultiplyPoint(hit.point);
        //UnityEngine.Debug.Log("AddToRecording");
        //var timestamp = TimestampForDictionary(gazeDictionary);

        recordingString += string.Format("{0},{1},{2},{3},{4}\n"
            , Time.deltaTime.ToString("F4")
            , hit.collider.gameObject.ToString()
            , hitSpot.x.ToString("F4")
            , hitSpot.y.ToString("F4")
            , hitSpot.z.ToString("F4")
        );
    }

    public static Dictionary<string, object> pupil0Dictionary;
    public static Dictionary<string, object> pupil1Dictionary;
    private static Dictionary<string, object> _gazeDictionary;
    public static Dictionary<string, object> gazeDictionary
    {
        get
        {
            return _gazeDictionary;
        }
        set
        {
            _gazeDictionary = value;
        }
    }

    public static void SaveRecording(string toPath)
    {
        string filePath = toPath + "/" + "UnityGazeExport.csv";
        currentFilePath = filePath;
        File.WriteAllText(filePath, recordingString);
    }

    public static float TimestampForDictionary(Dictionary<string, object> dictionary)
    {
        object timestamp;
        dictionary.TryGetValue("timestamp", out timestamp);
        return (float)(double)timestamp;
    }

    public GameObject thingy;

    private char lineSep = '\n';
    private char fieldSep = ',';

    List<string[]> fileData;
    Vector3[] coordinates;
    List<float> X = new List<float>();
    List<float> Y = new List<float>();
    List<float> Z = new List<float>();
    List<int> CountIfs = new List<int>();
    List<float> Percentrank = new List<float>();
    List<int> Red = new List<int>();
    List<int> Green = new List<int>();

    // Pulled from dataRead
    void createHeatmap()
    {
        using (var reader = new StreamReader(currentFilePath))
        {
            reader.ReadLine();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');
                X.Add(float.Parse(values[2]));
                Y.Add(float.Parse(values[3]));
                Z.Add(float.Parse(values[4]));
            }
        }

        coordinates = new Vector3[X.Count];

        CountIf();
        PercentRank();
        addRed();
        addGreen();

        for (int i = 0; i < X.Count; i++)
        {
            try
            {
                coordinates[i].x = -(X[i] * 1.4f) - .04f;
                coordinates[i].y = (Y[i] * 1.4f) + 2.5f;
                coordinates[i].z = -(Z[i] * 1.4f) - 1.35f;
                //UnityEngine.Debug.Log("X: " + X[i] + "\tY: " + Y[i] + "\tZ: " + Z[i] + "\tCountif: " + CountIfs[i] + "\tPercentRank: " + Percentrank[i] + "\tRed: " + Red[i] + "\tGreen: " + Green[i]);
                Color32 cubeColor = new Color32((byte)Red[i], (byte)Green[i], 0, 255);
                GameObject heatCube = Instantiate(thingy, coordinates[i], Quaternion.identity);
                heatCube.GetComponent<MeshRenderer>().material.SetColor("_Color", cubeColor);
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogException(e, this);
            }
        }
    }

    void CountIf()
    {
        for (int i = 0; i < X.Count; i++)
        {
            int ret = 0;
            for (int j = 0; j < X.Count; j++)
            {
                if ((X[j] >= (X[i] - 0.025f)) && (X[j] <= (X[i] + 0.025f)) && (Y[j] >= (Y[i] - 0.025f)) && (Y[j] <= (Y[i] + 0.025f)))
                {
                    ret++;
                }
            }
            CountIfs.Add(ret);
        }
    }

    void PercentRank()
    {
        for (int i = 0; i < X.Count; i++)
        {
            float countAbove = 0;
            float countBelow = 0;
            for (int j = 0; j < X.Count; j++)
            {
                if(CountIfs[i] > CountIfs[j])
                {
                    countBelow++;
                }
                else if (CountIfs[i] < CountIfs[j])
                {
                    countAbove++;
                }
            }
            float ret = (countBelow / (countBelow + countAbove));
            Percentrank.Add(ret);
        }
    }

    void addRed()
    {
        for (int i = 0; i < X.Count; i++)
        {
            if (Percentrank[i] < 0.5f)
            {
                Red.Add((int)(Percentrank[i] * 500));
            }
            else
            {
                Red.Add((int)255);
            }
        }
    }

    void addGreen()
    {
        for (int i = 0; i < X.Count; i++)
        {
            if (Percentrank[i] > 0.5f)
            {
                Green.Add((int)(255 - ((Percentrank[i] - 0.5f) * 500)));
            }
            else
            {
                Green.Add((int)255);
            }
        }
    }
}
