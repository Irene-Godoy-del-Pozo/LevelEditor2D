using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

#if UNITY_EDITOR

using UnityEditor.SceneManagement;
/**
 * Clase statica para manejar la creacion de escenas
 */
[SerializeField]
public static class SceneManagemnt
{
    #region Grid
    /**
     * Cuadrícula de la escena
     */
    public static GameObject _grid;

    /**
     * Columnas totales
     */
    public static int _totalColumns = 40;

    /**
     * Filas totales
     */
    public static int _totalRows = 50;

    /**
     * Tamaño de las celdas
     */
    public static int Cellsize = 1;
    #endregion

    /**
     * Diccionario que colecciona todos los Tiles de la escena y los clasifica por categoria
     */
    public static Dictionary<string, List<Tiles>> scene_prefab = new Dictionary<string, List<Tiles>>();

    /**
     * Gameobjects vacíos relacionados por categoria para que el inspector esté más ordenado
     */
    public static Dictionary<string, GameObject> parents_category_prefabs = new Dictionary<string, GameObject>();

    /**
     * Lista de Reglas de la escena
     */
    public static List<Rule> rules = new List<Rule>();

    /**
    * Crea una nueva escena
     */
    public static void CreateScene()
    {
        //Da la posibilidad de guardar la escena actual al usuario
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

        //Crea una nueva escena vacia
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);      
    }

    /**
     * Crea un nuevo nivel
     */
    public static void NewLevel()
    {
        //Creamos una nueva escena y la limpiamos
        CreateScene();
        Clear();

        //Nos aseguramos de poner el grid a null
        _grid = null;

        //Creamos un nuevo objeto y lo llamamos Grid
        _grid = new GameObject("Grid");

        //Le asignamos la posicion (0,0,0)
        _grid.transform.position = Vector3.zero;

        //Añadimos el Script Grid al Gameobject
        _grid.AddComponent<Grid>();

        //Seteamos las variables del grid
        set_Grid();
    }

    /**
     * Borra todos los objetos de la escena
     */
    public static void Clear()
    {
        //Encontramos todos los objetos de la escena y los destruimos
        GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();

        foreach (GameObject _object in allObjects)
        {
            GameObject.DestroyImmediate(_object);
        }

        //Nos aseguramos que los objetos del diccionario parents_category_prefab estan borrados y limpiamos el diccionario
        foreach (KeyValuePair<string, GameObject> pair in parents_category_prefabs)
        {
            GameObject.DestroyImmediate(pair.Value);
        }
        parents_category_prefabs.Clear();

        //Nos aseguramos que los objetos del diccionario scene_prefab estan borrados y limpiamos el diccionario
        foreach (KeyValuePair<string, List<Tiles>> pair in scene_prefab)
        {
            foreach (Tiles obj in pair.Value)
                GameObject.DestroyImmediate(obj.GetPrefab());
        }
        scene_prefab.Clear(); 
    }
    

    /**
     *   funcion que actualiza los valores de col, row y cellsize del grid
     */
    public static void set_Grid()
    {
       
         _grid.GetComponent<Grid>().Set_Variables(_totalColumns, _totalRows, Cellsize);
    }
}

#endif
