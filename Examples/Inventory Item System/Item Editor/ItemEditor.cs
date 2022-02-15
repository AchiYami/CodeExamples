using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


/**
Author : Simon Campbell
Category: Editor - Items/Inventory - Item

Notes: None

Changelog:  15/2/22 - Cleaned document up and added comments fitting to CSharp standard.
**/

///<summary>Editor Only Class to allow for the creation, editing and deletion of Items.</summary>
public class ItemEditor : EditorWindow
{
    //Database Varaibles
    [SerializeField]
    ///<summary>Item Database that stores all created Items.</summary>
    public ItemDatabase itemDatabase;

    ///<summary>The item we are currently editing.</summary>
    public ItemData itemToEdit;

    ///<summary>The item that is currently selected.</summary>
    public ItemData _selectedItem;

    ///<summary>List of Foldouts to display.</summary>
    List<AssetFoldoutData> AssetList;

    ///<summary>Reference to our position within scrolling the item list.</summary>
    Vector2 _assetListScrollPos;

    ///<summary>GUI Style for an Item Asset button.</summary>
    private GUIStyle ItemAssetButtonStyle;

    ///<summary>GUI Style for an Item asset that has been selected.</summary>
    private GUIStyle ItemAssetButtonSelectedStyle;

    ///<summary>Cached Item name</summary>
    private string _cachedItemName;

    ///<summary>Control over whether we are editing an existing item, or creating a new item.</summary>
    private enum DisplayState
    {
        EDIT,
        CREATE
    }

    ///<summary>Control over whether we are editing an existing item, or creating a new item.</summary>
    private DisplayState _state;

    ///<summary>Is the current item "dirty"? Does it have changes that need saved?</summary>
    private bool _isItemDirty = false;

    ///<summary>The amount of diferent item types there are present.</summary> 
    private int itemTypeCount = System.Enum.GetNames(typeof(ItemType)).Length;


    [MenuItem("Directive/Database/Item Database %#q")]
    ///<summary>Method to use unitys built in commands to open an editor window.</summary>
    public static void OpenWindow()
    {
        //Creates the editor window
        ItemEditor window = EditorWindow.GetWindow<ItemEditor>();

        //Sets the window size.
        window.minSize = new Vector2(1000, 700);
        window.maxSize = new Vector2(1000, 700);

        //Shows the window.
        window.Show();
        window.Initialize();
    }

    void OnEnable()
    {
        Initialize();
    }

    ///<summary>Method to use unitys built in commands to open an editor window with a specific item in focus.</summary>
    public static void OpenWindow(ItemData item)
    {
        //Creates the editor window.
        ItemEditor window = EditorWindow.GetWindow<ItemEditor>();

        //Sets the window size.
        window.minSize = new Vector2(800, 450);

        //Shows the window and focuses the item.
        window.Show();
        window.itemToEdit = item;
    }

    ///<summary>Initializes the editor and it's styles.</summary>
    public void Initialize()
    {
        //Creates the GUI styles for the editor.
        InitializeStyles();

        //Creates a new asset list, and it's foldout data.
        if (AssetList == null)
        {
            AssetList = new List<AssetFoldoutData>();

            for (int i = 0; i < itemTypeCount; i++)
            {
                AssetList.Add(new AssetFoldoutData(false, (ItemType)i));
            }
        }
    }

    ///<summary>Reloads the asset foldout list.</summary>
    public void Reload()
    {
        AssetList = new List<AssetFoldoutData>();

        for (int i = 0; i < itemTypeCount; i++)
        {
            AssetList.Add(new AssetFoldoutData(false, (ItemType)i));
        }
    }

    ///<summary>Creates the GUI Styles for the Item Editor.</summary>
    void InitializeStyles()
    {
        //Creates the style fo an Item Asset button.
        if (ItemAssetButtonStyle == null)
        {
            ItemAssetButtonStyle = new GUIStyle();
            ItemAssetButtonStyle.padding = new RectOffset(30, 0, 0, 0);
            ItemAssetButtonStyle.normal.textColor = Color.white;
            ItemAssetButtonStyle.active.textColor = Color.white;
            ItemAssetButtonStyle.stretchHeight = false;
            ItemAssetButtonStyle.fixedHeight = 25;
        }

        //Creates the style for an Item Asset button that is selected.
        if (ItemAssetButtonSelectedStyle == null)
        {
            ItemAssetButtonSelectedStyle = new GUIStyle();
            ItemAssetButtonSelectedStyle.padding = new RectOffset(300, 0, 0, 0);
            ItemAssetButtonSelectedStyle.normal.textColor = Color.white;
            ItemAssetButtonSelectedStyle.active.textColor = Color.white;
            ItemAssetButtonSelectedStyle.normal.background = MakeTex(1, 1, Color.blue);
            ItemAssetButtonSelectedStyle.stretchHeight = false;
            ItemAssetButtonSelectedStyle.fixedHeight = 25;
        }

    }

    ///<summary>Builtin Unity method for drawing editor GUI.</summary>
    void OnGUI()
    {
        //Begin the Layout
        EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));

        EditorGUILayout.BeginVertical(GUILayout.Width(300));

        //Draw the Asset Navigation bar.
        DisplayAssetBar();

        //Draw the Option Button panel below the bar.
        DisplayOptionButtons();

        EditorGUILayout.EndVertical();

        //Assigned the item to edit to the selected item
        if (_selectedItem != null)
        {
            itemToEdit = _selectedItem;
        }

        //If we have created a new item, show the creation panel, otherwise show the edit panel.
        if (_state == DisplayState.CREATE)
        {
            DisplayCreateItemArea();
        }
        else
        {
            DisplayMainArea();
        }

        EditorGUILayout.EndHorizontal();
    }

    /// ----------- ----------- ----------- ----------- GUI DISPLAY

    ///<summary>Method to display the Asset Item navigation bar.</summary>
    void DisplayAssetBar()
    {
        //Create Layout
        EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
        EditorGUILayout.LabelField("Asset Database", GUILayout.ExpandWidth(true));

        //Handle scrolling.
        _assetListScrollPos = EditorGUILayout.BeginScrollView(_assetListScrollPos);

        //Display each item type foldout.
        foreach (AssetFoldoutData asset in AssetList)
        {
            DisplayAssetBarFoldout(asset);
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.EndVertical();
    }

    ///<summary>Display the option buttons for Creating, Removing, Editing and Cloning.</summary>
    void DisplayOptionButtons()
    {
        //If we're editing, display New/Clone/Delete/Save buttons.
        if (_state == DisplayState.EDIT)
        {
            EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(false));
            EditorGUILayout.BeginHorizontal("box");

            DisplayNewButton();
            DisplayCloneButton();

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal("box");

            DisplayDeleteButton();
            DisplaySaveButton();

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
        else//Else display the Cancel button.
        {
            DisplayCancelButton();
        }
    }

    ///<summary>Display a Foldout button in the Asset navigation bar.</summary>
    ///<param name="foldoutData">The foldout data to draw.</param>
    void DisplayAssetBarFoldout(AssetFoldoutData foldoutData)
    {

        //Craete the layout.
        EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(false), GUILayout.Width(300));

        //If the asset needs saved, show a symbol * to signify this, otherwise just use the item's type.
        if (foldoutData.isDirty)
        {
            foldoutData.expanded = EditorGUILayout.Foldout(foldoutData.expanded, ItemData.GetItemTypeName(foldoutData.type) + "*");
        }
        else
        {
            foldoutData.expanded = EditorGUILayout.Foldout(foldoutData.expanded, ItemData.GetItemTypeName(foldoutData.type));
        }

        //If the foldout is expanded.
        if (foldoutData.expanded)
        {
            //Show each item in it's list.
            for (int i = 0; i < foldoutData.itemList.Count; i++)
            {
                //Create a button and give it the name of the item.
                GUIContent buttonContent = new GUIContent();
                buttonContent.text = foldoutData.itemList[i].ItemName;

                //Show a thumbnail of the item (if it has an image), if not, create a placeholder.
                if (foldoutData.itemList[i].ItemImage == null)
                {
                    buttonContent.image = MakeTex(25, 25, Color.white);
                }
                else
                {
                    buttonContent.image = foldoutData.itemList[i].ItemImage.texture;
                }

                //If we have selected the item, show it as a selected button item style.
                if (_selectedItem == foldoutData.itemList[i])
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    GUILayout.Button(buttonContent, ItemAssetButtonSelectedStyle, GUILayout.ExpandWidth(true));
                    GUILayout.EndHorizontal();
                }
                else //if not, display the regular button style.
                {

                    GUILayout.BeginHorizontal();
                    GUILayout.Space(10);

                    //If the button is clicked, change the editor to edit mode, choose this item and cache it's name.
                    if (GUILayout.Button(buttonContent, ItemAssetButtonStyle, GUILayout.ExpandWidth(true)))
                    {
                        _state = DisplayState.EDIT;
                        _selectedItem = foldoutData.itemList[i];
                        _cachedItemName = _selectedItem.ItemName;
                    }
                    GUILayout.EndHorizontal();

                }

            }
        }
        EditorGUILayout.EndVertical();
    }

    ///<summary>Display the main Area of the editor.</summary>
    void DisplayMainArea()
    {
        //Create a layout,
        EditorGUILayout.BeginVertical("box", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        DisplayEditMainArea();

        EditorGUILayout.EndVertical();
    }

    ///<summary>Diplays the main item editing area of the editor.</summary>
    void DisplayEditMainArea()
    {
        //If there is an item selected, draw it's sections in order.
        // - Generic Item Data
        // - It's Description _ Bonuses
        // - Any specific Item Data to it's type. (i.e, Damage for weapons.)
        if (itemToEdit != null)
        {
            itemToEdit.DrawItemEditorGenerics();
            itemToEdit.DrawItemEditorDescriptionAndBonuses();
            itemToEdit.DrawItemEditorSpecifics();
        }
        else //Else show a No Item Selected screen.
        {
            EditorGUILayout.LabelField("No Item Selected", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
        }
    }

    ///<summary>Displays the main item creation section of the editor.</summary>
    void DisplayCreateItemArea()
    {
        //Create a layout
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Create New Item");
        EditorGUILayout.BeginVertical();

        //Show each kind of item we can create. as a button
        foreach (AssetFoldoutData asset in AssetList)
        {
            //Create an item of this time if the button is clicked.
            if (GUILayout.Button(DirectiveEnumUtils.GetPrettyItemType(asset.type)))
            {
                CreateNewItem(asset);
            }
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndVertical();
    }

    ///  ----------- ----------- ----------- ----------- OPTION BUTTON FUNCTIONALITY

    ///<summary>Create a New button that changes the editor state to create.</summary>
    void DisplayNewButton()
    {
        if (GUILayout.Button("New Item"))
        {
            _state = DisplayState.CREATE;
        }

    }

    ///<summary>Create a button that allows an item to be saved.</summary>
    void DisplaySaveButton()
    {
        if (GUILayout.Button("Save Item"))
        {
            //Changes the editor to edit mode.
            _state = DisplayState.EDIT;

            //Saves the current item.
            SaveItem(_selectedItem);
        }
    }

    ///<summary>Create a button that allows cancelling of an item being created.</summary>
    void DisplayCancelButton()
    {
        if (GUILayout.Button("Cancel"))
        {
            //Imediately gets rid of the current item.
            DestroyImmediate(_selectedItem);

            //Reloads the UI 
            Reload();

            //Changes editor back to create mode.
            _state = DisplayState.CREATE;
        }
    }

    ///<summary>Creates a button that allows for the deletion of an item.</summary>
    void DisplayDeleteButton()
    {
        if (GUILayout.Button("Delete Item"))
        {
            //Will display a warning prompt asking to confim we want to delete this item.
            if (EditorUtility.DisplayDialog("Confirm Item Delete", "Are you sure you want to delete " + _selectedItem.ItemName + " from the Item Database?", "Yes", "No"))
            {
                //Deletes the item.
                DeleteItem();
            }
        }
    }

    ///<summary>Creates a button that allows us to create a clone of a selected item.</summary>
    void DisplayCloneButton()
    {
        if (GUILayout.Button("Clone Item"))
        {
            //Changes the editor to edit mode.
            _state = DisplayState.EDIT;

            //Copies the item.
            CopyItem();
        }
    }

    ///<summary>Method to allow the saving of an item to the database. Will create an on-disk asset.</summary>
    void SaveItem(ItemData item)
    {
        //Finds the correct filepath.
        string _filePath = DetermineSavePath(item);

        //Ensures the file path exists.
        DirectiveAssetUtilities.CheckIfFilePathExists(_filePath, true);

        //Checks to see if the item already exists at that path.
        ItemData _existingItem = AssetDatabase.LoadAssetAtPath<ItemData>("Assets/Resources/" + _filePath + _cachedItemName + ".asset");

        //If there is an already existing item, overwrite it with this one.
        if (_existingItem != null)
        {
            Debug.Log("Item Already Exists");

            //Copy the data from the new item into this one.
            EditorUtility.CopySerialized(item, _existingItem);

            //Change the name to the new name.
            AssetDatabase.RenameAsset("Assets/Resources/" + _filePath + _cachedItemName + ".asset", item.ItemName);
        }
        else //Else create a new item.
        {
            //Get the FULL filepath.
            string fullPath = "Assets/Resources/" + _filePath + item.ItemName + ".asset";
            Debug.Log("Saving Asset : " + fullPath);

            //Create the asset with its name, at that path.
            AssetDatabase.CreateAsset(Instantiate(item), fullPath);
            Debug.Log("Creating Asset : " + fullPath);
        }

        //Set the item back to "clean" status.
        GetFoldoutData(item.ItemType).MarkAsClean();
    }

    ///<summary>Gets the foldout data of a given itemType.</summary>
    ///<param name="type">The type of item we wish to get the foldout data for.</param>
    AssetFoldoutData GetFoldoutData(ItemType type)
    {
        for (int i = 0; i < itemTypeCount; i++)
        {
            if (AssetList[i].type == type)
            {
                return AssetList[i];
            }
        }
        return null;

    }

    ///<summary>Determines the filepath we should be saving an item to.
    ///<param name="item">The item we are trying to save.</param>
    string DetermineSavePath(ItemData item)
    {
        string savePath = "Item Database/";
        savePath += ItemData.GetItemTypeName(item.ItemType) + "/";
        return savePath;
    }

    ///<summary>Creates a new item.</summary>
    ///<param name="data">The AssetData for this item.</param>
    void CreateNewItem(AssetFoldoutData data)
    {
        //An empty item for the new item.
        ItemData item;

        //Find the correct item Type, and cast the item to the appropriate type.
        switch (data.type)
        {
            // WEAPONS //
            case ItemType.ShipLightWeapon:
                item = new WeaponData();
                break;
            case ItemType.ShipMediumWeapon:
                item = new WeaponData();
                break;
            case ItemType.ShipHeavyWeapon:
                item = new WeaponData();
                break;
            case ItemType.ShipPrototypeWeapon:
                item = new WeaponData();
                break;

            // PROPULSION //
            case ItemType.ShipImpulseEngine:
                item = new EngineData();
                break;
            case ItemType.ShipAfterburnerDrive:
                item = new EngineData();
                break;
            case ItemType.ShipManuveringThrusters:
                item = new EngineData();
                break;
            case ItemType.ShipPhaseRiftDrive:
                item = new EngineData();
                break;

            // DEFENSIVE //
            case ItemType.ShipPrimaryHullPlating:
                item = new HullPlatingData();
                break;
            case ItemType.ShipSecondaryHullPlating:
                item = new ShieldData();
                break;
            case ItemType.ShipCombatShields:
                item = new HullPlatingData();
                break;
            case ItemType.ShipDeflectorShields:
                item = new ShieldData();
                break;
            // SENSORS //
            case ItemType.ShipShortRangeSensors:
                item = new SensorData();
                break;
            case ItemType.ShipLongRangeSensors:
                item = new SensorData();
                break;
            case ItemType.ShipTransponder:
                item = new SensorData();
                break;
            // CRITICAL //
            case ItemType.ShipPowerSupply:
                item = new LifeSupportData();
                break;
            case ItemType.ShipLifeSupport:
                item = new LifeSupportData();
                break;
            // AUX //
            case ItemType.ShipTacticalAuxiliarySystem:
                item = new SubSystemsData();
                break;
            case ItemType.ShipDefensiveAuxiliarySystem:
                item = new SubSystemsData();
                break;
            case ItemType.ShipOffensiveAuxiliarySystem:
                item = new SubSystemsData();
                break;

            case ItemType.Material:
                item = new MaterialData();
                break;
            case ItemType.Cargo:
                item = new CargoData();
                break;
            case ItemType.Junk:
                item = new JunkData();
                break;
            case ItemType.PersonalHeadArmour:
                item = new PersonalHeadArmorData();
                break;
            case ItemType.PersonalUpperArmour:
                item = new PersonalUpperArmourData();
                break;
            case ItemType.PersonalLowerArmour:
                item = new PersonalLowerArmourData();
                break;
            case ItemType.PersonalConsumable:
                item = new PersonalConsumableData();
                break;
            case ItemType.PersonalGadget:
                item = new PersonalGadgetData();
                break;
            case ItemType.PersonalMeleeWeapon:
                item = new PersonalMeleeWeaponData();
                break;
            case ItemType.PersonalRangedWeapon:
                item = new PersonalRangedWeaponData();
                break;

            case ItemType.PersonalLifeSupport:
                item = new PersonalLifeSupportData();
                break;
            case ItemType.PersonalThrusterPack:
                item = new PersonalThrusterPackData();
                break;
            case ItemType.ShipConsumable:
                item = new ShipConsumableData();
                break;

            default:
                item = new ItemData();
                break;
        }

        //Assign the item's data.
        item.ItemType = data.type;

        //Add it to the item list.
        data.itemList.Add(item);

        //Mark the item as needing to be saved.
        data.MarkAsDirty();

        //Set it as the item to be edited.
        _selectedItem = item;

        //Change the editor to edit mode.
        _state = DisplayState.EDIT;
    }

    ///<summary>Copies an item into a new item.</summary>
    void CopyItem()
    {
        //Creates a new instance of the item using it's data.
        _selectedItem = Instantiate(_selectedItem);

        //Denote the new item as a clone.
        _selectedItem.ItemName += "(Clone)";
        _selectedItem.name = _selectedItem.ItemName;

        //Add the item to it's appropriate foldout.
        GetFoldoutData(_selectedItem.ItemType).itemList.Add(_selectedItem);

        //Save the new item.
        SaveItem(_selectedItem);
    }


    ///<summary>Method to delete an item from the database.</summary>
    void DeleteItem()
    {
        //Searching each item asset foldout list, and then deleted the selected item.
        foreach (AssetFoldoutData asset in AssetList)
        {
            if (asset.type == _selectedItem.ItemType)
            {
                asset.DeleteItem(_selectedItem);
            }
        }

    }

    //Utils

    ///<summary>Utility method to create a small texture.</summary>
    ///<param name="width">The width of the new texture.</param>
    ///<param name="height">The height of the new texture.</param>
    ///<param name="col">The colour of the new 
    private Texture2D MakeTex(int width, int height, Color col)
    {
        //Creates a pixel array of colours.
        Color[] pix = new Color[width * height];

        //Craates each pixel in that colour.
        for (int i = 0; i < pix.Length; i++)
            pix[i] = col;

        //Creates a texture out of this pixel array.
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();

        return result;
    }

}

///<summary>Class representing a foldout list of items in the Item Editor.</summary>
class AssetFoldoutData
{
    ///<summary>Is this asset foldout expanded?</summary>
    public bool expanded;

    ///<summary>The list of items under this foldout.
    public List<ItemData> itemList;

    ///<summary>The type of items this list contains.</summary>
    public ItemType type;

    ///<summary>Is this foldout in need of saving?</summary>
    private bool _isDirty;

    ///<summary>Public accessor for checking if this asset needs saved or not.</summary>
    public bool isDirty { get { return _isDirty; } }

    ///<summary>Marks this asset as having unsaved changes.</summary>
    public void MarkAsDirty()
    {
        _isDirty = true;
    }

    ///<summary>Marks this asset as being saved.</summary>
    public void MarkAsClean()
    {
        _isDirty = false;
    }

    ///<summary>Consutrctor method for a new Asset Foldout.</summary>
    ///<param name="foldout">Should this foldout be created expanded?</param>
    ///<param name="type">What item type should this foldout represent?</param>
    public AssetFoldoutData(bool foldout, ItemType type)
    {
        expanded = foldout;
        this.type = type;

        //Add a list of all already created items from the appropriate path.
        itemList = new List<ItemData>();
        itemList.AddRange(Resources.LoadAll<ItemData>("Item Database/" + ItemData.GetItemTypeName(type)));
    }

    ///<summary>Deletes an item from the asset foldout.</summary>
    ///<param name="item">The item to delete.</param>
    public bool DeleteItem(ItemData item)
    {
        //If the file exists in the filepath, delete it and remove it from the item.
        if (DirectiveAssetUtilities.DeleteFile("Resources/Item Database/" + ItemData.GetItemTypeName(type) + "/" + item.ItemName + ".asset"))
        {
            itemList.Remove(item);
            return true;
        }
        else
        {
            //Else, the item doesn't exist on the disk. Remove it from the editor, show warning message. 
            itemList.Remove(item);
            Debug.LogError("Removed from Editor, but could not delete Item " + item.ItemName);
            return false;
        }
    }
}
