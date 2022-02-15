using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
Author : Simon Campbell
Category: Systems - Save/Load

Notes: None

Changelog:  15/2/22 - Cleaned document up and added comments fitting to CSharp standard.
**/

///<summary>Interface to denote a class as being able to be Saved/Loaded, and provides method signatures to do such.</summary>
public interface ISavableData 
{
   
    void SaveData();

    void LoadData();

}
