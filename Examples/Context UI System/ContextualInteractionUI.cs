using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/**
Author : Simon Campbell
Category: World Objects - First Person - UI

Notes: None

Changelog:  10/2/22 - Cleaned document up and added comments fitting to CSharp standard.
**/


///<summary>
///Class that handles the root and control of the overall Context Sensitive UI,
///this class does not handle the functionality of the configurations. See the individual configuration files for that.
///</summary>
public class ContextualInteractionUI : MonoBehaviour
{

    [SerializeField]
    ///<summary>Builtin Unity CanvasGroup, to allow for Alpha control.</summary>
    private CanvasGroup canvasGroup;

    [Header("Configuration Transforms")]
    [SerializeField]
    ///<summary>The Context Configuration when looking at an Interactable Entity.</summary>
    private InteractionContextMenu InteractionConfig;

    [SerializeField]
    ///<summary>The Context Configuration when looking at a Lootable Entity.</summary>
    private LootContextMenu LootConfig;

    [SerializeField]
    ///<summary>The GameObject the Interaction Configuration Layout is attached to.</summary>
    private GameObject InteractionConfigGameObject;

    [SerializeField]
    ///<summary>The GameObject the Loot Configuration Layout is attached to.</summary>
    private GameObject LootConfigGameObject;

    ///<summary>The FirstPersonInteractable that is current the target of the Context UI.</summary>
    private FirstPersonInteractable _contextEntity;


    ///<summary>Hides all UI Elements of the Context UI smoothly.</summary>
    public void Hide()
    {

        //Ensures we don;t try to show and hide the UI at the same time.
        StopAllCoroutines();


        if (isActiveAndEnabled)
            StartCoroutine(FadeOut());
    }

    ///<summary>Shows all UI Elements of the Context UI smoothly.</summary>
    public void Show()
    {
        //Ensures we don't try to show and hide the UI at the same time.
        StopAllCoroutines();

        if (isActiveAndEnabled)
            StartCoroutine(FadeIn());
    }


    ///<summary>Enables one child transform, and disabled all siblings. 
    ///Used to ensure we only show one context UI configuation at a time.
    ///</summary>
    ///<param name="transformToEnable">The transform we wish to enable exclusively.</param>
    private void SetActiveExclusive(Transform transformToEnable)
    {
        //Go through each child transform - only enable the one that matches
        //our target, disable all others.
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i) != transformToEnable)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    ///<summary>Enables the loot menu, and populates it.</summary>
    ///<param name="lootItems">The list of items to show.</param>
    ///<param name="contextEntity">The object that is the target of the Context UI.</param>
    public void ShowLootMenu(List<ItemData> lootItems, FirstPersonInteractable contextEntity)
    {
        //Set the context
        _contextEntity = contextEntity;

        //Exclusively enable and show the Loot Configuration
        SetActiveExclusive(LootConfig.transform);
        Show();

        //Populate the list
        LootConfig.PopulateList(lootItems);

        //Set the binding control context.
        LootConfig.SetContext(contextEntity.PrimaryActionBinding, contextEntity.SecondaryActionBinding, contextEntity.ExitActionBinding);
    }

    ///<summary>Public Getter for the Loot Context Menu</summary>
    public LootContextMenu GetLootContextMenu()
    {
        return LootConfig;
    }


    ///<summary>
    ///Shows the Interaction Configuration context UI.
    ///</summary>
    ///<param name="contextEntity">The target of the context UI.</param>
    public void ShowContextMenu(FirstPersonInteractable contextEntity)
    {
        //Set the context.
        _contextEntity = contextEntity;

        //Exclusively enable and show the Interaction Configuration
        SetActiveExclusive(InteractionConfig.transform);
        Show();

        //Set the binding control context..
        InteractionConfig.SetContext(contextEntity.PrimaryActionBinding, contextEntity.SecondaryActionBinding, contextEntity.ExitActionBinding);
    }


    ///<summary>Asynchronous Coroutine to smoothly fade in a canvasGroup.</summary>
    private IEnumerator FadeIn()
    {
        while (canvasGroup.alpha < 1.0f)
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1.0f, Time.deltaTime);
            yield return null;
        }
    }

    ///<summary>Asynchronous Coroutine to smoothly fade out a canvasGroup.</summary>
    private IEnumerator FadeOut()
    {
        while (canvasGroup.alpha > 0.0f)
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0, Time.deltaTime * 2);
            yield return null;
        }
    }
}
