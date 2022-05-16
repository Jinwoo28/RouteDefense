using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapShapeMake : MonoBehaviour
{
    [SerializeField] private GameObject Tile;

    DetectObject detector = new DetectObject();

    int layerNum;

    IEnumerator co = null;

    TileInfo[] tilelist;

    void Start()
    {
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
                    pos.GetComponent<TileInfo>().Change(_tf);
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

    public void SaveInfo()
    {

    }

}
