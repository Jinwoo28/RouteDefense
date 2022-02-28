using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Test2 : MonoBehaviour
{
    TextMeshProUGUI text=null;
    Color color;
    Camera cam = null;

    public Transform _object = null;
    
    void Start()
    {
        cam = Camera.main;
        text = this.GetComponent<TextMeshProUGUI>();
        color = text.color;
        color = Color.red;
        text.color = color;
    }

    // Update is called once per frame
    float x = 0;
    void Update()
    {
        x += Time.deltaTime; 

        text.transform.position = cam.WorldToScreenPoint(new Vector3(_object.position.x, _object.position.y+x, _object.position.z)); 
    }
}
