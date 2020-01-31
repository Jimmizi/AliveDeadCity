using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameStage
{
    public enum State
    {
        Init,
        Update,
        Exit
    }

    private State mCurrentState;
    public State CurrentState => mCurrentState;

    public abstract void Init();
    public abstract void Update();
    public abstract void Exit();
}

/// <summary>
/// Used to direct the flow of the game and how it plays out
/// </summary>
public class GameFlow : MonoBehaviour
{
    private List<GameStage> mGameStages = new List<GameStage>();
    private short mCurrentStage = 0;


    void Awake()
    {
        Service.Provide(this);
    }

    void Start()
    {
        
    }

    /// <summary>
    /// Process the current stage, returning if the stage should go to the next one
    /// </summary>
    /// <returns>True if done</returns>
    bool ProcessCurrentStage()
    {
        switch (mGameStages[mCurrentStage].CurrentState)
        {
            case GameStage.State.Init:

                mGameStages[mCurrentStage].Init();

                break;
            case GameStage.State.Update:

                mGameStages[mCurrentStage].Update();

                break;

            case GameStage.State.Exit:

                mGameStages[mCurrentStage].Exit();
                return true;

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return false;
    }

    void Update()
    {
        
        if (mCurrentStage < mGameStages.Count)
        {
            if (ProcessCurrentStage())
            {
                mCurrentStage++;
            }

        }
        else
        {
            //Game is done
        }
    }
}
