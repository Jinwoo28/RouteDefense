using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AlertKind
{
    bubu,
    Build,
    Cant,
    Click,
    Die,
    Emergency,
    GetCoin,
    OK,
    Sell,
    Start,
    Victory
}

[RequireComponent(typeof(AudioSource))]
public class AlertSetting : MonoBehaviour
{
    private AudioSource AS = null;

    public void PlaySound(AlertKind _kind,GameObject obj)
    {
        if(obj.GetComponent<AudioSource>() == null)
        {
            obj.AddComponent<AudioSource>();
        }

        AS = obj.gameObject.GetComponent<AudioSource>();

        string soundName = _kind.ToString();
        AudioClip clip = Resources.Load<AudioClip>("Sound/Button/" + soundName);

        AS.clip = clip;
        AS.Play();
    }
}
