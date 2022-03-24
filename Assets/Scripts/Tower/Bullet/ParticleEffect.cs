using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : Bullet
{
    [SerializeField] float ReturnTime = 0;
    private bulletpolling Op = null;
    protected float TImer = 0;
    protected override void Update()
    {
        TImer += Time.deltaTime;
        if (TImer >= ReturnTime)
        {
            particle.ReturnObject(this);
            TImer = 0;
        }
    }

    

}
