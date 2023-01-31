using UnityEngine;

public static class SaveManager<T>
{
    public static void Save(string _key, T _oject)
    {
        string jsonFile = JsonUtility.ToJson(_oject);
        PlayerPrefs.SetString(_key, jsonFile);
    }

    public static void Load(string _key, T _object)
    {
        if (PlayerPrefs.HasKey(_key))
        {
            string jsonFile = PlayerPrefs.GetString(_key);
            JsonUtility.FromJsonOverwrite(jsonFile, _object);
        }
    }

}