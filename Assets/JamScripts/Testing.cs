using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public bool SkipTitle;

    /// <summary>
    /// Don't typewriter scroll through text
    /// </summary>
    public bool InstantText;

    /// <summary>
    /// Auto move to the next line of conversation instead of waiting for input. Uses a small timer.
    /// </summary>
    public bool AutomaticEndOfLineSkip;

    /// <summary>
    /// Don't let the text pause when hitting punctuation.
    /// </summary>
    public bool IgnoreTextPauses;

    void Awake()
    {
        Service.Provide(this);
    }

}
