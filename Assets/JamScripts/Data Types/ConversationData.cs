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

    public List<LineData> Lines = new List<LineData>();
}
