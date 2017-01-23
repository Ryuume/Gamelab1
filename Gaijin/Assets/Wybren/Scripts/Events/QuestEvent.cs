using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class QuestEvent : MonoBehaviour
{

    //TODO
    //Send data to Quest when triggered

    public int levelNumber;
    public string NewObjective;
    QuestManager manager;

    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<QuestManager>();
    }

    public void OnTriggerEnter()
    {
        manager.UpdateQuestState(NewObjective, this);
        Destroy(gameObject);
    }

}
