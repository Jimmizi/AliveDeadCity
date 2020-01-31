using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class SoundManager : MonoBehaviour
{
    [Serializable]
    public struct SoundInstance
    {
        [FMODUnity.EventRef]
        public string EventName;

        [HideInInspector]
        public FMOD.Studio.EventInstance instance;

        public void Create()
        {
            instance = FMODUnity.RuntimeManager.CreateInstance(EventName);
        }

        public void Start()
        {
            instance.start();
        }

        public void Stop(bool fade = true, bool kill = false)
        {
            instance.stop(fade ? FMOD.Studio.STOP_MODE.ALLOWFADEOUT : FMOD.Studio.STOP_MODE.IMMEDIATE);

            if (kill)
            {
                instance.release();
            }
        }
    }

    public List<SoundInstance> mSoundInstances = new List<SoundInstance>();


    void OnApplicationQuit()
    {
        foreach (var sound in mSoundInstances)
        {
            sound.Stop(false, true);

        }
    }


}
