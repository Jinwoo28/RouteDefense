using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    // 슬라이더로 사운드 조절
    // 만약 사운드가 0이면 이미지 끄기

    // 온오프 버튼으로 사운드 끄고 켜기

    // -> 오프를 눌렀을 때 행동           
    // 1. 사운드 이미지 변경 (Off Image)
    // 2. 현재 사운드를 임시 저장소에 보관
    // 3. 현재 사운드를 0으로 변경

    // -> 온으로 돌렸을 때 행동
    // 1. 사운드 이미지 변경 (On Image)
    // 2. 임시 저장소에 있던 사운드 값으로 현재 사운드 값 변경

    // 시작시 하는 행동
    // 1. 이 스크립트는 Main Audio(BGM)에 달려있다.
    // 2. 시작시 GetComponent로 Main Audio를 가져온다.
    // 3. BGM Slider와 Effect Slider의 값을 해당 Properties의 값으로 초기화시킨다.
    // 4. MainAudio의 Volumn을 BGM Properties의 값으로 초기화 시킨다.
    // ++ effect sound를 가진 오브젝트들도 생성시 Effect Properties의 값으로 Volumn을 초기화 시킨다.


    //BGM 사운드 조절 slider와 버튼 이미지
    [SerializeField] private GameObject BGMSoundOffImage = null;
    [SerializeField] private Slider BGMslider = null;

    //Effect 사운드 조절 slider와 버튼 이미지
    [SerializeField] private GameObject EffectSoundOffImage = null;
    [SerializeField] private Slider Effectslider = null;

    //BGM sound
    private AudioSource MainAudio = null;

    //효과음들은 델리게이트로 사운드 조절
    public delegate void EffectSound(float volumn);
    static public EffectSound effectsound;

    void Start()
    {
        MainAudio = this.GetComponent<AudioSource>();

        //스크립트 활성화 시 슬라이더와 사운드 초기화
        BGMslider.value = BGMVolumn;
        Effectslider.value = EffectVolumn;
    }

    #region Sound PlayerPrefs Properties

    //음소거 시 사운드를 임시 저장할 Properties
    public float TMPBGM
    {
        get => PlayerPrefs.GetFloat("TMPBGM");
        set => PlayerPrefs.SetFloat("TMPBGM", value);
    }
    public float TMPEffect
    {
        get => PlayerPrefs.GetFloat("TMPEffect");
        set => PlayerPrefs.SetFloat("TMPEffect", value);
    }

    //BGM사운드 볼륨을 PlayerPrefs에서 저장 출력
    public float BGMVolumn
    {
        get => PlayerPrefs.GetFloat("BSound");
        set => PlayerPrefs.SetFloat("BSound", value);
    }

    //Effect사운드 볼륨을 PlayerPrefs에서 저장 출력
    public float EffectVolumn
    {
        get => PlayerPrefs.GetFloat("ESound");
        set => PlayerPrefs.SetFloat("ESound", value);
    }
    #endregion




    #region Sound Volumn Change
    //dynamic으로 slider 값이 변할 때 호출
    public void EffectSoundChange(float _volumn)
    {
        //PlayerPrefs에 값 저장
        EffectVolumn = _volumn;

        //delegate로 저장되어 있는 함수의 volumn값 변경
        //delegate에 저장된 함수가 있을 경우에만 실행
        if(effectsound != null)
        {
            effectsound(_volumn);
        }

        if (_volumn != 0)
        {
            EffectSoundOffImage.SetActive(false);
        }
        else if(_volumn == 0)
        {
            EffectSoundOffImage.SetActive(true);
        }

    }    
    public void BGMSoundChange(float _volumn)
    {
        //PlayerPrefs에 값 저장
        BGMVolumn = _volumn;

        //MainAudio의 Volumn 변경
        MainAudio.volume = _volumn;

        if (_volumn != 0)
        {
            BGMSoundOffImage.SetActive(false);
        }
        else if (_volumn == 0)
        {
            BGMSoundOffImage.SetActive(true);
        }
    }
    #endregion

    #region OnOff Button

    //사운드 이미지 클릭 시 현재 사운드 값을 임시저장
    public void BGMSoundOff()
    {
        TMPBGM = BGMVolumn;
        BGMslider.value = 0;
        BGMSoundOffImage.SetActive(true);
    }

    public void BGMSoundOn()
    {
        BGMslider.value = TMPBGM;
        BGMSoundOffImage.SetActive(false);
    }

    public void EffectSoundOff()
    {
        TMPEffect = EffectVolumn;
        Effectslider.value = 0;
        EffectSoundOffImage.SetActive(true);
    }

    public void EffectSoundOn()
    {
        Effectslider.value = TMPEffect;
        EffectSoundOffImage.SetActive(false);
    }
    #endregion

}
