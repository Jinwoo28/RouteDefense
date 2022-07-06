using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    EnemyManager EM;
    AudioSource AS;
    public EnemyManager SetEM { set => EM = value; }

    EnemyPooling EP;
    public EnemyPooling SetEp { set => EP = value; }

    public bool iscoin;

    public void InitSC()
    {
        AS = this.GetComponent<AudioSource>();
        AS.volume = PlayerPrefs.GetFloat("ESound");
        SoundSettings.effectsound += SoundChange;
    }

    private void OnDestroy()
    {
        SoundSettings.effectsound -= SoundChange;
    }

    private void SoundChange(float x)
    {
        if (AS != null)
        {
            AS.volume = x;
        }
    }

    private void Update()
    {
        this.transform.position += transform.up * Time.deltaTime*2;

    }

    public void SetUP(Vector3 position)
    {
        
        AS.Play();
        Invoke("ReturnCoin", 0.5f);
    }

    private void ReturnCoin()
    {
        EP.ReturnCoin(this, iscoin?0:1);
    }
}

