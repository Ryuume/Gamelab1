using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    //TODO
    //1. Get mouse position in worldspace. +++
    //2. Calculate at what angle the mouse is compared to the player.
    //3. Have four states of movement, which call different animations (movement stays the same).
    //4. Switch through states based on 2 and call necessary animations.
    //5. Call all necessary animations.
    //6. Have health, and such as variables.
    //7. Register hits, death and such and play animations based on this.

    RaycastHit hit;

    public LayerMask targetMask;

    Vector3 mousePos;

    public void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, targetMask))
        {
            Vector3 targetPos = hit.point;
            targetPos.y = transform.position.y;
            if (Vector3.Distance(transform.position, targetPos) > .5f)
            {
                //Quaternion targetRotation = Quaternion.LookRotation(targetPos - transform.position);
                //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 4 * Time.deltaTime);
                float angle = Vector3.Angle(transform.position, targetPos);
                print(angle);
            }
        }
    }
}
