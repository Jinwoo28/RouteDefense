using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Newtonsoft.Json;

public class LoadMap : MonoBehaviour
{
    private static bool MapSuccess = false;

    public static MapShapeSettings map = new MapShapeSettings();

    void Start()
    {
        if (!MapSuccess)
        {
            LoadInfo();
            MapSuccess = true;
        }
    }


    public void LoadInfo()
    {
        FileStream stream = new FileStream(Application.dataPath + "/MapShape.json", FileMode.Open);
        byte[] data = new byte[stream.Length];
        stream.Read(data, 0, data.Length);
        stream.Close();

        string jsonData = Encoding.UTF8.GetString(data);
        map = JsonConvert.DeserializeObject<MapShapeSettings>(jsonData);
    }
}
