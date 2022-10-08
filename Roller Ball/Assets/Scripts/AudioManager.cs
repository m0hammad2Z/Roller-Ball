using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource gameMusic;
    public static bool muteGameMusic;
    
    void Start()
    {
        gameMusic = GetComponent<AudioSource>();    
        muteGameMusic = PlayerPrefs.GetInt("MuteMainMusic") == 1 ? true : false; 
    }

    void Update()
    {
        if(muteGameMusic == true)
        {
            gameMusic.mute = true;
        }else gameMusic.mute = false;
    }

    public static void Mute()
    {
        muteGameMusic = true;
        PlayerPrefs.SetInt("MuteMainMusic", muteGameMusic == false ? 0 : 1);
    }

    public static void UnMute()
    {
        muteGameMusic = false;
        PlayerPrefs.SetInt("MuteMainMusic", muteGameMusic == false ? 0 : 1);
    }
}
