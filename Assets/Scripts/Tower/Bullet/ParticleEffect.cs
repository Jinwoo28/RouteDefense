using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : Bullet
{
    [SerializeField] float ReturnTime = 0;
    private bulletpolling Op = null;
    protected float TImer = 0;

    private AudioSource AS = null;

    protected override void Start()
    {
        base.Start();
        AS = this.GetComponent<AudioSource>();
        AS.volume = PlayerPrefs.GetFloat("ESound");
        SoundSettings.effectsound += SoundChange;
    }

    private void SoundChange(float x)
    {
        AS.volume = x;
    }

    private void OnDestroy()
    {
        SoundSettings.effectsound -= SoundChange;
    }

    protected override void Update()
    {
        TImer += Time.deltaTime;
        if (TImer >= ReturnTime)
        {
            TImer = 0;
            particle.ReturnObject(this);
        }
    }

    public void ResetTime()
    {
        
    }



    

}
