using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class dataRead : MonoBehaviour {

    public GameObject thingy;

    private char lineSep = '\n';
    private char fieldSep = ',';

    List<string[]> fileData;
    Vector3[] coordinates;

    // Use this for initialization
    void Start() {

        GameObject.FindGameObjectWithTag("AircraftJet").GetComponent<Rigidbody>().isKinematic = true;
        using (var reader = new StreamReader(@"C:\Users\vel\Desktop\UnityGazeExport.csv"))
        {
            List<string> X = new List<string>();
            List<string> Y = new List<string>();
            List<string> Z = new List<string>();
            List<string> Red = new List<string>();
            List<string> Green = new List<string>();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');
                X.Add(values[2]);
                Y.Add(values[3]);
                Z.Add(values[4]);
                Red.Add(values[7]);
                Green.Add(values[8]);
            }
            coordinates = new Vector3[X.Count];
            for (int i = 0; i < X.Count; i++)
            {
                try
                {
                    coordinates[i].x = -float.Parse(X[i]);
                    coordinates[i].y = float.Parse(Y[i]) + 2.8f;
                    coordinates[i].z = -float.Parse(Z[i]);
                    Color32 cubeColor = new Color32(byte.Parse(Red[i]), byte.Parse(Green[i]), 0, 255);
                    GameObject heatCube = (GameObject) Instantiate(thingy, coordinates[i], Quaternion.identity);
                    heatCube.GetComponent<MeshRenderer>().material.SetColor("_Color", cubeColor);
                }
                catch (System.Exception e)
                {
                    Debug.LogException(e, this);
                }
            }
        }

        GameObject[] goArray = GameObject.FindGameObjectsWithTag("HeatMap");

        foreach (GameObject go in goArray)
        {
            go.GetComponent<SphereCollider>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnCollisionStay(Collision collision)
    {
        
    }
}
