using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Test : MonoBehaviour
{
    public GameObject dddd = null;
    GameObject ddd;

    public Transform canas;


    void Start()
    {

         ddd = Instantiate(dddd, this.transform.position, Quaternion.identity);
        ddd.transform.SetParent(canas);
       
    }

    // Update is called once per frame
    void Update()
    {
        //sss.transform.Translate(new Vector3(0, 2 * Time.deltaTime, 0));
    }
}
