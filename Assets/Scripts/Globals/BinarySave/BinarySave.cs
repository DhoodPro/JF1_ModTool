using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class BinarySave
{
    public static List<DataPull> data;

    public static void LoadData()
    {
        //if (data != null) return;

        List <DataPull> dt = new List<DataPull>();
        if (File.Exists(Application.persistentDataPath + "/saveData.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/saveData.gd", FileMode.Open);
            dt = (List<DataPull>)bf.Deserialize(file);
            file.Close();
        }

        data = dt;
    }

    public static DataPull Load(string fileName)
    {
        LoadData();
        
        DataPull dp = null;

        foreach(DataPull d in data)
        {
            if (d.nameOfFile == fileName) dp = d;
        }

        return dp;
    }

    public static void Save(DataPull save)
    {
        LoadData();

        bool createNew = true;

        foreach(DataPull dp in data)
        {
            if (dp.nameOfFile == save.nameOfFile)
            {
                createNew = false;
                dp.valueOfFile = save.valueOfFile;
            }
        }

        if (createNew) data.Add(save);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/saveData.gd");
        bf.Serialize(file, data);
        file.Close();
    }

    public static void SaveNew(DataPull save)
    {
        LoadData();

        foreach (DataPull dp in data)
        {
            if (dp.nameOfFile == save.nameOfFile)
            {
                save.nameOfFile += "1";
            }
        }

        data.Add(save);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/saveData.gd");
        bf.Serialize(file, data);
        file.Close();

        foreach(DataPull dp in data)
        {
            Debug.Log("this read something!");
        }
    }

    public static void Delete(DataPull save)
    {
        LoadData();
        List<DataPull> newData = new List<DataPull>();

        foreach (DataPull dp in data)
        {
            if (dp.nameOfFile != save.nameOfFile)
            {
                newData.Add(dp);
            }
        }

        data = newData;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/saveData.gd");
        bf.Serialize(file, data);
        file.Close();
    }

}

[System.Serializable]
public class DataPull
{
    public string nameOfFile;
    public object valueOfFile;
}

[System.Serializable]
public class PointSave
{
    public float[] pos;
}

[System.Serializable]
public class RollerCoasterSave
{
    public string RollerCoaster;
    public PointSave[] Points;
    public bool Close;
    public int Carts;
}
