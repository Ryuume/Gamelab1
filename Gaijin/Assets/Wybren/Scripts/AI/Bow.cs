using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bow : MonoBehaviour
{

    Vector2 damages;
    public List<AudioClip> shootClips = new List<AudioClip>();

	public void RecieveData(Vector2 _damages)
    {
        damages = _damages;
    }

    public void FireArrow(GameObject projectile, Quaternion targetRotation, float theta, float projectileSpeed)
    {
        GameObject arrow = (GameObject)MonoBehaviour.Instantiate(projectile, transform.position, Quaternion.identity);

        arrow.SendMessageUpwards("RecieveData",damages);
        PlayShoot();
        arrow.transform.rotation = targetRotation;
        arrow.transform.Rotate(theta, 0, 0);
        arrow.GetComponent<Rigidbody>().velocity = arrow.transform.forward * projectileSpeed;
    }

    public void PlayShoot()
    {
        int randomNum = Random.Range(0, shootClips.Count - 1);
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = shootClips[randomNum];
        audioSource.Play();
    }
}
