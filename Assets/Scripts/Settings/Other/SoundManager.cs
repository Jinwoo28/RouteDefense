using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] AudioClips;
    AudioSource audioSource = new AudioSource();

    static AudioClip[] SAudio;

    AlertSetting AS = new AlertSetting();

    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();

        SAudio = AudioClips;

        
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
        audioSource.clip = AudioClips[soundNum];
        audioSource.Play();
    }

    public void InsthisObj(int num)
    {
        audioSource = this.GetComponent<AudioSource>();
        audioSource.clip = AudioClips[num];
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
