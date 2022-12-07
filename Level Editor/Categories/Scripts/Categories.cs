using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * ScriptableObject que representa cada categoria de objeto de la escena
 */ 
[CreateAssetMenu(fileName = "Categories", menuName = "Create Categories", order = 1)]
public class Categories : ScriptableObject
{
    /**
     * Lista de PrefabElements perteneciente a la categoria
     */ 
    public List<PrefabElement> prefabs;

    /**
     * Profundidad (eje z)
     */ 
    public float layer;

}
