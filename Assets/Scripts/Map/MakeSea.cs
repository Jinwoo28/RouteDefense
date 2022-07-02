using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSea : MonoBehaviour
{
    [SerializeField] private GameObject Sea = null;

    private void Awake()
    {
        for(int i = 0; i < 3; i++)  //세로 
        {
            for(int j = 0;j < 3; j++)   //가로
            {
                for (int k = 0; k < 2; k++) //한 블럭 안에 세로
                {
                    for (int l = 1; l < 3; l++) //한 블럭 안에 가로
                    {
                        //-12.5   -> -7.5
                        //-12.5   -> -7.5
                        var sea =Instantiate(Sea, new Vector3((2.5f + l * 5)+(j*10), this.transform.position.y, (2.5f + k * 5)+(i*10)), Quaternion.identity,this.transform);
                        Debug.Log(sea.transform.position);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
