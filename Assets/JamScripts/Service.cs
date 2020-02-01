using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

public static class Service
{
    public static UiManager UI()
    {
        return mUiManagerPtr;
    }

    public static GameFlow Flow()
    {
        return mGameFlowPtr;
    }

    public static SoundManager Audio()
    {
        return mSoundManagerPtr;
    }

    public static ChatBox Chat()
    {
        return mChatBoxPtr;
    }

    public static JsonDataExecuter Execution()
    {
        return mJsonExecuterPtr;
    }

    public static Testing Test()
    {
#if !DEBUG
        //When release, return a blank testing config so nothing will be enabled
        return new Testing();
#endif
        return mTesterPtr;
    }

    public static void Provide(UiManager ui)
    {
        Assert.IsNull(mUiManagerPtr);
        mUiManagerPtr = ui;
    }
    public static void Provide(GameFlow flow)
    {
        Assert.IsNull(mGameFlowPtr);
        mGameFlowPtr = flow;
    }

    public static void Provide(SoundManager audio)
    {
        Assert.IsNull(mSoundManagerPtr);
        mSoundManagerPtr = audio;
    }

    public static void Provide(ChatBox chatB)
    {
        Assert.IsNull(mChatBoxPtr);
        mChatBoxPtr = chatB;
    }

    public static void Provide(JsonDataExecuter executer)
    {
        Assert.IsNull(mJsonExecuterPtr);
        mJsonExecuterPtr = executer;
    }

    public static void Provide(Testing test)
    {
        Assert.IsNull(mTesterPtr);
        mTesterPtr = test;
    }

    private static UiManager mUiManagerPtr;
    private static GameFlow mGameFlowPtr;
    private static SoundManager mSoundManagerPtr;
    private static ChatBox mChatBoxPtr;
    private static JsonDataExecuter mJsonExecuterPtr;
    private static Testing mTesterPtr;
}
