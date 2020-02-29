using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevManager : Singleton<DevManager>
{
    [SerializeField] bool development;
    [SerializeField] bool showDebugText;
    [SerializeField] bool spawnEnemies;
    [SerializeField] bool allowPlayerDeath;
    [SerializeField] [Range(0,2)] float timeScale;
    [SerializeField] Text debugText;

    public bool AllowPlayerDeath { get { return development ? allowPlayerDeath : true ; } }
    public bool SpawnEnemies { get { return development ? spawnEnemies: true ; } }

    int numLines = 50;
    string[] messages;

    protected override void Awake()
    {
        base.Awake();
        messages = new string[numLines];
    }

    private void Update()
    {
        debugText.text = "";
        if (development)
        {
            if (showDebugText)
            {
                SetDebugText();
            }
            TimeManager.Instance.debugTimeScale = timeScale;
        }
    }

    public void Set(int line, string message)
    {
        messages[line] = message;
    }

    private void SetDebugText()
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