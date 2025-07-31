using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[System.Serializable]
public class DataList<T>
{
    public List<T> list;
}

public static class SystemIO
{
    public static void SaveFile<T>(DataList<T> dataList, string fileName)
    {
        string json = JsonUtility.ToJson(dataList, true);
        
        using (StreamWriter writer = new StreamWriter(Application.dataPath + "/Resources/" + fileName + ".json"))
        {
            writer.Write(json);
        }
    }

    public static DataList<T> LoadFile<T>(string fileName)
    {
        try
        {
            TextAsset json = Resources.Load<TextAsset>(fileName);
            return JsonUtility.FromJson<DataList<T>>(json.text);
        }
        catch (FileNotFoundException e)
        {
            Debug.Log("File not found!");
        }

        return null;
    }
}
