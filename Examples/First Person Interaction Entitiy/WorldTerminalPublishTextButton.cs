using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Author : Simon Campbell
Category: World Objects - First Person

Notes: None

Changelog:  10/2/22 - Cleaned document up and added comments fitting to CSharp standard.
**/


///<summary>
///Class that handles publishing text to a terminal display upon being pressed.
///</summary>
public class WorldTerminalPublishTextButton : MonoBehaviour, IWorldTerminalButton
{

    ///The text that we want to publish to the display
    [TextArea]
    public string TextToPopulate;

    ///<summary>>Called upon the button being pressed. Will publish a message to the display in a destructive manner.</summary>
    ///<param name="worldTerminal">The terminal we want to publish a message to</param>
    public void Activate(GenericWorldTerminal worldTerminal)
    {
        worldTerminal.TextDisplay.WriteTextDestructive(TextToPopulate);
    }
}
