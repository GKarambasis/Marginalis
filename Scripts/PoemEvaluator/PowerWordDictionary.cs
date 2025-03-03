using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

public class PowerWordDictionary : MonoBehaviour
{
    private Dictionary<string, string[]> pwDictionary = new Dictionary<string, string[]>();

    //Singleton
    public static PowerWordDictionary Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps the object alive between scenes
        }
        else
        {
            Destroy(gameObject); // Ensures only one instance exists
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        LoadCMUDictionary("PowerWords");
    }

    private void LoadCMUDictionary(string fileName)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(fileName);
        if (textAsset == null)
        {
            Debug.LogError("Power Word Dictionary file not found");
            return;
        }

        string[] lines = textAsset.text.Split('\n');

        foreach (string line in lines)
        {
            //skips comments and empty lines
            if (string.IsNullOrEmpty(line) || line.StartsWith(";;;")) continue;

            var parts = line.Split(new[] { "," }, System.StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 1)
            {
                //store phonemes
                pwDictionary[parts[0].ToLower()] = parts.Skip(1).ToArray();
            }
        }
        Debug.Log("Power Word Dictionary loaded with " + pwDictionary.Count + " power word lists.");
    }

    public bool WordExists(string word)
    {
        if (pwDictionary.Values.Any(arr => arr.Contains(word.ToLower())))
        {
            Debug.Log("Power Word exists in dictionary: " + word);
        }

        return pwDictionary.Values.Any(arr => arr.Contains(word.ToLower()));
    }

    public string GetWordKey(string word)
    {
        // Ensure the word is in lowercase to match case-insensitive check
        word = word.ToLower();

        // Check if the word exists in the dictionary and return the corresponding value
        foreach (var key in pwDictionary.Keys)
        {
            if (pwDictionary[key].Contains(word))
            {
                return key; // return the value (the list or array associated with the key)
            }
        }

        return null;
    }
}
