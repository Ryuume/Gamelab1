using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dagger : MonoBehaviour
{
    public bool doDamage, isEnemy;
    [HideInInspector]
    public float minDamage, maxDamage;

    public List<AudioClip> hitClips = new List<AudioClip>();

    void DoDamage()
    {
        doDamage = true;
    }
    void NoDamage()
    {
        doDamage = false;
    }

    public void RecieveData(Vector2 damages)
    {
        minDamage = damages.x;
        maxDamage = damages.y;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "Player")
        {
            if (doDamage == true)
            {
                PlayHit();
                float damage = Random.Range(minDamage, maxDamage);
                print("Hit for " + damage + " damage");
                col.SendMessageUpwards("Hit", damage);
                doDamage = false;
            }
        }
    }

    public void PlayHit()
    {
        int randomNum = Random.Range(0, hitClips.Count - 1);
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = hitClips[randomNum];
        audioSource.Play();
    }
}
