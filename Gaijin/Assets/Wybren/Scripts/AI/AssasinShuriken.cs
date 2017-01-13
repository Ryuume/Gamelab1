using UnityEngine;
using System.Collections;

public class AssasinShuriken : MonoBehaviour {

    [HideInInspector]
    public Vector2 damages;

    public void RecieveData(Vector2 _damages)
    {
        damages = _damages;
    }

    public void SpawnShuriken(GameObject shuriken, Vector3 target)
    {
        GameObject projectile = (GameObject)Instantiate(shuriken, transform.position, Quaternion.identity);
        projectile.SendMessageUpwards("SetDamage", damages);
        projectile.SendMessageUpwards("RecieveData", target);
    }
}
