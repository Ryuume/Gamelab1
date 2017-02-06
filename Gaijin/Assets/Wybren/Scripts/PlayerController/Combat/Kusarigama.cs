using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Kusarigama : MonoBehaviour
{
    public float minDamage, maxDamage;
    public List<AudioClip> hitClips = new List<AudioClip>();

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Enemy")
        {
            float damage = Random.Range(minDamage, maxDamage);
            print("Hit for " + damage + " damage");
            col.SendMessageUpwards("Hit", damage);
            PlayHit();
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
