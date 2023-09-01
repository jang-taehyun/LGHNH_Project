using GameManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource[] audioSources;

    private void Start()
    {
        SettingChanged();
    }

    private void Update()
    {


    }

    public void SettingChanged()
    {
        if (GameManager.GameManager.Inst.bgmonoff == false)
        {
            audioSources[0].mute = true;
        }

        if (GameManager.GameManager.Inst.seonoff == false)
        {
            audioSources[1].mute = true;
        }

        if (GameManager.GameManager.Inst.bgmonoff)
        {
            audioSources[0].mute = false;
        }

        if (GameManager.GameManager.Inst.seonoff)
        {
            audioSources[1].mute = false;
        }
    }
}
