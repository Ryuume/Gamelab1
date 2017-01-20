using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Death : MonoBehaviour {

    public GameObject prefab;

	// Use this for initialization
    void SetDeath()
    {
        prefab.GetComponent<Rigidbody>().isKinematic = false;
        prefab.GetComponent<CapsuleCollider>().radius = .2f;
        prefab.GetComponent<CapsuleCollider>().height = .1f;
    }

    void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Delete()
    {
        prefab.GetComponent<FieldOfView>().draw = false;
        prefab.GetComponent<AreaOfView>().draw = false;
        prefab.GetComponent<AreaOfView>().turn = false;
        prefab.GetComponent<AIManager>().enabled = false;
        prefab.GetComponent<NavMeshAgent>().enabled = false;
    }
}
