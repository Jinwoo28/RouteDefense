using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumRotation : MonoBehaviour
{
    private Camera cam;
    private Vector3 relativePos = Vector3.zero;

    [SerializeField] private bool Move;
    float MoveTime = 0;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Vector3 relativePos = cam.transform.position - this.transform.position;

        Quaternion rotationtotarget = Quaternion.LookRotation(relativePos);

        Vector3 Dir = Quaternion.RotateTowards(this.transform.rotation, rotationtotarget, 360).eulerAngles;

        this.transform.rotation = Quaternion.Euler(0, 180+Dir.y, 0);

        if (Move)
        {
            MoveTime += Time.deltaTime*5;


                this.transform.position += new Vector3(0, Mathf.Sin(MoveTime)*0.0075f, 0);

        }
    }
}
