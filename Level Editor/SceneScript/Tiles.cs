using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Clase que recolpila la información de cada objeto creado
 */ 
public class Tiles 
{
    
    /**       
     *  Gameobject asociado al tile
     */
    GameObject prefab;

   /** 
    *  Nombre de la categoria a la que pertenece
    */ 
    string category = "";

    /**
     * Columna y fila inicial del objeto
     */
    int col_init, row_init;
    
    /** 
     * Contructor
     */
    public Tiles(string _label, GameObject _prefab , int col , int row)
    {
        //Assignamos las variables de prefab y categoria
        category = _label;
        prefab = _prefab;
        col_init = col;
        row_init = row;
        
    }

    //Getters 

    public int get_Init_Col() { return col_init; }
    public int get_Init_Row() { return row_init; }
    public string get_category() { return category; }

    public GameObject GetPrefab(){return prefab;}

}
