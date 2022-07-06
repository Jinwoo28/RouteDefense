using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillParent : MonoBehaviour
{
    private AudioSource As = null;

    [SerializeField] private bool X;

    private void Awake()
    {
        if (!X)
        {
            As = this.GetComponent<AudioSource>();
            As.volume = PlayerPrefs.GetFloat("ESound");
            SoundSettings.effectsound += SoundChange;
        }
    }
    private void SoundChange(float x)
    {

        if (As != null)
        {
            As.volume = x;
        }

    }

    private void OnDestroy()
    {
        SoundSettings.effectsound -= SoundChange;
    }

}
