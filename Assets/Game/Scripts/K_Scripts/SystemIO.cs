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

        using (StreamWriter writer = new StreamWriter("Assets\\Game\\Data\\" + fileName + ".json"))
        {
            writer.Write(json);
        }
    }

    public static DataList<T> LoadFile<T>(string fileName)
    {
        string json = string.Empty;

        try
        {
            using (StreamReader reader = new StreamReader("Assets\\Game\\Data\\" + fileName + ".json"))
            {
                json = reader.ReadToEnd();
            }
            return JsonUtility.FromJson<DataList<T>>(json);
        }
        catch (FileNotFoundException e)
        {
            Debug.Log("File not found!");
        }

        return null;
    }
}
