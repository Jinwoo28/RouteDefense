using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Newtonsoft.Json;

public class LoadMap : MonoBehaviour
{
    private static bool mapSuccess = false;

    public static MapShapeSettings map = new MapShapeSettings();

    void Start()
    {
        if (!mapSuccess)
        {
            LoadMapInfo();
            mapSuccess = true;
        }
    }

    public void LoadMapInfo()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "MapShape.json");
        string mapdata = File.ReadAllText(path);

        map = JsonConvert.DeserializeObject<MapShapeSettings>(mapdata);
    }
}
