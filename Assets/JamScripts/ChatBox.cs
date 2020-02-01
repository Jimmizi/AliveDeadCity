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
        AlphaGroup.alpha = 1f;
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

        ButtonAlphaGroup.alpha = 1.0f;

        SetReadyToStart();
    }

    void EndChat()
    {
        mCurrentConversationData = null;
        mCurrentChoiceData = null;
        mProcessingChat = false;
        AlphaGroup.alpha = 0;
        ButtonAlphaGroup.alpha = 0;
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

                return;
            }
            //If we're done with appending text, wait until the player has pressed something to advance text
            else if (!Input.anyKey)
            {
                return;
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

}
