using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MultiRaycast : MonoBehaviour
{
    [SerializeField]
    float rotSpeed = 20;
    [SerializeField]
    int length = 100;
    [SerializeField]
    LayerMask layerMask;

    // Layers
    const int FLOOR = 6;
    const int HOUSE = 7;
    const int ALIEN = 8;
    const int SPACESHIP = 9;
    const int TREE = 10;
    const int CAR = 11;
    

    long filecounter = 0;
    List<float[]> pcs;

    public float waitingTime = 100.0f;
    private double lastInterval;
    private bool incrementInterval = false;
    private int pointsMinimum = 10000;
    private int pointsCount = 0;

    private void Start()
    {
        // If there are point clouds in directory from a previous run, 
        // delete them
        cleanDir();
    }

    // Update is called once per frame
    void Update()
    {
        // Rotation of LiDAR on capsule
        transform.Rotate(new Vector3(0, 1f, 0) * rotSpeed * Time.deltaTime, Space.Self);

        // The different directions the rays of the LiDAR of the capsule point
        // to capture points

        int j;
        int k;
        List<Vector3> directions = new List<Vector3>();

        for (j = -5; j < 5; j++){
            for(k = -5; k < 5; k++){
                directions.Add(new Vector3(k*.1f, j*.01f, 1));
            }
        }
        
        RaycastHit[] hit = new RaycastHit[directions.Count];
        int i = 0;

        
        foreach (Vector3 dir in directions)
        {
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, transform.TransformDirection(dir), out hit[i], Mathf.Infinity, layerMask))
            {
                // Declare variables for colors
                float r = 0;
                float g = 0;
                float b = 0;
                Color lineColor = Color.cyan;
                int c = 0;

                switch (hit[i].transform.gameObject.layer)
                {
                    // Find what sort of object we hit and adjust color and 
                    // register layer accordingly
                    case HOUSE:
                        r = 1.0f;
                        g = 0.0f;
                        b = 0.0f;
                        lineColor = new Vector4(r, g, b, 1); // Red
                        c = HOUSE;
                        break;
                    case ALIEN:
                        r = 0.0f;
                        g = 1.0f;
                        b = 0.0f;
                        lineColor = new Vector4(r, g, b, 1); // Green
                        c = ALIEN;
                        break;
                    case FLOOR:
                        r = 0.0f;
                        g = 0.0f;
                        b = 1.0f;
                        lineColor = new Vector4(r, g, b, 1); // Blue
                        c = FLOOR;
                        break;
                    case SPACESHIP:
                        r = 1.0f;
                        g = 0.0f;
                        b = 1.0f;
                        lineColor = new Vector4(r, g, b, 1); // Magenta
                        c = SPACESHIP;
                        break;
                    case TREE:
                        r = 1.0f;
                        g = 0.92f;
                        b = 0.016f;
                        lineColor = new Vector4(r, g, b, 1); // Yellow
                        c = TREE;
                        break;
                    case CAR:
                        r = 0.0f;
                        g = 0.0f;
                        b = 0.0f;
                        lineColor = new Vector4(r, g, b, 1); // Black
                        c = CAR;
                        break;
                }
                Debug.DrawRay(transform.position, transform.TransformDirection(dir) * hit[i].distance, lineColor);
                //Debug.Log("Did Hit " + i);
                pointsCount++;
                SavePoint(hit[i].point, c, r, g, b);
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(dir) * length, Color.white);
                //Debug.Log("Did not Hit");
            }

            // Check if we have reached the minimum amount of points we want 
            // and if we have, increment file counter
            if (pointsCount > pointsMinimum)
            {
                Debug.Log("Filled file: " + filecounter);
                // Move onto next file
                filecounter++;
                pointsCount = 0;
                
            }

            i++;

            print(filecounter);
            print(i);
        }
    }

    void cleanDir(){
        // At the start of the application, delete the existing point cloud
        // files in our Point Cloud directory 
        print("Deleting pre-existing point cloud files");

        string path = Application.dataPath + "/PointClouds";

        if (Directory.Exists(path)){
            Directory.Delete(path, true);
        }
        Directory.CreateDirectory(path);
    }

    void SavePoint(Vector3 point, int c, float r, float g, float b)
    {
        string s = "";
        s += point[0].ToString() + ",";
        s += point[1].ToString() + ",";
        s += point[2].ToString() + ",";
        s += r.ToString() + ",";
        s += g.ToString() + ",";
        s += b.ToString() + "\n";
        
        print(s);

        File.AppendAllText(Application.dataPath + "/PointClouds/" + filecounter + ".txt", s);
        //filecounter++;
    }
}
