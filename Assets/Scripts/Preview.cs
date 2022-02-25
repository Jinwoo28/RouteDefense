using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preview : MonoBehaviour
{
    private bool canbuildable = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {



    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tile"))
        {
            canbuildable = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Tile"))
        {
            canbuildable = true;
        }
    }

    public bool CanBuildable
    {
        get
        {
            return canbuildable;
        }
    }

}
