using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
*  Clase serializable que representa cada uno de los objetos de una clase
*/
[System.Serializable]
public class PrefabElement 
{
    /**
    *  Nombre del objeto
    */
    public string name;

    /**
    *    Textura que representará al objeto en el inspector
    */
    public Texture2D image;

    /**
    *     Gameobject del objeto 
    */
    public GameObject prefab;

    /**
    *    Devuelve un GuiContent con el nombre e imagen que se verán en el inspector 
    */
    public GUIContent GetGuiContent()
    {
        GUIContent guiContent = new GUIContent();
        guiContent.text = name;
        guiContent.image = image;

        return guiContent;
    }
}
