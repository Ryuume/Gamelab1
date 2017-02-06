using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IngameMenu : MonoBehaviour
{
    bool inMenu;
    public GameObject menu;

    void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        inMenu = !inMenu;

        menu.SetActive(inMenu);
        if(inMenu == false)
        {
            Time.timeScale = 1;
            print("Time = 1");
        }
        if(inMenu == true)
        {
            Time.timeScale = 0;
            print("Time = 0");
        }
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
