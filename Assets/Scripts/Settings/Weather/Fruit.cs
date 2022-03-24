using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    private int remains = 20;
    public int GetRemains { get => remains; }

    private Vector3 originPos = Vector3.zero;
    public Vector3 GetOriginPos { get => originPos; }
    private TreeSc tree = null;

    private bool onwater = false;
    private bool ontile = false;

    private GameObject water = null;
    private GameObject Tile = null;

    public void Setup(Vector3 _originpos,TreeSc _tree)
    {
        remains = 20;
        originPos = _originpos;
        tree = _tree;
    }

    public void ReturnPos()
    {
        this.transform.position = originPos;
    }

    public void PosChange()
    {
        this.transform.position = originPos;
    }

    public void CountDown(int value)
    {
        remains -= value;
    }

    public void ReturnObj()
    {
        Debug.Log("Return");
        tree.ReturnFruit(this);
    }

    private void Update()
    {
        if(Tile!=null && water != null)
        {
            if(water.transform.localScale.y/2+0.375f >= originPos.y)
            {
                this.transform.position = new Vector3(originPos.x, water.transform.localScale.y / 2 + 0.33f, originPos.z);
                Debug.Log((water.transform.localScale.y - Tile.transform.localScale.y) - 0.17f);
            }
            else
            {
                this.transform.position = originPos;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tile"))
        {
            this.transform.position = originPos;
            Tile = other.gameObject;
            ontile = true;
        }
        if (other.CompareTag("Water"))
        {
            water = other.gameObject;
            onwater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Tile"))
        {
            ontile = false;
        }
        if (other.CompareTag("Water"))
        {
            onwater = false;
        }
    }
}
