using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo : MonoBehaviour
{
    private int X;
    private int Y;
    private bool Tf;

    public int GetX => X;
    public int GetY => Y;
    public bool GetTf => Tf;

    private SpriteRenderer SR = null;

    private void Start()
    {
        SR = this.GetComponent<SpriteRenderer>();
    }

    public void SetUp(int x, int y, bool tf)
    {
        X = x;
        Y = y;
        Tf = tf;
    }

    public void Change(bool _tf)
    {
        Tf = _tf;

        if (Tf)
        {
            SR.color = Color.red;
        }
        else
        {
            SR.color = Color.white;
        }
    }
}
