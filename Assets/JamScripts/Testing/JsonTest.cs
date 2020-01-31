using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class JsonTest : MonoBehaviour
{ 
    public TextAsset fileForChoiceData;
    public TextAsset fileForEventData;
    public TextAsset fileForConversationData;

    public bool UpdateDefaultChoiceFormat;
    public bool UpdateDefaultEventFormat;
    public bool UpdateDefaultConversationFormat;

    void GenerateFileFormats()
    {
        ChoiceData choiceData = new ChoiceData();
        choiceData.Choices.Add(new ChoiceData.ChoiceOption("Sample text", "Next file to open"));
        choiceData.Choices.Add(new ChoiceData.ChoiceOption("Sample text 2", "Next file to open 2"));

        ConversationData convData = new ConversationData();
        convData.Lines.Add(new ConversationData.LineData("Speaker Name", "Speech text"));
        convData.Lines.Add(new ConversationData.LineData("Speaker Name 2", "Speech text 2"));

        EventData evtData = new EventData();
        evtData.Type = EventData.EventType.Damage;
        evtData.DamagePartyMemberIndex = 0;
        evtData.DamageAmount = 50;
        evtData.InventoryAction = EventData.InventoryEventType.Add;
        evtData.InventoryItemName = "sword";
        evtData.OpenConversationFile = "file.json";

        string choiceText = JsonUtility.ToJson(choiceData);
        string convText = JsonUtility.ToJson(convData);
        string evtText = JsonUtility.ToJson(evtData);


        if (UpdateDefaultChoiceFormat)
        {
            File.WriteAllText(AssetDatabase.GetAssetPath(fileForChoiceData), choiceText);
            EditorUtility.SetDirty(fileForChoiceData);
        }

        if (UpdateDefaultEventFormat)
        {
            File.WriteAllText(AssetDatabase.GetAssetPath(fileForEventData), evtText);
            EditorUtility.SetDirty(fileForEventData);
        }

        if (UpdateDefaultConversationFormat)
        {
            File.WriteAllText(AssetDatabase.GetAssetPath(fileForConversationData), convText);
            EditorUtility.SetDirty(fileForConversationData);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        EventData evtData = JsonUtility.FromJson<EventData>(fileForEventData.text);

        GenerateFileFormats();




    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
