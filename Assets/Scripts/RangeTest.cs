using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeTest : MonoBehaviour
{
    [SerializeField] private Transform[] range = null;

    Vector3 OriginPos;
    float Y = 0;
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            range[i].localScale = new Vector3(1, 1, 1);
        }
         OriginPos = this.transform.position;
        moveUp = OriginPos.y;
    }

    // Update is called once per frame
    void Update()
    {


        ShowRange();
        
    }
    float moveUp = 0;
    public void ShowRange()
    {


        
        moveUp += Time.deltaTime*0.7f;
        this.transform.position = new Vector3(OriginPos.x, moveUp, OriginPos.z);
        if (moveUp >= 0.5f)
        {
            moveUp = 0;
            this.transform.position = OriginPos;
        }
    }
}
