using UnityEngine;
using System.Collections;

public class camFollow : MonoBehaviour {

    public Transform playerPos;
    public int height = 7, distance = 3;
    public float dampTime = 0.2f, maxSpeed = 10f, rotationX = 40f, rotationY = 30f, xOffset = 2f;

    [HideInInspector]
    public Vector3 speed = Vector3.zero;

    void Start ()
    {
        //makes distance a negative, because the camera is behind the player, not in front of it.
        distance = -distance;
        //sets the standard position for the camera.
        transform.position = new Vector3(playerPos.position.x -xOffset, playerPos.position.y + height, playerPos.position.z -distance);
        //rotates the camera to face down towards the player.
        transform.Rotate(rotationX, rotationY, 0);
    }

    void FixedUpdate ()
    {
        //continually updates the current position.
        Vector3 currentPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        //continually updates the needed position, which is that of the player, plus the height of the camera and the distance from the player.
        Vector3 neededPos = new Vector3(playerPos.position.x - xOffset, playerPos.position.y + height, playerPos.position.z + distance);

        //follows the player and smooths the movement.
        transform.position = Vector3.SmoothDamp(currentPos, neededPos, ref speed, dampTime, maxSpeed);
    }
}
