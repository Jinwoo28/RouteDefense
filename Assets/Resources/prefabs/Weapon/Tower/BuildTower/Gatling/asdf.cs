using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asdf : MonoBehaviour
{
    [SerializeField] private Material[] Ma;

    
    void Start()
    {
        Debug.Log(GetComponentInChildren<Renderer>().sharedMaterial.GetColor("_Color"));   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GetComponentInChildren<Renderer>().sharedMaterial.SetColor("_Color", new Color(1, 0, 0));
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            GetComponentInChildren<Renderer>().sharedMaterial.SetColor("_Color", new Color(0, 1, 0));
        }
    }
}
