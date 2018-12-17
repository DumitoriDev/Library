using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class AudioBtn : MonoBehaviour {

    [SerializeField] private Sprite soundOnSprite,soundOffSprite;

    [SerializeField] string AudioKey;
    protected bool soundOn;
    protected AudioSource audioSource;

    protected Image image;
    protected void Start () {
        audioSource = GetComponent<AudioSource>();
        image = GetComponent<Image>();

        GetKey();
        SetSoundByKey();
    }

    protected void GetKey()
    {
        soundOn = (PlayerPrefs.GetString(AudioKey) != "no") ? true : false;
    }

    public void TurnAudio()
    {
        soundOn = !soundOn;
        string value = (soundOn) ? "yes" : "no";
        PlayerPrefs.SetString(AudioKey, value);

        SetSoundByKey();
    }

    protected void SetSoundByKey()
    {
        if(soundOn)
        {
            audioSource.mute = false;
            image.sprite = soundOnSprite;
        }
        else
        {
            audioSource.mute = true;
            image.sprite = soundOffSprite;
        }
    }
}
