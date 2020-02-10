using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public struct SaveData
{
    public int highScore;
}

public class SaveManager : MonoBehaviour
{
    string saveFileName = "save0";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveGame(SaveData saveData)
    {

    }

    //public SaveData LoadGame()
    //{

    //}
}
