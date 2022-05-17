using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Text;

public class MapShapeSettings
{
    public List<NewShape> newshapes = new List<NewShape>();
    public NewShape newshape;
    public string Fuck = "Fuck";

    public void AddShape(bool[,] value, string name)
    {
        if (!CheckOverlab(name))
        {
            newshape = new NewShape(value);
            newshapes.Add(newshape);
        }
    }

    public bool CheckOverlab(string _name)
    {
        for (int i = 0; i < newshapes.Count; i++)
        {
            string shapename = newshapes[i].name;
            if (shapename == _name)
            {
                return true;
            }
        }
        return false;
    }

    public void RemoveShape(string _name)
    {
        for(int i = 0; i < newshapes.Count; i++)
        {
            string shapename = newshapes[i].name;
            if(shapename == _name)
            {
                newshapes.Remove(newshapes[i]);
                return;
            }
        }
    }

    public class NewShape
    {
        public string name;
        public bool[,] tilelist = new bool[10, 10];

        public NewShape(bool[,] value)
        {
            tilelist = value;
        }

    }

    public void printFunc()
    {
        Debug.Log(newshapes.Count);
        for (int i = 0; i < newshapes.Count; i++)
        {
            Debug.Log("asdfas");
            Debug.Log(newshapes[i].name);
        }
    }
}



public class MapShapeMake : MonoBehaviour
{
    public MapShapeSettings mapsettings;

    [SerializeField] private GameObject Tile;

    [SerializeField] private GameObject OKPanal = null;

    DetectObject detector = new DetectObject();

    int layerNum;

    string mapname;

    IEnumerator co = null;

    TileInfo[] tilelist;

    public bool[,] tilevalue;

    void Start()
    {
        tilevalue = new bool[10, 10];

        //FileStream stream = new FileStream(Application.dataPath + "/test.json", FileMode.OpenOrCreate);
        //if (stream == null)
        //{
        //    SaveInfo();
        //}

        mapsettings = new MapShapeSettings();

        mapname = "NULL";
        LoadInfo();
        //if (GetSettings == 0)
        //{
        //    SaveInfo();
        //    GetSettings = 1;
        //}
        //else if (GetSettings == 1)
        //{
        //    LoadInfo();
        //    GetSettings = 2;
        //}

        mapsettings.printFunc();

        Debug.Log(mapsettings.Fuck);

        tilelist = new TileInfo[100];

        layerNum = 1 << LayerMask.NameToLayer("Shape");
        for (int i = 0; i < 10;i++)
        {
            for(int j = 0; j < 10; j++)
            {
                var tile = Instantiate(Tile, new Vector3(j, 0, i), Quaternion.Euler(90,0,0));
                tile.transform.parent = this.transform;
                tile.GetComponent<TileInfo>().SetUp(j, i, false);
                tilelist[i*10 + j] = tile.GetComponent<TileInfo>();
            }
        } 
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Transform pos = detector.ReturnTransform(layerNum);
            if (pos != null)
            {
                bool Tf = !pos.GetComponent<TileInfo>().GetTf;
                co = ClickTile(Tf);
                StartCoroutine(co);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (co != null)
            {
                StopCoroutine(co);
            }
        }
    }
    IEnumerator ClickTile(bool _tf)
    {
        while (true)
        {
            if (Input.GetMouseButton(0))
            {
                Transform pos = detector.ReturnTransform(layerNum);

                if (pos != null)
                {
                    var tile = pos.GetComponent<TileInfo>();
                    tile.Change(_tf);

                    tilevalue[tile.GetX, tile.GetY] = tile.GetTf;
                }
            }
            yield return null;
        }
    }

    public void ResetBtn()
    {
        for(int i = 0; i < tilelist.Length; i++)
        {
            tilelist[i].Change(false);
        }
    }

    public int GetSettings { get => PlayerPrefs.GetInt("MapSetting"); set => PlayerPrefs.SetInt("MapSetting", value);}


    public void Setname(string name)
    {
        mapname = name;
    }

    public void ShowPanal()
    {
        OKPanal.SetActive(true);
    }
    public void OffPanal()
    {
        OKPanal.SetActive(false);
    }


    public void SaveInfo()
    {
        //if (mapname.Length <= 3)
        //{
        //    return;
        //}

        //for (int i = 0; i < mapsettings.newshape.Count; i++)
        //{
        //    if (mapname == mapsettings.newshape[i].name)
        //    {
        //        return;
        //    }
        //}

        mapsettings.AddShape(tilevalue, mapname);

        FileStream stream = new FileStream(Application.dataPath + "/test.json", FileMode.OpenOrCreate);
        string jsonData = JsonConvert.SerializeObject(mapsettings);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        stream.Write(data, 0, data.Length);
        stream.Close();

        Debug.Log(mapsettings.newshapes.Count + "저장 개수");

        mapname = null;
        OKPanal.SetActive(false);
        GetSettings = 1;
    }

    public void LoadInfo()
    {
        FileStream stream = new FileStream(Application.dataPath + "/test.json", FileMode.Open);
        byte[] data = new byte[stream.Length];
        stream.Read(data, 0, data.Length);
        stream.Close();

        string jsonData = Encoding.UTF8.GetString(data);
        MapShapeSettings test = JsonConvert.DeserializeObject<MapShapeSettings>(jsonData);
    }

    public void RemoveData(string name)
    {
        //for(int i = 0; i < mapsettings.newshape.Count; i++)
        //{
        //    if(mapsettings.newshape[i].name == name)
        //    {
        //        mapsettings.newshape.Remove(mapsettings.newshape[i]);
        //    }
        //}
    }

}
