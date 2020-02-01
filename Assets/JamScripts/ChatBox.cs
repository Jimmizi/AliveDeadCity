using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ChatBox : MonoBehaviour
{
    #region Public settings

    public Text SpeakerTextComponent;
    public Text SpeechTextComponent;

    public Button[] ChoiceButtons = new Button[5];

    public CanvasGroup AlphaGroup;
    public CanvasGroup ButtonAlphaGroup;

    /// <summary>
    /// How long to wait between appending characters to the speech box
    /// </summary>
    public float TimeBetweenCharacters = 0.1f;

    #endregion

    private float mConvEndOfLineSkipTimer = 0.0f;

    private ConversationData mConversationToDisplay;

    private ConversationData mCurrentConversationData = null;
    private ChoiceData mCurrentChoiceData = null;

    /// <summary>
    /// Is the chat box currently displaying text of some sort
    /// </summary>
    private bool mProcessingChat;

    /// <summary>
    /// If true, this is a conversation, if false this is a choice
    /// </summary>
    private bool mIsConversation;

    public bool Processing => mProcessingChat;
    public bool HasValidChat => mCurrentConversationData != null || mCurrentChoiceData != null;

    #region Conversation Vars

    /// <summary>
    /// Current line of the conversation
    /// </summary>
    private int mCurrentConvLine;

    /// <summary>
    /// Next character to append in the conversation line
    /// </summary>
    private int mCurrentLineChar;

    private float mCharacterTimer;

    #endregion

    #region Choice Vars

    private int mChoicePicked = -1;

    #endregion

    /// <summary>
    /// Reset the conversation text box, and set the speaker for the current line about to be spoken
    /// </summary>
    private void ResetConversationToCurrentLine()
    {
        SpeakerTextComponent.text = mCurrentConversationData.Lines[mCurrentConvLine].Speaker;
        SpeechTextComponent.text = "";
        mCurrentLineChar = 0;
    }

    /// <summary>
    /// Choice / Conversation init
    /// </summary>
    private void SetReadyToStart()
    {
        mCurrentConvLine = 0;
        mCurrentLineChar = 0;
        mProcessingChat = true;
    }

    /// <summary>
    /// Prepare the chat box for a conversation
    /// </summary>
    /// <param name="conv"></param>
    public void StartChat(ConversationData conv)
    {
        mIsConversation = true;
        mCurrentConversationData = conv;
        mCurrentChoiceData = null;

        SetButtons(false);
        StartCoroutine(FadeChatGroup(0.0f, 1.0f, 0.5f));

        SetReadyToStart();
        ResetConversationToCurrentLine();
    }

    /// <summary>
    /// Prepare the chat box for a choice option
    /// </summary>
    /// <param name="choice"></param>
    public void StartChat(ChoiceData choice)
    {
        mIsConversation = false;
        mCurrentChoiceData = choice;
        mCurrentConversationData = null;

        SpeechTextComponent.text = "";
        SpeakerTextComponent.text = "";
        mChoicePicked = -1;

        SetButtons(false, true);

        for (var i = 0; i < choice.Choices.Count; i++)
        {
            var textComp = ChoiceButtons[i].GetComponentInChildren<Text>();
            if (textComp != null)
            {
                textComp.text = String.Format("[{0}] " + choice.Choices[i].Text, i);
            }
        }

        StartCoroutine(FadeChoiceGroup(0.0f, 1.0f, 0.5f));

        SetReadyToStart();
    }

    public void EndCleanup()
    {
        mCurrentConversationData = null;
        mCurrentChoiceData = null;
    }

    public void ChoicePressed(int choice)
    {
        Debug.Log("Choice pressed: " + choice);
        if (mChoicePicked == -1 && choice < mCurrentChoiceData.Choices.Count)
        {
            Debug.Log("Choice locked in");
            mChoicePicked = choice;
            SetButtons(false);
        }
    }

    void EndChat()
    {
        mProcessingChat = false;

        if (mIsConversation)
        {
            StartCoroutine(FadeChatGroup(1.0f, 0.0f, 0.5f));
        }
        else
        {
            StartCoroutine(FadeChoiceGroup(1.0f, 0.0f, 0.5f));
        }
    }

    private void ProcessConversation()
    {
        //if we have a valid conversation line, proceed with displaying it
        if (mCurrentConvLine >= 0)
        {
            //Iterate and append text until we've added it all
            if (SpeechTextComponent.text.Length < mCurrentConversationData.Lines[mCurrentConvLine].Speech.Length)
            {
                //If delay time is reached, append, if not add to the timer
                if (mCharacterTimer >= TimeBetweenCharacters)
                {
                    mCharacterTimer = 0.0f;

                    //TODO Special timing characters

                    SpeechTextComponent.text += mCurrentConversationData.Lines[mCurrentConvLine].Speech
                        .Substring(mCurrentLineChar++, 1);
                }
                else
                {
                    mCharacterTimer += Time.deltaTime;
                }

                if (Service.Test().InstantText)
                {
                    SpeechTextComponent.text = mCurrentConversationData.Lines[mCurrentConvLine].Speech;
                }

                return;
            }
            //If we're done with appending text, wait until the player has pressed something to advance text
            else if (!Input.anyKey)
            {
                if (!Service.Test().AutomaticEndOfLineSkip)
                {
                    return;
                }
                else
                {
                    if (mConvEndOfLineSkipTimer < 0.75f)
                    {
                        mConvEndOfLineSkipTimer += Time.deltaTime;
                        return;
                    }

                    mConvEndOfLineSkipTimer = 0;
                }
            }
            
            if (mCurrentConvLine < mCurrentConversationData.Lines.Count - 1)
            {
                //Move to the next line, and reset the current line
                mCurrentConvLine++;
                
                ResetConversationToCurrentLine();
            }
            else
            {
                //Once we're out of lines, set to -1 to begin processing end of conversation events
                mCurrentConvLine = -1;
            }
            
        }
        //Otherwise we are done with this conversation
        else
        {
            EndChat();
        }
    }

    void ProcessChoice()
    {
        if (mChoicePicked == -1)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (mCurrentChoiceData.Choices.Count >= 1)
                {
                    Debug.Log("Choice key 1 pressed.");
                    mChoicePicked = 0;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (mCurrentChoiceData.Choices.Count >= 2)
                {
                    Debug.Log("Choice key 2 pressed.");
                    mChoicePicked = 1;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (mCurrentChoiceData.Choices.Count >= 3)
                {
                    Debug.Log("Choice key 3 pressed.");
                    mChoicePicked = 2;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                if (mCurrentChoiceData.Choices.Count >= 4)
                {
                    Debug.Log("Choice key 4 pressed.");
                    mChoicePicked = 3;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                if (mCurrentChoiceData.Choices.Count >= 5)
                {
                    Debug.Log("Choice key 5 pressed.");
                    mChoicePicked = 4;
                }
            }
        }
        else
        {
            mCurrentChoiceData.ChoiceTaken = mChoicePicked;
            EndChat();
        }
    }

    void Awake()
    {
        Service.Provide(this);
        AlphaGroup.alpha = 0;
        ButtonAlphaGroup.alpha = 0;
    }

    void Start()
    {

    }
    
    void Update()
    {
        if (mProcessingChat)
        {
            if (mIsConversation)
            {
                ProcessConversation();
            }
            else
            {
                ProcessChoice();
            }
        }
    }

    private void SetButtons(bool interactable, bool resetText = false)
    {
        int currentChoices = 0;

        if (mCurrentChoiceData?.Choices != null)
        {
            currentChoices = mCurrentChoiceData.Choices.Count;
        }

        for (var i = 0; i < ChoiceButtons.Length; i++)
        {
            var button = ChoiceButtons[i];
            button.interactable = interactable && i < currentChoices;

            //If this is going to be interactive
            button.gameObject.SetActive(i < currentChoices);
           
            if (resetText)
            {
                var textComp = button.GetComponentInChildren<Text>();
                if (textComp != null)
                {
                    textComp.text = "-";
                }
            }
        }
    }

    public IEnumerator FadeChatGroup(float startAlpha, float endAlpha, float duration)
    {
        if (Math.Abs(AlphaGroup.alpha - endAlpha) > 0.01f)
        {
            float elapsedTime = 0f;
            float totalDuration = duration;

            while (elapsedTime < totalDuration)
            {
                elapsedTime += Time.deltaTime;
                float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / totalDuration);

                AlphaGroup.alpha = currentAlpha;

                yield return null;
            }
        }
    }
    public IEnumerator FadeChoiceGroup(float startAlpha, float endAlpha, float duration)
    {
        if (Math.Abs(ButtonAlphaGroup.alpha - endAlpha) > 0.01f)
        {
            float elapsedTime = 0f;
            float totalDuration = duration;

            while (elapsedTime < totalDuration)
            {
                elapsedTime += Time.deltaTime;
                float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / totalDuration);

                ButtonAlphaGroup.alpha = currentAlpha;

                yield return null;
            }

            SetButtons(true);
        }
    }
}
