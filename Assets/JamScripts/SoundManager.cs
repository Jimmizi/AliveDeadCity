using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMOD.Studio;
using FMODUnity;

public class SoundManager : MonoBehaviour
{
    

    private FMOD.Studio.EventInstance menuMusicInstance;
    private FMOD.Studio.EventInstance gameLoopInstance;

    [FMODUnity.EventRef] 
    public string MenuMusicEvent;

    [FMODUnity.EventRef]
    public string GameLoopMusicEvent;

    void Awake()
    {
        Service.Provide(this);

    }

    public void PlayMenuMusic()
    {
        menuMusicInstance = FMODUnity.RuntimeManager.CreateInstance(MenuMusicEvent);
        menuMusicInstance.start();
    }

    public void StopMenuMusic()
    {
        menuMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void PlayGameLoopMusic()
    {
        gameLoopInstance = FMODUnity.RuntimeManager.CreateInstance(GameLoopMusicEvent);
        gameLoopInstance.start();
    }

    public void StopGameLoopMusic()
    {
        gameLoopInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    void OnApplicationQuit()
    {
        if (menuMusicInstance.isValid())
        {
            menuMusicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            menuMusicInstance.release();
            menuMusicInstance.clearHandle();
        }

        if (gameLoopInstance.isValid())
        {
            gameLoopInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            gameLoopInstance.release();
            gameLoopInstance.clearHandle();
        }
    }


}
