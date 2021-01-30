using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataBase
{
    void LoadData();
    void SaveData();
    IMyData GetData(string ID);
}

public interface IMyData
{
    string dataID { get; set; }
    string myName { get; set; }
}

public class DataBase<T> where T : ScriptableObject, IMyData
{
    protected string m_Path;
    protected string m_FileName;
    protected Dictionary<string, IMyData> m_dicData = new Dictionary<string, IMyData>();

    public DataBase(string path) 
    { 
        m_Path = path; 
    }

    public IMyData GetData(string ID)
    {
        if (m_dicData.Count == 0)
            LoadData();

        IMyData data;
        if (m_dicData.TryGetValue(ID, out data))
        {
            return data;
        }

        return null;
    }

    public void LoadData()
    {
        T[] data = ResourceManager.Instance.LoadAll<T>(m_Path);

        if (data == null)
        {
            return;
        }

        for (int i = 0; i < data.Length; i++)
        {
            m_dicData.Add(data[i].dataID, data[i]);
        }
    }
}

