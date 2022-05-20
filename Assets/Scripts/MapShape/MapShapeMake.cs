using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using TMPro;

public class MapShapeSettings
{
    public List<NewShape> newshapes;

    public List<int> numlist = new List<int>();

    public MapShapeSettings()
    {
        newshapes = new List<NewShape>();
    }


    //맵 추가
    public void AddShape(NewShape newshape)
    {
        if (!CheckOverlab(newshape.name))
        {
            newshapes.Add(newshape);
            Debug.Log("저장완료");
        }
        else
        {
            for (int i = 0; i < newshapes.Count; i++)
            {
                string shapename = newshapes[i].name;
                if (shapename == newshape.name)
                {
                    newshapes[i] = newshape;
                }
            }
        }
    }

    //중복이름 체크
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

    //이름으로 삭제
    public void RemoveShape(string _name)
    {
        for(int i = 0; i < newshapes.Count; i++)
        {
            if(newshapes[i].name == _name)
            {
                Debug.Log(newshapes[i].name);
                newshapes.Remove(newshapes[i]);
                return;
            }
        }
    }

    //맵을 이름으로 출력
    public NewShape PrintFunc(string _name)
    {
        for(int i = 0; i < newshapes.Count; i++)
        {
            if(_name == newshapes[i].name)
            {
                return newshapes[i];
            }
        }

        return null;        
    }

    

    //맵의 이름과 인덱스를 가진 노드
    public class NewShape
    {
        public string name;
        public bool[,] tilelist = new bool[10, 10];

        public NewShape(bool[,] value, string _name)
        {
            name = _name;
            tilelist = value;
        }
    }
}



public class MapShapeMake : MonoBehaviour
{
    public static MapShapeSettings mapsettings = new MapShapeSettings();

    [SerializeField] private TMP_Dropdown maplist = null;

    [SerializeField] private GameObject Tile;

    [SerializeField] private GameObject OKPanal = null;
    [SerializeField] private GameObject Remove = null;

    DetectObject detector = new DetectObject();

    int layerNum;

    string mapname;

    IEnumerator co = null;

    TileInfo[,] tilelist;

    public bool[,] tilevalue;

    private int MapNumber = 0;

    

    void Start()
    {
        tilevalue = new bool[10, 10];

        mapname = "NULL";

        LoadInfo();


        tilelist = new TileInfo[10,10];

        layerNum = 1 << LayerMask.NameToLayer("Shape");
        for (int i = 0; i < 10;i++)
        {
            for(int j = 0; j < 10; j++)
            {
                var tile = Instantiate(Tile, new Vector3(j, 0, i), Quaternion.Euler(90,0,0));
                tile.transform.parent = this.transform;
                tile.GetComponent<TileInfo>().SetUp(j, i, false);
                tilelist[j,i] = tile.GetComponent<TileInfo>();
            }
        }

        InitDropBox();
        DropBoxChage(0);
    }

    int num = 100;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (tilevalue[j, i]) Debug.Log("true");
                    else Debug.Log("false");
                }
            }

        }

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
        for(int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                tilelist[j,i].Change(false);
                tilevalue[j, i] = false;
            }
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
    public void ShowRemove()
    {
        Remove.SetActive(true);
    }
    public void OffRemove()
    {
        Remove.SetActive(false);
    }

    public void InitDropBox()
    {
        maplist.ClearOptions();

        for(int i = 0; i < mapsettings.newshapes.Count; i++)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = mapsettings.newshapes[i].name;
            maplist.options.Add(option);
        }
    }
    public void DropBoxChage(int num)
    {
        maplist.value = num;

        for(int i = 0; i < 10; i++)
        {
            for(int j = 0; j < 10; j++)
            {
                tilelist[j, i].Change(mapsettings.newshapes[num].tilelist[j,i]);
                tilevalue[j, i] = tilelist[j, i].GetTf;

                Debug.Log(tilevalue[j, i]);
            }
        }
    }

    public void SaveInfo(int tf)
    {
        if (tf == 1)
        {
            if (mapname.Length <= 3)
            {
                return;
            }

            if (mapsettings.CheckOverlab(mapname))
            {
                return;
            }

            MapShapeSettings.NewShape newshape = new MapShapeSettings.NewShape(tilevalue, mapname);

            mapsettings.AddShape(newshape);
        }


        FileStream stream = new FileStream(Application.dataPath + "/MapShape.json", FileMode.OpenOrCreate);
        string jsonData = JsonConvert.SerializeObject(mapsettings, Formatting.Indented);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        stream.Write(data, 0, data.Length);
        stream.Close();

        //Save2();

        OKPanal.SetActive(false);

        InitDropBox();
        Debug.Log("맵 저장");
        ResetBtn();
        mapname = "";
    }

    public void Save2()
    {
        FileStream stream = new FileStream(Application.dataPath + "/MapShape.json", FileMode.OpenOrCreate);
        string jsonData = JsonConvert.SerializeObject(mapsettings, Formatting.Indented);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        stream.Write(data, 0, data.Length);
        stream.Close();
    }

    public void LoadInfo()
    {
        FileStream stream = new FileStream(Application.dataPath + "/MapShape.json", FileMode.Open);
        byte[] data = new byte[stream.Length];
        stream.Read(data, 0, data.Length);
        stream.Close();

        string jsonData = Encoding.UTF8.GetString(data);
        mapsettings = JsonConvert.DeserializeObject<MapShapeSettings>(jsonData);
    }

    public void RemoveMap()
    {


            mapsettings.RemoveShape(mapname);
        

        InitDropBox();

        Remove.SetActive(false);
        mapname = "";
    }




}
