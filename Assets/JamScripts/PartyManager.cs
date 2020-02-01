using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public GameObject PartyLayoutObject;
    private Renderer[] mPartyMemberMaterials;

    void Awake()
    {
        Service.Provide(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        mPartyMemberMaterials = PartyLayoutObject.GetComponentsInChildren<Renderer>();
        SetPartyMembersAlpha(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeInParty()
    {
        StartCoroutine(FadePartyMembers(0.0f, 1.0f, 0.5f));
    }

    public void FadeOutParty()
    {
        StartCoroutine(FadePartyMembers(1.0f, 0.0f, 0.5f));
    }

    void SetPartyMembersAlpha(float alpha)
    {
        foreach (var member in mPartyMemberMaterials)
        {
            member.material.color = new Color(member.material.color.r, 
                                              member.material.color.g, 
                                              member.material.color.b, alpha);
        }
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
