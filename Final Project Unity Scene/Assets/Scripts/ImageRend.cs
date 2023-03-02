using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ImageRend : MonoBehaviour
{
    [SerializeField]
    Image image;
    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject ground;
    [SerializeField]
    int granularity = 10;
    [SerializeField]
    bool trackPlayer = false;

    private Color[,] map;
    //private Color[,] moving_map;
    private float step;
    private float len;

    private int lastX;
    private int lastY;

    private RaycastHit hit;
    const int CAPSULE = 10;
    const int WALL = 8;

    long filecounter = 0;

    [SerializeField]
    bool savePointCloud;

    // Start is called before the first frame update
    void Start()
    {
        len = ground.transform.localScale.x * 10;
        //Debug.Log("len:" + len);
        step = len / granularity;
        ResetMap();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //ResetMap();
        DrawPlayer();
        if (Raycast.hasHit)
            DrawObstacle();

        OutImage();
    }

    void DrawPlayer()
    {
        if (!trackPlayer)
            map[lastX, lastY] = Color.black;

        // draw player barycenter to image
        double x = player.transform.position.x / player.transform.localScale.x + len/2;
        double y = player.transform.position.z / player.transform.localScale.z + len/2;
        //Debug.Log("true:"+x + "," + y);

        int map_x = (int) Math.Floor(granularity * x / len);
        int map_y = (int) Math.Floor(granularity * y / len);
        //Debug.Log("map:"+map_x + "," + map_y);

        map[map_x, map_y] = Color.white;

        lastX = map_x;
        lastY = map_y;
    }

    void DrawObstacle()
    {
        hit = Raycast.hit;
        Color lineColor = Color.red;
        int c = 0;

        // draw player barycenter to image
        double x = hit.point.x + len / 2;
        double y = hit.point.z + len / 2;
        //Debug.Log("true:"+x + "," + y);

        int map_x = (int)Math.Floor(granularity * x / len);
        int map_y = (int)Math.Floor(granularity * y / len);
        //Debug.Log("map:"+map_x + "," + map_y);

        switch (hit.transform.gameObject.layer)
        {
            case CAPSULE:
                lineColor = Color.yellow;
                c = CAPSULE;
                //moving_map[map_x, map_y] = lineColor;
                break;
            case WALL:
                lineColor = Color.red;
                c = WALL;
                break;
        }
        map[map_x, map_y] = lineColor;

        if (savePointCloud)
            SavePoint(hit.point, c);
    }

    void ResetMap()
    {
        //restore map
        map = new Color[granularity, granularity];
        for (int i=0; i<granularity; i++)
        {
            for (int j = 0; j < granularity; j++)
            {
                if (map[i,j] != Color.red)
                {
                    map[i, j] = Color.black;  
                }
            }
        }
    }

    Texture2D OutImage()
    {
        //create 2D texture from matrix
        Texture2D tex = new Texture2D(granularity, granularity);
        for (int i = 0; i < granularity; i++)
        {
            for (int j = 0; j < granularity; j++)
            {
                tex.SetPixel(i, j, map[i, j]);
            }
        }
        tex.Apply();
        image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));

        return tex;
    }

    private void OnApplicationQuit()
    {
        //Encode it to PNG and save
        Texture2D tex = OutImage();
        var Bytes = tex.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/Test.png", Bytes);
    }

    void SavePoint(Vector3 point, int c)
    {
        string s = "";
            s += point[0].ToString() + ",";
            s += point[1].ToString() + ",";
            s += point[2].ToString() + ",";
            s += c.ToString() + "\n";

        File.AppendAllText(Application.dataPath + "/PointClouds/" + filecounter + ".txt", s);
        //filecounter++;
    }
}
