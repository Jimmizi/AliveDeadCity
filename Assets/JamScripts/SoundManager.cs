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
    private FMOD.Studio.EventInstance typingLoopInstance;

    [FMODUnity.EventRef] 
    public string MenuMusicEvent;

    [FMODUnity.EventRef]
    public string GameLoopMusicEvent;

    [FMODUnity.EventRef]
    public string TypingLoopEvent;

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

    public void PlayTypingLoop()
    {
        typingLoopInstance = FMODUnity.RuntimeManager.CreateInstance(TypingLoopEvent);
        typingLoopInstance.start();
    }

    public void StopTypingLoop()
    {
        typingLoopInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
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

        if (typingLoopInstance.isValid())
        {
            typingLoopInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            typingLoopInstance.release();
            typingLoopInstance.clearHandle();
        }
    }


}
