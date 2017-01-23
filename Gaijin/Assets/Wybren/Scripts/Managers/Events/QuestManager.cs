using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    //TODO
    //OPTION 1: Keep realtime database of every event not yet triggered (all of them when you load a fresh save)
    //Spawn in every event from the database. (or remove every event not in the database)
    //OPTION 2: Keep realtime database of every event that has been triggered, and remove these when done loading the game.
    //Keep track of position in Storyline, which main quests have been completed, and which have not been completed.

    //Update questbook in the menu.
    //Save data of position in story.

    public List<QuestEvent> allObjectives = new List<QuestEvent>();
    public GameObject objectiveScroll, scrollText, objectiveText;
    public float visibleTime;

    float timer;
    bool active;
    

    public void Update()
    {
        if(active == true)
        {
            timer += Time.deltaTime;
            if(timer > visibleTime)
            {
                timer = 0;
                active = false;
                objectiveScroll.SetActive(false);
            }
        }
    }

    public void UpdateQuestState(string newObjective, QuestEvent triggeredEvent)
    {
        active = true;
        objectiveScroll.SetActive(true);
        scrollText.GetComponent<Text>().text = newObjective;
        objectiveText.GetComponent<Text>().text = newObjective;
        triggeredEvent.enabled = false;
        allObjectives.Remove(triggeredEvent);
        if (allObjectives.Count > 0)
        {
            allObjectives[0].enabled = true;
        }
    }
}
