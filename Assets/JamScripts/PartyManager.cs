using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class PartyManager : MonoBehaviour
{
    public const int MAX_PARTY_MEMBERS = 3;
    private const int DEFAULT_HEALTH = 100;

    private Renderer[] mPartyMemberMaterials;
    private float[] mMaterialOriginalAlpha;

    public string[] MemberNames = new string[MAX_PARTY_MEMBERS];
    public Color[] MemberColours = new Color[MAX_PARTY_MEMBERS];

    private bool[] mMemberAlive = new bool[MAX_PARTY_MEMBERS] { false, false, false };
    private bool[] mExistanceFading = new bool[MAX_PARTY_MEMBERS] { false, false, false };

    public int[] PartyHealth = new int[MAX_PARTY_MEMBERS] {DEFAULT_HEALTH, 0, 0};

    public void KillParty()
    {
        for (int i = 0; i < MAX_PARTY_MEMBERS; i++)
        {
            PartyHealth[i] = 0;
        }
    }

    void Awake()
    {
        Service.Provide(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        mPartyMemberMaterials = GetComponentsInChildren<Renderer>();

        if (mPartyMemberMaterials.Length > 0)
        {
            mMaterialOriginalAlpha = new float[mPartyMemberMaterials.Length];

            for (var index = 0; index < mPartyMemberMaterials.Length; index++)
            {
                mMaterialOriginalAlpha[index] = mPartyMemberMaterials[index].material.color.a;
            }
        }

        for (int i = 0; i < MAX_PARTY_MEMBERS; i++)
        {
            PartyHealth[i] = mMemberAlive[i] ? 100 : 0;
            SetMemberAlpha(i, mMemberAlive[i] ? 1 : 0);
        }

        SetPartyMembersAlpha(0);
    }

    // Update is called once per frame
    void Update()
    {
        //Handle fading in and out for members
        for (int i = 0; i < MAX_PARTY_MEMBERS; i++)
        {
            if (PartyHealth[i] <= 0)
            {
                if (mMemberAlive[i] && !mExistanceFading[i])
                {
                    SetMemberDeadColor(i);
                    StartCoroutine(FadePartyMember(i, 1.0f, 0.0f, 1.0f));
                    mMemberAlive[i] = false;
                }
            }
            else
            {
                if (!mMemberAlive[i] && !mExistanceFading[i])
                {
                    SetMemberAliveColor(i);
                    StartCoroutine(FadePartyMember(i, 0.0f, 1.0f, 0.5f));
                    mMemberAlive[i] = true;
                }
            }
        }
    }

    public void FadeInParty()
    {
        StartCoroutine(FadePartyMembers(0.0f, 1.0f, 0.5f));
    }

    public void FadeOutParty()
    {
        StartCoroutine(FadePartyMembers(1.0f, 0.0f, 0.5f));
    }

    private const float NON_SPEAKER_ALPHA = 0.6f;

    public void ResetSpeakerMembers()
    {
        for (int i = 0; i < MAX_PARTY_MEMBERS; i++)
        {
            if (mMemberAlive[i])
            {
                //if not alphaed in, do so
                if (Math.Abs(GetMemberAlpha(i, true) - 1.0f) > 0.01f)
                {
                    StartCoroutine(FadePartyMember(i, NON_SPEAKER_ALPHA, 1.0f, 0.5f, true));
                }

            }
        }
    }

    public void SetMemberAsSpeaker(int memberIndex)
    {
        for (int i = 0; i < MAX_PARTY_MEMBERS; i++)
        {
            if (mMemberAlive[i])
            {
                if (memberIndex != i)
                {
                    StartCoroutine(FadePartyMember(i, GetMemberAlpha(i, true), NON_SPEAKER_ALPHA, 0.5f, true));
                }
                else
                {
                    var speakerAlpha = GetMemberAlpha(i, true);

                    //if isn't equal to full alpha, go to full alpha
                    if (Math.Abs(speakerAlpha - 1.0f) > 0.01f)
                    {
                        StartCoroutine(FadePartyMember(i, speakerAlpha, 1.0f, 0.5f, true));
                    }
                }
            }
        }
    }

    float GetMemberAlpha(int memberIndex, bool blackInstead = false)
    {
        Assert.IsTrue(mPartyMemberMaterials.Length == MAX_PARTY_MEMBERS * 2);

        return !blackInstead ? mPartyMemberMaterials[memberIndex * 2].material.color.a
            : mPartyMemberMaterials[memberIndex * 2].material.color.r;
    }

    public bool IsMemberAlive(int member) => mMemberAlive[member];

    private void SetMemberDeadColor(int member)
    {
        Assert.IsTrue(mPartyMemberMaterials.Length == MAX_PARTY_MEMBERS * 2);

        for (int i = member * 2; i < (member * 2) + 2; i++)
        {
            if (mMemberAlive[member])
            {
                mPartyMemberMaterials[i].material.color = Color.red;
            }
        }
    }

    private void SetMemberAliveColor(int member)
    {
        Assert.IsTrue(mPartyMemberMaterials.Length == MAX_PARTY_MEMBERS * 2);

        for (int i = member * 2; i < (member * 2) + 2; i++)
        {
            if (mMemberAlive[member])
            {
                mPartyMemberMaterials[i].material.color = Color.white;
            }
        }
    }

    void SetMemberAlpha(int memberIndex, float alpha, bool setAsBlackColorInstead = false)
    {
        Assert.IsTrue(mPartyMemberMaterials.Length == MAX_PARTY_MEMBERS * 2);

        for (int i = memberIndex * 2; i < (memberIndex * 2) + 2; i++)
        {
            //if (!mMemberAlive[memberIndex])
               // continue;

            var color = mPartyMemberMaterials[i].material.color;

            if (!setAsBlackColorInstead)
            {
                float alphaToUse = alpha;

                if (alphaToUse >= mMaterialOriginalAlpha[i])
                {
                    alphaToUse = mMaterialOriginalAlpha[i];
                }

                mPartyMemberMaterials[i].material.color =
                    new Color(color.r, color.g, color.b, alphaToUse);
            }
            else
            {
                mPartyMemberMaterials[i].material.color =
                    new Color(alpha, alpha, alpha, color.a);
            }
        }
    }

    void SetPartyMembersAlpha(float alpha)
    {
        int iMember = 0;
        for (var index = 0; index < mPartyMemberMaterials.Length; index++)
        {
            if (index > 0 && index % 2 == 0)
            {
                iMember++;
            }

            if (!mMemberAlive[iMember] && Math.Abs(alpha) > 0.01f) 
                continue;

            var member = mPartyMemberMaterials[index];
            float alphaToUse = alpha;

            if (alphaToUse >= mMaterialOriginalAlpha[index])
            {
                alphaToUse = mMaterialOriginalAlpha[index];
            }

            member.material.color = new Color(member.material.color.r,
                member.material.color.g,
                member.material.color.b, alphaToUse);
        }
    }

    private IEnumerator FadePartyMember(int memberIndex, float startAlpha, float endAlpha, float duration, bool setblackInstead = false)
    {
        mExistanceFading[memberIndex] = true;

        float elapsedTime = 0f;
        float totalDuration = duration;

        while (elapsedTime < totalDuration)
        {
            elapsedTime += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / totalDuration);

            SetMemberAlpha(memberIndex, currentAlpha, setblackInstead);
            
            yield return null;
        }

        mExistanceFading[memberIndex] = false;
    }

    private IEnumerator FadePartyMembers(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        float totalDuration = duration;

        while (elapsedTime < totalDuration)
        {
            elapsedTime += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / totalDuration);

            SetPartyMembersAlpha(currentAlpha);

            yield return null;
        }
    }
}
