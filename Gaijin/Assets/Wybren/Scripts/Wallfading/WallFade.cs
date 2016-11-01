using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WallFade : MonoBehaviour
{
    [HideInInspector]
    public List<Transform> hiddenObjects = new List<Transform>();

    public Transform player, mainCamera;
    public LayerMask mask;
    public float fadeTime = 0.05f, fadeAmount = 0.1f;

    public bool includeChilds = true;
	

	void Update ()
    {
        //Calculates the direction the raycast has to be fired in.
        Vector3 direction = player.position - mainCamera.position;

        //Calculates the distance from the camera to the player
        float distance = direction.magnitude;

        //Shoots a raycast that hits everything in its path (as long as it's got the correct mask), not just the first thing it finds, and puts it all into an array.
        RaycastHit[] hits = Physics.RaycastAll(mainCamera.position, direction, distance, mask);

        //Cycles through the entire array.
        for (int i = 0; i < hits.Length; i++)
        {
            //Grabs the hit that the forloop is currently at.
            Transform currentHit = hits[i].transform;

            //Checks if the selected hit from the array is not already in the "hiddenObjects" list.
            if (!hiddenObjects.Contains(currentHit))
            {
                //adds the hit to the list.
                hiddenObjects.Add(currentHit);

                //Starts the class FadeInFadeOut (this class makes the object fade away), and gives it all neccesary information.
                FadeInFadeOut fade = new FadeInFadeOut();
                fade.fadeAmount = fadeAmount;
                fade.fadeTime = fadeTime;
                fade.hiddenObjects = hiddenObjects;

                //Starts the "Fadeout" Coroutine in the "FadeInFadeOut" class. (Coroutine is neccesary for delaying the fade (WaitForSeconds))
                //Supplies the Coroutine with the object that needs to be faded out.
                StartCoroutine(fade.Fadeout(currentHit));

                //Does the same thing as above for every child if includeChilds is on, as long as they are marked for fading. (see tag).
                if (includeChilds == true)
                {
                    foreach (Transform child in currentHit)
                    {
                        if (child.gameObject.tag == "OccluderChild")
                        {
                            hiddenObjects.Add(child);
                            FadeInFadeOut childFade = new FadeInFadeOut();
                            childFade.isChild = true;
                            childFade.fadeAmount = fadeAmount;
                            childFade.fadeTime = fadeTime;
                            childFade.hiddenObjects = hiddenObjects;
                            StartCoroutine(fade.Fadeout(child));
                        }
                    }
                }
            }
        }

        //Constantly cycles through all objects in "HiddenObjects"
        for (int i = 0; i < hiddenObjects.Count; i++)
        {
            bool isHit = false;

            //Cycles through all hits in the "Hits" array.
            for(int j = 0; j < hits.Length; j++)
            {
                //If the hiddenObject is still in the "Hits" array, nothing happens and the next object in "HiddenObjects is selected.
                if (hiddenObjects[i] == hits[j].transform)
                {
                    isHit = true;
                    break;
                }
                //Because childs of hit objects are never stored in the "Hits array" I check here if the child object that is not in the "Hits" array
                //has a parent. If it has a parent, I check if that parent also posesses the layermask necessary to be faded. If so, this object is a child of a faded object, and can stay, otherwise,
                //it is faded in again.
                else if(hiddenObjects[i].tag == "OccluderChild")
                {
                    isHit = true;
                    break;
                }
            }

            //If the hiddenObject is no longer in the "Hits" array (raycast removes them automatically when they are no longer hit):
            if (isHit == false)
            {
                //Selects the currently selected object.
                Transform wasHit = hiddenObjects[i];

                //Removes the object from the "HiddenObjects" list.
                hiddenObjects.RemoveAt(i);

                //Just as when an item is first found, the "FadeInFadeOut" class is constructed and gets the neccesary information supplied.
                FadeInFadeOut fade = new FadeInFadeOut();
                fade.fadeAmount = fadeAmount;
                fade.fadeTime = fadeTime;
                fade.hiddenObjects = hiddenObjects;

                //Starts another Coroutine in "FadeInFadeOut", this time the "Fadein" coroutine. This coroutine reverses the effect from the "Fadeout" coroutine.
                //Supplies the coroutine with the object that needs to be faded in.
                StartCoroutine(fade.Fadein(wasHit));
                i--;

                //Does the same as above if "includeChilds" is selected for each child the object has, as long as these childs are marked for fading..
                if (includeChilds == true)
                {
                    foreach (Transform child in wasHit)
                    {
                        if (child.gameObject.tag == "OccluderChild")
                        {
                            hiddenObjects.Remove(child);
                            FadeInFadeOut childFade = new FadeInFadeOut();
                            childFade.fadeAmount = fadeAmount;
                            childFade.fadeTime = fadeTime;
                            childFade.hiddenObjects = hiddenObjects;
                            StartCoroutine(fade.Fadein(child));
                        }
                    }
                }
            }
        }
	}

    public void OnCollisionEnter(Collision col)
    {
        print(col.gameObject);
        if (col.gameObject.layer == mask)
        {
            //Grabs the hit that the forloop is currently at.
            Transform currentHit = col.transform;

            //Checks if the selected hit from the array is not already in the "hiddenObjects" list.
            if (!hiddenObjects.Contains(currentHit))
            {
                //adds the hit to the list.
                hiddenObjects.Add(currentHit);

                //Starts the class FadeInFadeOut (this class makes the object fade away), and gives it all neccesary information.
                FadeInFadeOut fade = new FadeInFadeOut();
                fade.fadeAmount = fadeAmount;
                fade.fadeTime = fadeTime;
                fade.hiddenObjects = hiddenObjects;

                //Starts the "Fadeout" Coroutine in the "FadeInFadeOut" class. (Coroutine is neccesary for delaying the fade (WaitForSeconds))
                //Supplies the Coroutine with the object that needs to be faded out.
                StartCoroutine(fade.Fadeout(currentHit));

                //Does the same thing as above for every child if includeChilds is on, as long as they are marked for fading. (see tag).
                if (includeChilds == true)
                {
                    foreach (Transform child in currentHit)
                    {
                        if (child.gameObject.tag == "OccluderChild")
                        {
                            hiddenObjects.Add(child);
                            FadeInFadeOut childFade = new FadeInFadeOut();
                            childFade.isChild = true;
                            childFade.fadeAmount = fadeAmount;
                            childFade.fadeTime = fadeTime;
                            childFade.hiddenObjects = hiddenObjects;
                            StartCoroutine(fade.Fadeout(child));
                        }
                    }
                }
            }
        }
    }
}
