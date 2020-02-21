using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class SaveState
{
    public int highScore = 0;
    public float totalPlayTime = 0f;

    //Settings
    public bool isVibration = true;
    public float volumeMusic = 1f;
    public float volumeSfx = 1f;

}

public class SaveManager
{
    private static readonly string fileName = "Dont Cheat Dude.json";

    public static void Save(SaveState saveState)
    {
        string jsonString = JsonConvert.SerializeObject(saveState, Formatting.Indented);
        File.WriteAllText(SavePath(), jsonString);
    }

    public static SaveState Load()
    {
        if (File.Exists(SavePath()))
        {
            string jsonString = File.ReadAllText(SavePath());
            return JsonConvert.DeserializeObject<SaveState>(jsonString);
        }
        else
        {
            return new SaveState();
        }
    }

    private static string SavePath()
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }
}