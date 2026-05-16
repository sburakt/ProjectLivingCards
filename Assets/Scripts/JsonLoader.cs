using TCG.Model.Cards;
using UnityEngine;

public static class JsonLoader
{
    public static void Load()
    {
        TextAsset[] jsonFiles = Resources.LoadAll<TextAsset>("StaticCards");
        
        if (jsonFiles.Length != 0)
        {
            foreach (var jsonFile in jsonFiles)
            {
                StaticCard staticCard = JsonUtility.FromJson<StaticCard>(jsonFile.text);
                StaticCardLibrary.AddCard(staticCard);
            }
        }
        else
        {
            Debug.LogError("no JSON file found!");
        }
    }
}