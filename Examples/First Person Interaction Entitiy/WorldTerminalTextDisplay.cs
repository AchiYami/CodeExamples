using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/**
Author : Simon Campbell
Category: World Objects - First Person - Worldspace UI

Notes: None

Changelog:  10/2/22 - Cleaned document up and added comments fitting to CSharp standard.
**/

///<summary>
///A class that handles publishing text to a display on WorldTerminal objects.
///</summary>
public class WorldTerminalTextDisplay : MonoBehaviour
{

    ///<summary>The entire string to write</summary>
    private string TextToWrite;

    ///<summary>Worldspace Text UI to display the message</summary>
    private TMP_Text Text;

    ///<summary>Writes text that replaces the current text.</summary>
    ///<param name= "textToWrite" >The message to write</param>
    public void WriteTextDestructive(string textToWrite)
    {
        //Assigns a cached reference to the message
        TextToWrite = textToWrite;

        //Ensures we have a valid reference to the worldspace text.
        if (Text == null)
        {
            Text = GetComponent<TMP_Text>();
        }


        //Stops all coroutines, ensuring we don't have multiple messages trying to 
        //print at the same time.
        StopAllCoroutines();

        //Starts the coroutine.
        StartCoroutine(WriteTextRoutine());
    }

    ///<summary>Writes text that is in addition to the current text.</summary>
    ///<param name= "textToWrite">The message to write</param>
    public void WriteTextAdditive(string textToWrite)
    {
        //Assigns a cached reference to the message
        TextToWrite = textToWrite;

        //Ensures we have a valid reference to the worldspace text.
        if (Text == null)
        {
            Text = GetComponent<TMP_Text>();
        }

        //Stops all coroutines, ensuring we don't have multiple messages trying to 
        //print at the same time.
        StopAllCoroutines();

        //Starts the coroutine
        StartCoroutine(WriteTextRoutineAdditive());
    }


    ///<summary>
    ///Asynchronous coroutine that handles writing text in a type-writer fashion in a destructive manner.
    ///</summary>
    private IEnumerator WriteTextRoutine()
    {
        //Initialize
        Text.text = "";
        Text.maxVisibleCharacters = 0;
        Text.text = TextToWrite;

        //Print a character one at a time, typewriter style
        while (Text.maxVisibleCharacters < TextToWrite.Length)
        {
            Text.maxVisibleCharacters++;
            yield return null;
        }
    }

    ///<summary>
    ///Asynchronous coroutine that handles writing text in a type-writer fashion in an additive manner
    ///</summary>
    private IEnumerator WriteTextRoutineAdditive()
    {
        //Initialize the maximum visible characters to the current length of 
        //the worldspace text.
        Text.maxVisibleCharacters = Text.text.Length;

        //Take a new line if text is already present.
        if (Text.text.Length > 0)
        {
            Text.text += "\n";
        }

        //Concatenate the new message 
        Text.text += TextToWrite;

        //Type the new message, one character at a time.
        while (Text.maxVisibleCharacters < TextToWrite.Length)
        {
            Text.maxVisibleCharacters++;
            yield return null;
        }
    }

}
