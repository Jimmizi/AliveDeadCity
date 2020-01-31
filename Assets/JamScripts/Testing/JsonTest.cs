using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class JsonTest : MonoBehaviour
{
    class TestClass
    {
        [Serializable]
        public struct ConvoLine
        {
            public ConvoLine(string speakerStr, string speechStr)
            {
                Speaker = speakerStr;
                Speech = speechStr;
            }

            public string Speaker;
            public string Speech;
        }

        public List<ConvoLine> Lines = new List<ConvoLine>();

    }

    public TextAsset jsonFile;

    // Start is called before the first frame update
    void Start()
    {

        TestClass myObject = new TestClass();
        myObject.Lines.Add(new TestClass.ConvoLine("Test Speaker", "Test Speech"));
        myObject.Lines.Add(new TestClass.ConvoLine("Test Speaker the second", "Test Speech the second"));

        string jsonTest = JsonUtility.ToJson(myObject);
        TestClass newObject = JsonUtility.FromJson<TestClass>(jsonFile.text);

        //File.WriteAllText(AssetDatabase.GetAssetPath(jsonFile), jsonTest);
        //EditorUtility.SetDirty(jsonFile);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
