using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class Katana : MonoBehaviour {


    public bool doDamage, isEnemy;
    public List<AudioClip> swooshClips = new List<AudioClip>(), hitClips = new List<AudioClip>();
    [HideInInspector]
    public float minDamage, maxDamage;

    public void RecieveData(Vector2 damages)
    {
        minDamage = damages.x;
        maxDamage = damages.y;
        isEnemy = true;
    }

    public void PlaySound()
    {
        int randomNum = Random.Range(0, swooshClips.Count - 1);
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = swooshClips[randomNum];
        audioSource.Play();
    }

    void OnTriggerEnter(Collider col)
    {
        if (isEnemy == false)
        {
            if (col.gameObject.tag == "Enemy")
            {
                if (doDamage == true)
                {
                    float damage = Random.Range(minDamage, maxDamage);
                    print("Hit for " + damage + " damage");
                    PlayHit();
                    col.SendMessageUpwards("Hit", damage);
                }
            }
        }else
        {
            if (col.gameObject.tag == "Player")
            {
                if (doDamage == true)
                {
                    float damage = Random.Range(minDamage, maxDamage);
                    print("Enemy hit for " + damage + " damage");
                    doDamage = false;
                    PlayHit();
                    col.SendMessageUpwards("Hit", damage);
                }
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
