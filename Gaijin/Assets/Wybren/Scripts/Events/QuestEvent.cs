using UnityEngine;
using System.Collections;

public class QuestEvent{

    //TODO
    //Send data to Quest when triggered

    public bool LoadCutscene;
    public string levelName;
    QuestManager manager;

    void Start()
    {
        //manager = GameObject.Find("Manager").GetComponent<QuestManager>();
    }

    public void OnTriggerEnter()
    {
        manager.UpdateQuestState();

        if(LoadCutscene == true)
        {
            //load level "levelName"
        }
    }

}
