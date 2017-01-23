using UnityEngine;
using System.Collections;

public class Events : MonoBehaviour {
    
    //TODO
    //Have options for every single possible event
    //Events possible:
    //-Checkpoint (saving)
    //-Ambush (spawn enemies)
    //-MoveCam (move camera somewhere)
    //-Cutscene (play cutscene)
    //-Trap (Spikes, door closing, ect)
    //-Sound effect (Play music, sound, ect)

    public enum eventType { Ambush, MoveCam, Cutscene, SoundEffect};

    public eventType type = new eventType();


    void OnTriggerEnter()
    {

    }

}
