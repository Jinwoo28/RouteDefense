using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemy : Enemy_Creture
{
    [SerializeField] private Transform Body = null;
    [SerializeField] private Transform Mark = null;

    [SerializeField] private float height = 0;

    protected override void Start()
    {
        base.Start();
        Body.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + height, this.transform.position.z);
        Mark.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.125f, this.transform.position.z);
    }

    float X = 0;
    private void Update()
    {
       // Body.transform.position += Vector3.up * Time.deltaTime*0.25f;

        X += Time.deltaTime*180; 
        Mark.rotation = Quaternion.Euler(-90, 0, X);
    }

    public Transform GetBody() => Body;

    
}
