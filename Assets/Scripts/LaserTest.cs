using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LaserTest : MonoBehaviour
{

    private LineRenderer LR = null;
    public Transform target = null;
    void Start()
    {
  
        LR = this.GetComponent<LineRenderer>();
    }

    void Update()
    {
        LR.SetPosition(0, this.transform.position);
        LR.SetPosition(1, target.position);
    }
}
