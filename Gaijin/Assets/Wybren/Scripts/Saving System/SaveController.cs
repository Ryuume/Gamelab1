using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Xml;
using System.IO;

//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//Drag this script onto an empty GameObject called "GameController".
//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//This script takes care of loading and re-routing data to the correct script.
public class SaveController : MonoBehaviour
{ 

    public Button saveButton;
    public Button loadButton;
    public List<string> values = new List<string>(), names = new List<string>();
    public List<GameObject> saveObjects = new List<GameObject>();
    public string objName = "/DataCollection/AllData/Data[@Name='", saveName, path;
    public bool testDelete;

    public void Save()
    {
        SaveData.Save(path, SaveData.dataContainer);
    }

    public void Load()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(path);

        XmlNode root = doc.SelectSingleNode("/DataCollection/AllData");
        

        int childNodeCount = root.ChildNodes.Count;
        print(childNodeCount + " Childnodes");

        for (int index = 0; index < childNodeCount; index++)
        {
            string searchName = objName + names[index] + "']";
            print(searchName + "SearchName");
            XmlNodeList xnList = doc.SelectNodes(searchName);
            foreach (XmlNode xn in xnList)
            {
                if (xn.HasChildNodes)
                {
                    foreach (XmlNode item in xn.ChildNodes)
                    {
                        values.Add(item.InnerText);
                    }
                    saveObjects[index].SendMessage("GetData");
                    values.Clear();
                }
            }

        }
    }
}


