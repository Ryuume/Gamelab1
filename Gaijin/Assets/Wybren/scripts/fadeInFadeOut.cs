using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FadeInFadeOut
{
    public List<Transform> hiddenObjects;
    public float fadeAmount, fadeTime;
    public bool isChild;

    public IEnumerator Fadeout(Transform currentHit)
    {
        //Selects the material from the object that needs to be faded out.
        Material material = currentHit.GetComponent<Renderer>().material;

        //Constructs a "changeMaterial" class. This class chances the render mode.
        ChangeMaterial change = new ChangeMaterial();
        //Calls "ChangeToFade" in the constructed class. This void changes the material's render mode from "Opaque" to "Fade"
        change.ChangeToFade(material);

        //Doubles the fade time if the object is a child.
        if (isChild == true)
        {
            fadeTime = fadeTime * 2;
        }
        
        //Slowly fades the object away.
        for (float f = 1; f > 0; f -= fadeAmount)
        {
            //Breaks the forloop if the object is no longer needed to fade, but still fading.
            if (!hiddenObjects.Contains(currentHit))
            {
                break;
            }

            //Pauzes the script for [fadeTime] (standard 0.05 seconds).
            yield return new WaitForSeconds(fadeTime);
            //grabs the color from the object's material.
            Color color = material.color;
            //substracts the [fadeAmount] (standard 0.1) from the color's alpha.
            color.a -= fadeAmount;
            //adapts color of the object to the new color.
            material.color = color;
        }

        //if the object is still required to fade away, this if statement is activated.
        if (hiddenObjects.Contains(currentHit))
        {
            //disables the objects renderer, so no more processing power (for rendering) is used by the faded object.
            currentHit.GetComponent<Renderer>().enabled = false;

            //Resets the materials alpha to 0, to be sure the object is completely faded.
            Color resetColor = material.color;
            resetColor.a = 0;
            material.color = resetColor;
        }
    }

    public IEnumerator Fadein(Transform wasHit)
    {
        //Re-enables the renderer on the object.
        wasHit.GetComponent<Renderer>().enabled = true;

        //Selects the material from the object that needs to be faded in.
        Material material = wasHit.GetComponent<Renderer>().material;

        //Slowly fades the object in.
        for (float f = 0; f < 1; f += fadeAmount)
        {
            //grabs the color from the object's material.
            Color color = material.color;
            //adds the [fadeAmount] (standard 0.1) from the color's alpha.
            color.a += fadeAmount;
            //adapts color of the object to the new color.
            material.color = color;

            //Pauzes the script for [fadeTime] (standard 0.05 seconds).
            yield return new WaitForSeconds(fadeTime);
        }

        //Resets the materials alpha to 0, to be sure the object is completely visible.
        Color resetColor = material.color;
        resetColor.a = 1;
        material.color = resetColor;

        //Constructs a "changeMaterial" class. This class chances the render mode.
        ChangeMaterial change = new ChangeMaterial();
        //Calls "ChangeToFade" in the constructed class. This void changes the material's render mode from "Fade" to "Opaque" 
        change.ChangeToOpaque(material);
    }
}
