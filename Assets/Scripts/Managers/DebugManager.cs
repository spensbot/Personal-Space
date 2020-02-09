using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : Singleton<DebugManager>
{
    int numLines = 30;
    string[] messages;
    [SerializeField] bool debug;
    [SerializeField] Text debugText;

    protected override void Awake()
    {
        base.Awake();
        messages = new string[numLines];
    }

    private void LateUpdate()
    {
        SetDebugText();
    }

    public void Set(int line, string message)
    {
        messages[line] = message;
    }

    private void SetDebugText()
    {
        debugText.text = "";
        if (debug)
        {
            for (int i = 0; i < messages.Length; i++)
            {
                if (messages[i] != null)
                {
                    debugText.text += messages[i] + "\n";
                }
                else
                {
                    debugText.text += "\n";
                }
            }
        }
    }
}