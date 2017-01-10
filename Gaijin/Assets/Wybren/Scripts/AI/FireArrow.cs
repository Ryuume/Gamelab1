using UnityEngine;
using System.Collections;

public class FireArrow : MonoBehaviour
{
    public AIManager manager;

    public void Fire()
    {
        manager.eUpdate.aUpdate.Fire();
    }
}
