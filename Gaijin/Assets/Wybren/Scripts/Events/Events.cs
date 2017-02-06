using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[RequireComponent(typeof (AudioSource))]
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

    public enum eventType { Ambush, MoveCam, LoadLevel, SoundEffect};

    public eventType type = new eventType();

    [Header("Ambush")]
    public Transform player;
    public List<GameObject> spawnEnemies = new List<GameObject>();
    public Transform spawnPosition;
    public float spawnRadius;

    [Header("MoveCam")]
    public Transform cam, newCamPos;
    public float camPosDuration;

    [Header("LoadLevel")]
    public int levelNum;
    public GameObject loadScreen;

    [Header("SoundEffect")]
    public AudioClip soundEffect;

    bool isActive = true;

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player" && isActive == true)
        {
            switch(type)
            {
                case eventType.Ambush:
                    {
                        Ambush();
                        break;
                    }
                case eventType.MoveCam:
                    {
                        MoveCam();
                        break;
                    }
                case eventType.LoadLevel:
                    {
                        LoadLevel();
                        break;
                    }
                case eventType.SoundEffect:
                    {
                        SoundEffect();
                        break;
                    }
            }
        }
    }

    void Ambush()
    {
        isActive = false;
        List<AIManager> _AIList = new List<AIManager>();

        for(int i = 0; i < spawnEnemies.Count; i++)
        {

            Vector3 point = (Random.insideUnitSphere * spawnRadius) + spawnPosition.position;
            GameObject enemy = (GameObject)Instantiate(spawnEnemies[i], point, Quaternion.identity);
            AIManager _AI = enemy.GetComponent<AIManager>();
            _AI.SetData();
            _AI.suspicious = true;
            _AI.eUpdate.sTarget = player.position;
            _AI.eUpdate.unit.waiting = true;
            _AI.eUpdate.unit.target = null;
            _AI.eUpdate.unit.randomTarget = Vector3.zero;
            _AIList.Add(_AI);
        }

        GameObject.FindGameObjectWithTag("GameManager").GetComponent<CombatManager>().SpottedPlayer(_AIList, player);
    }

    void MoveCam()
    {
        //not using this for now.
    }

    void LoadLevel()
    {
        loadScreen.SetActive(true);
        SceneManager.LoadScene(levelNum);
        isActive = false;
    }

    void SoundEffect()
    {
        AudioSource audioPlayer = GetComponent<AudioSource>();
        audioPlayer.clip = soundEffect;
        audioPlayer.Play();
        isActive = false;
    }
}
