using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CMUDictionary : MonoBehaviour
{
    private Dictionary<string, string[]> cmuDictionary = new Dictionary<string, string[]>();

    //Singleton
    public static CMUDictionary Instance { get; private set; }
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
        LoadCMUDictionary("cmudict");
    }

    private void LoadCMUDictionary(string fileName)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(fileName);
        if(textAsset == null)
        {
            Debug.LogError("CMU Dictionary file not found");
            return;
        }

        string[] lines = textAsset.text.Split('\n');

        foreach (string line in lines)
        {
            //skips comments and empty lines
            if (string.IsNullOrEmpty(line) || line.StartsWith(";;;")) continue; 

            var parts = line.Split(new[] {" "}, System.StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 1) 
            {
                //store phonemes
                cmuDictionary[parts[0].ToLower()] = parts.Skip(1).ToArray();
            }
        }
        Debug.Log("CMU Dictionary loaded with " + cmuDictionary.Count + " words.");
    }
    
    //Checks if the word exists
    public bool WordExists(string word)
    {
        if(!cmuDictionary.ContainsKey(word.ToLower()))
        {
            Debug.LogWarning("Word does not exist in dictionary: " + word);
        }

        return cmuDictionary.ContainsKey(word.ToLower());
    }

    //gets the phonemes of the word
    public string[] GetPhonemes(string word)
    {
        string[] phonemes = new string[0];

        if (WordExists(word)) { phonemes = cmuDictionary[word.ToLower()]; }
        else { Debug.LogWarning("No Phonemes for " + word + " Found"); }

        return phonemes;
    }

    /// <summary>
    /// Checks if two words can rhyme
    /// </summary>
    /// <param name="word1"></param>
    /// <param name="word2"></param>
    /// <returns></returns>
    public bool DoWordsRhyme(string word1, string word2)
    {
        string[] phonemes1 = GetPhonemes(word1);
        string[] phonemes2 = GetPhonemes(word2);

        if (phonemes1 == null || phonemes2 == null) return false; // If word not found

        //Debug.Log("Last Phoenemes: " + phonemes1.Last() + phonemes2.Last());

        return phonemes1.Length > 1 && phonemes2.Length > 1 &&
               phonemes1.Last() == phonemes2.Last(); // Compare last phoneme
    }
}
