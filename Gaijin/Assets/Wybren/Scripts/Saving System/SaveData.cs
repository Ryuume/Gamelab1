using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//Just have this script in your scene, it doesnt need to be applied to a gameObject (it cant be). This class takes care of saving data, and loading(ish).
//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

public class SaveData
{
    public static DataCollection dataContainer = new DataCollection();

    public delegate void SerializeAction();
    //public static event SerializeAction OnBeforeLoaded;
    //public static event SerializeAction OnLoaded;
    public static event SerializeAction OnBeforeSave;



    public static void Load()
    {
        GameObject go = GameObject.Find("GameController");
        SaveController other = (SaveController)go.GetComponent(typeof(SaveController));
        other.Load();
    }

    public static void Save(string path, DataCollection data)
    {
        Debug.Log("SAVE");
        OnBeforeSave();

        SaveActors(path, data);

        ClearData();
    }

    public static void AddData(Data data)
    {
        dataContainer.gameData.Add(data);
    }

    public static void ClearData()
    {
        dataContainer.gameData.Clear();
    }

    private static void SaveActors(string path, DataCollection data)
    {
        XmlDocument xmlDoc = new XmlDocument();
        if (File.Exists(path))
        {
            xmlDoc.Load(path);

            XmlElement elmRoot = xmlDoc.DocumentElement;

            elmRoot.RemoveAll(); // remove all inside the transforms node.
        }

        XmlSerializer serializer = new XmlSerializer(typeof(DataCollection));

        FileStream stream = new FileStream(path, FileMode.Truncate);

        serializer.Serialize(stream, data);

        stream.Close();
    }

}