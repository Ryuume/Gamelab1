using UnityEngine;
using System.Collections;

public class SpawnShuriken : MonoBehaviour {

    public PlayerController player;

    void Spawn()
    {
        player.Shuriken();
    }

    void Reparent()
    {
        player.katana.SetActive(true);
    }

    void Disable()
    {
        player.kusarigama.SetActive(false);
        player.katana.SetActive(true);
    }

    void Smoke()
    {
        player.SmokeBomb();
    }
}
