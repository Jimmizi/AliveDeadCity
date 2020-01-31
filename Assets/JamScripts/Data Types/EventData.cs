using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventData
{
    public enum EventType
    {
        Damage,
        Conversation,
        InventoryChange
    }

    public enum InventoryEventType
    {
        Add,
        Remove
    }

    /// <summary>
    /// What type of event is this, what will it do?
    /// </summary>
    public EventType Type;

    #region Damage Event

    /// <summary>
    /// The index of the party member to damage for a damage event type. If left as -1, will damage a random member.
    /// </summary>
    public int DamagePartyMemberIndex = -1;

    /// <summary>
    /// Amount to default the party member for.
    /// </summary>
    public int DamageAmount = 0;

    #endregion

    #region Conversation Event

    /// <summary>
    /// The conversation file to open next
    /// </summary>
    public string OpenConversationFile = "";

    #endregion

    #region Inventory Event

    /// <summary>
    /// The type of action happening to the party inventory
    /// </summary>
    public InventoryEventType InventoryAction;

    /// <summary>
    /// The name of the item being modified
    /// </summary>
    public string InventoryItemName = "";

    #endregion
}
