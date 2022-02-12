using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

/**
Author : Simon Campbell
Category: World Objects - First Person

Notes: None

Changelog:  10/2/22 - Cleaned document up and added comments fitting to CSharp standard.
**/

///<summary>
///<para>A Generic implementation of a World Terminal</para>
///<para>This implentation consists of a World Terminal, and a section to display blocks of text.</para>
///</summary>
public class GenericWorldTerminal : WorldTerminal
{

   /// <summary>The name given to the terminal.</summary>
    [TitleGroup("Generic Terminal Options")]
    public string TerminalLabel;

    ///<summary>Worldspace UI representation of the Terminal Label.</summary>
    [TitleGroup("Generic Terminal Options")]
    public TMP_Text LabelText;

    ///<summary>Reference to a generic World Terminal Text Display.</summary>
    [TitleGroup("Generic Terminal Options")]
    public WorldTerminalTextDisplay TextDisplay;

    ///<summary>Builtin Unity method that gets called each time the script gets enabled.</summary>
    public void OnEnable()
    {
        LabelText.text = TerminalLabel;
    }

    ///<summary>Publishes a new message to the Text Display that replaces any message currently there.</summary>
    ///<param name="message">The message we want to publish.</param>
    public void PublishTextToDisplayDestructive(string message)
    {
        TextDisplay.WriteTextDestructive(message);
    }

    ///<summary>Publishes a new message to the Text Display in addition to any previous messages.</summary>
    ///<param name="message">The message we want to publish.</param>
    public void PublishTextToDisplayAdditive(string message)
    {
        TextDisplay.WriteTextDestructive(message);
    }

}
