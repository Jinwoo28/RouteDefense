using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    float X = 0;
    // Update is called once per frame
    void Update()
    {
        X += Time.deltaTime;
        //Debug.Log(Vector3.Lerp(Vector3.zero, new Vector3(10,10,10), X));
    }
}
