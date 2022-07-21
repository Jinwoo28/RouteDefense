using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] DD;
    AudioSource audioSource = new AudioSource();

    static AudioClip[] SAudio;

    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();

        SAudio = DD;

        
        audioSource.volume = PlayerPrefs.GetFloat("ESound");
        SoundSettings.effectsound += SoundChange;
    }


    

    private void SoundChange(float x)
    {
 
            if (audioSource != null)
            {
                audioSource.volume = x;
            }
        
    }

    private void OnDestroy()
    {
        SoundSettings.effectsound -= SoundChange;
    }

    public void TurnOnSound(int soundNum)
    {
        audioSource.clip = DD[soundNum];
        audioSource.Play();
    }

    public void InsthisObj(int num)
    {
        audioSource = this.GetComponent<AudioSource>();
        audioSource.clip = DD[num];
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
