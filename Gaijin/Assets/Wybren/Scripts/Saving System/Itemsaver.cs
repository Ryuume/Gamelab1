using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Reflection;

//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//Only drag this script onto gameObjects that only need their position saved. When otherwise, copy this entire script into the script of the gameObject you want to save. then add all the data you need to save to the "Data" class, and change Store/GetData to store and apply all the information in the "Data" class.
//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

public class Itemsaver : MonoBehaviour
{

    public Data data = new Data();

    public float health = 100;

    public List<string> values = new List<string>();

    public SaveController other;



    public void StoreData()
    {
        data.name = gameObject.name;
        Vector3 pos = transform.position;
        data.posX = pos.x;
        data.posY = pos.y;
        data.posZ = pos.z;
    }

    public void GetData()
    {
        print(5);
        values = other.values;

        print("Zou aangepast moeten worden");

        data.posX = float.Parse(values[0]);
        data.posY = float.Parse(values[1]);
        data.posZ = float.Parse(values[2]);
        values.Clear();

        transform.position = new Vector3(data.posX, data.posY, data.posZ);
    }

    void OnEnable()
    {
        other = (SaveController)GameObject.Find("GameController").GetComponent(typeof(SaveController));
        SaveData.OnBeforeSave += delegate { StoreData(); };
        SaveData.OnBeforeSave += delegate { SaveData.AddData(data); };
        other.saveObjects.Add(gameObject);
        other.names.Add(gameObject.name);
    }

    void OnDisable()
    {
        SaveData.OnBeforeSave -= delegate { StoreData(); };
        SaveData.OnBeforeSave -= delegate { SaveData.AddData(data); };
    }
}

public class Data
{
    [XmlAttribute("Name")]
    public string name;

    [XmlElement("PosX")]
    public float posX;

    [XmlElement("PosY")]
    public float posY;

    [XmlElement("PosZ")]
    public float posZ;
}