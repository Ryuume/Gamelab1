using UnityEngine;
using UnityEngine.SceneManagement;

public class StartAndQuit : MonoBehaviour {

    public int sceneNum;

	public void StartGame()
    {
        SceneManager.LoadScene(sceneNum);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
