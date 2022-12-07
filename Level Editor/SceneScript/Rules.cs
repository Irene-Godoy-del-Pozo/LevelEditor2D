using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


#if UNITY_EDITOR

/**      
 *    Estructura de datos que representará cada regla creada
 */
[SerializeField]
public struct Rule
{
    /**
     *   Tipo de regla
     */
    public SceneRules._Rules chosen_rule;

    /**
     * Categororia X
     */
    public string x;
    /**
     * Categororia Y
     */
    public string y;
    /**
     * Categororia Z
     */
    public string z;

    /**
     * Representará el número de veces que se ejecutara la regla , o las casiilas que necesitará comprobar
     */
    public int quantity;

    /**
     * Comprueba si ha sido seleccionada o no
     */
    public bool showposition;

    /**
     * Comprueba ha sido seleccionada para ser eliminada o no
     */
    public bool remove;


}

/**
 * Clase estática que contiene toda la lógica de las reglas
 */
public static class SceneRules 
{
    /**
     * Enumeración de las reglas
     */
    public enum _Rules
    {
        X_to_Q_cells_of_Y,
        Min_X,
        Max_X,
        X_close_to_Y,
        NONE

    }

    /** <summary>
     *  Comprueba las casillas adyacentes buscando objetos de una categoria concreta 
     */ 
    static bool Cell_check(int k,int col, int row, GameObject prefab, string second_categorie)
    {
        //Columnas
        for (int i = col - k; i <= (col + k + prefab.transform.localScale.x - 1); i++)
        {
            //Comprobamos si no se sale del grid
            if (i >= 0 && i <= SceneManagemnt._totalColumns)
            {
               //Filas
                for (int j = row - k; j <= (row + k + prefab.transform.localScale.y - 1); j++)
                {
                    //Comprobamos si no se sale del grid
                    if (j >= 0 && j <= SceneManagemnt._totalRows)
                    {
                        
                        //Evitamos las casillas donde estaran nuestro objeto
                        if ((i < col || i > col + prefab.transform.localScale.x - 1) ||
                            (j < row || j > row + prefab.transform.localScale.y - 1))
                        {
                            //Si la escena contiene una key en esa casilla
                            if (SceneManagemnt._grid.GetComponent<Grid>().cells_occupied_categorie.ContainsKey(i + j * SceneManagemnt._totalColumns) == true )
                            {
                                //Recorremos la lista de categorias y comprobamos si alguna coincide con la categoria dada por parametro
                                foreach ( Categories category in SceneManagemnt._grid.GetComponent<Grid>().cells_occupied_categorie[(i + j * SceneManagemnt._totalColumns)])
                                {
                                    if (category.name == second_categorie)
                                        return true;

                                }
                            }
                        }
                    }
                }
            }
        }
         
        return false;
    }

    /**
     * Funcionalidad de las reglas que deben ser comprobadas en el momento de colocacion de un objeto
     */
    public static bool Has_Rule(Rule rule, string categorie,int col, int row, GameObject prefab )
    {
        switch(rule.chosen_rule)
        {
            //Una categoría no puede estar a Q casillas de otra
            case _Rules.X_to_Q_cells_of_Y:

                if (rule.x == categorie)
                    return Cell_check(rule.quantity, col, row, prefab, rule.y);
                else if (rule.y == categorie)
                    return Cell_check(rule.quantity, col, row, prefab, rule.x);

                
                break;
                
            //Maximo de una categoria    
            case _Rules.Max_X:
                if (rule.x == categorie && SceneManagemnt.scene_prefab.ContainsKey(categorie))
                {
                    if (SceneManagemnt.scene_prefab[categorie].Count  < rule.quantity)
                    {
                        return false;                     
                    }
                    else
                    {
                        return true; 
                    }
                }
                break;

            //Una categoria debe estar al lado de la otra
            case _Rules.X_close_to_Y:
                if (rule.x == categorie)
                    return !Cell_check(1, col, row, prefab, rule.y);
                
                break;

        }
        return false;
    }

    /**
     * Funcionalidad de las reglas que deben ser comprobadas en el momento de guardar la escena
     */
    public static bool Can_Save (Rule rule)
    {
        switch(rule.chosen_rule)
        {
            //Minimo de una ctaegoria
            case _Rules.Min_X:

                if (SceneManagemnt.scene_prefab.ContainsKey(rule.x) &&
                   (SceneManagemnt.scene_prefab[rule.x].Count < rule.quantity))
                    return false;

                break;
        }

        return true;
    }
}

#endif