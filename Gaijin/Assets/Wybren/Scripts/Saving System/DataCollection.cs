using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;

//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//Just have this script in your scene, it doesnt need to be applied to a gameObject (it cant be). This class just gives the "SaveData" script the information on how to arrange the data.
//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

[XmlRoot("DataCollection")]
public class DataCollection
{
    [XmlArray("AllData")]
    [XmlArrayItem("Data")]
    public List<Data> gameData = new List<Data>();
}
