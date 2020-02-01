using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum CurrentTextFormat
{
    Event,
    Conversation,
    Choice
}

/// <summary>
/// Used to direct the flow of the game and how it plays out
/// </summary>
public class GameFlow : MonoBehaviour
{
    

    /// <summary>
    /// Event File to start off the game
    /// </summary>
    public TextAsset StartEvent;

    private JsonDataExecuter mExecuter = new JsonDataExecuter();

    private CurrentTextFormat mStartFormat;
    private string mStartJsonText;

    void Awake()
    {
        Service.Provide(this);
        Service.Provide(mExecuter);
    }
    
    void Start()
    {
        Assert.IsNotNull(StartEvent);

        mStartFormat = CurrentTextFormat.Event;
        mStartJsonText = StartEvent.text;
    }

    void Update()
    {
        if (!Service.UI().InGame)
        {
            return;
        }

        if (!mExecuter.Processing)
        {
            mExecuter.GiveJsonToExecute(mStartFormat, mStartJsonText);
        }
        else
        {
            if (mExecuter.Update())
            {

            }
        }
        
        
    }
}
