using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationData
{
    [Serializable]
    public struct LineData
    {
        public LineData(string speakerStr, string speechStr)
        {
            Speaker = speakerStr;
            Speech = speechStr;
        }

        /// <summary>
        /// Speaker name to display at the top of the dialogue box or bark text.
        /// </summary>
        public string Speaker;

        /// <summary>
        /// What is the speaker saying.
        /// </summary>
        public string Speech;
    }

    /// <summary>
    /// Lines in this section of dialogue
    /// </summary>
    public List<LineData> Lines = new List<LineData>();

    /// <summary>
    /// After the last line has finished, what conversation should we run next.
    /// This has priority of loading if filled in.
    /// </summary>
    public string ConversationFileToRunNext;

    /// <summary>
    /// After the last line has finished, what event should we run next.
    /// This has second priority of loading if filled in.
    /// </summary>
    public string EventFileToRunNext;
}
