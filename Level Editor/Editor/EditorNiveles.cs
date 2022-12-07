using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine.UIElements;

/**
 * Clase que permite visualizar el editor de niveles
 */
public class EditorNiveles : EditorWindow
{

    #region Grid

    /**
     * Columnas totales
     */
    int _totalColumns = 40;

    /**
     * Filas totales
     */
    int _totalRows = 50;

    /**
     * Tamaño de celda
     */
    int cellsize = 1;

    /**
     * Función llamada para actualizar los valores del grid con los datos modificados por el usuario
     */
    void Update_Grid()
    {
        if (SceneManagemnt._grid != null)
        {
            SceneManagemnt._totalColumns = _totalColumns;
            SceneManagemnt._totalRows = _totalRows;
            SceneManagemnt.Cellsize = cellsize;

            SceneManagemnt.set_Grid();
        }
    }

    #endregion

    #region Categorias

    /**
     *  Lista serializada de las categorias agregadas por el usuario
     */
    [SerializeField]
    List<Categories> _categories = new List<Categories>();

    /**
     *  Lista de nombres de las categorias asignadas por el usuario
     */
    List<string> categories_names = new List<string>();

    /**
     *   Categoria seleccionada. Representa la posicion en la lista de la categoria seleccionada 
     */
    int _categorySelected = 0;

    #endregion

    #region Objetos seleccionables

    /**
     *  Posicion del scroll
     */
    private Vector2 sp2d;

    /**
     *  posicion del scroll de la reglas
     */
    private Vector2 _scrollPositionRules;

    /**
     *   Ancho de los botones seleccionables
     */
    private const float button_width = 80;
    /**
    *   Alto de los botones seleccionables
    */
    private const float button_heigth = 90;

    /**
    *   Prefab seleccionado
    */
    PrefabElement actual_Item;

    #endregion

    #region Reglas

    /**
    *   Lista de reglas a eliminar
    */
    List<Rule> remove_rules = new List<Rule>();

    /**
    *   Posición de scroll
    */
    Vector2 scrollPos;

    #endregion

    /**
    *   Inicializa la ventana y la crea
    */
    [MenuItem("Editor Niveles/Open Editor")]
    static void Initialize()
    {
        EditorNiveles window = (EditorNiveles)EditorWindow.GetWindow(typeof(EditorNiveles), false, "Editor");
    }

    #region Inspector

    /**
    *   Funcion interna para customizar el inspector
    */
    void OnGUI()
    {
        #region Grid

        //Titulo
        GUILayout.Label("Grid", EditorStyles.boldLabel);

        //Setteamos las variables con los daots rellenados por el usuario
        _totalColumns = EditorGUILayout.IntField("Columnas", _totalColumns);
        _totalRows = EditorGUILayout.IntField("Filas", _totalRows);
        cellsize = EditorGUILayout.IntField("Celdas", cellsize);

        //Botton que si es pulsado actualiza los valores de la clase grid 
        if (GUILayout.Button("Update Grid"))
        {
            Update_Grid();
        }

        #endregion

        GUILayout.Space(30);

        #region Escenas

        //Titulo
        GUILayout.Label("Scenes", EditorStyles.boldLabel);

        
        EditorGUILayout.BeginHorizontal();

        //Boton que crea un nuevo nivel
        if (GUILayout.Button("New Level"))
        {
     
            SceneManagemnt.NewLevel();

            Update_Grid();
       
            actual_Item = null;

            GUIUtility.ExitGUI();
        }

        //Boton que guarda la escena
        if (GUILayout.Button("Save Level"))
        {
            foreach (Rule rule in SceneManagemnt.rules)
            {
                if (!SceneRules.Can_Save(rule))
                { Debug.Log("N" +
                    "o se puede guardar"); return; }
            }



            if (EditorSceneManager.SaveOpenScenes())
            {
                //Borramos el grid para que no se guarde en la escena
                if (SceneManagemnt._grid != null)
                    DestroyImmediate(SceneManagemnt._grid);

            
                GUIUtility.ExitGUI();
            }

        }


        EditorGUILayout.EndHorizontal();

        #endregion

        GUILayout.Space(30);

        #region Rules

        GUILayout.Label("Rules", EditorStyles.boldLabel);

            #region Botones de Reglas

        EditorGUILayout.BeginHorizontal();

        //Muestra una ventana para añadir reglas
        if (GUILayout.Button("Add Rule"))
        {
            AddRules.Initialize();
        }

        //Elimina todas las regla seleccionadas
        if (GUILayout.Button("Remove Selected Rules"))
        {
            foreach(Rule rule in remove_rules)
            {
                if(SceneManagemnt.rules.Contains(rule))
                {
                    SceneManagemnt.rules.Remove(rule);
                }
            }
        }

        //Elimina todas las reglas
        if (GUILayout.Button("Remove All Rules"))
        {

            SceneManagemnt.rules.Clear();
            remove_rules.Clear();
        }

        EditorGUILayout.EndHorizontal();

            #endregion

        //Posicion del area de scroll
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(600), GUILayout.Height(200));

        //Recorremos las reglas añadidas
        for (byte i = 0; i < SceneManagemnt.rules.Count; i++)
        {

            //Creamos una regla auxiliar para poder modificar las variables booleanas
            Rule aux = SceneManagemnt.rules[i];

            EditorGUILayout.BeginHorizontal();

            //Controla la exposicion de cada regla 
            aux.showposition = EditorGUILayout.Foldout(aux.showposition, aux.chosen_rule.ToString(),true);

            //Controla si es seleccionado para ser eliminado o no
            aux.remove = EditorGUILayout.Toggle(aux.remove);

            EditorGUILayout.EndHorizontal();

            //Si esta activada la exposicion de la regla mostramos los elementos de esta
            if (aux.showposition)
            {                
                EditorGUILayout.BeginHorizontal();
                if (aux.x != "")
                    GUILayout.Label(aux.x);
                if (aux.y != "")
                    GUILayout.Label(aux.y);
                if (aux.z != "")
                    GUILayout.Label(aux.z);

                if (aux.quantity != 0)
                    GUILayout.Label(aux.quantity.ToString());

                EditorGUILayout.EndHorizontal();
            }

            //Sustituimos la regla antigua por la actual
            SceneManagemnt.rules[i] = aux;

            //Añadimos o quitamos de la lista de reglas a eliminar
            if (SceneManagemnt.rules[i].remove == true)
                remove_rules.Add(SceneManagemnt.rules[i]);
            else if (remove_rules.Contains(SceneManagemnt.rules[i]))
                remove_rules.Remove(SceneManagemnt.rules[i]);


            
        }
        EditorGUILayout.EndScrollView();

        #endregion

        GUILayout.Space(40);
  
        #region Categorias y prefabs

        //--------------Categorias------------------------

        GUILayout.Label("Categories", EditorStyles.boldLabel);

        //Creamos un Scriptable object
        ScriptableObject target = this;
        //Creamos un serializable object y asignamos el target anteriormente creado
        SerializedObject so = new SerializedObject(target);
        //Vinculamos el serializeobject con la lista de categorias y se lo asignamos a la serialized property
        SerializedProperty stringsProperty = so.FindProperty("_categories");
        //Mostramos las propiedad serializada en el inspector para que pueda ser rellenada por el usuario
        EditorGUILayout.PropertyField(stringsProperty);
        //Necesario para aplicar los cambios
        so.ApplyModifiedProperties();



        //---------------Seleccion de prefabs--------------

        //Comprobamos si existen categorias asignadas en la lista
        if (_categories.Count > 0)
        {
          
            //Limpiamos la lista de nombres 
            categories_names.Clear();
            //Recorremos las categorias asignadas en la lista y almacenamos los nombres en la lista de nombres
            foreach (Categories category in _categories)
            {
                if (category != null)
                    categories_names.Add(category.name);
            }

            //Creamos los botones de selecion de categoria y recogemos cúal ha sido pulsada
            int index = _categorySelected;
    
            index = GUILayout.SelectionGrid(index, categories_names.ToArray(), 3);
            _categorySelected = index;

            //Hacemos un área scrolleable
            ScrollArea();
        }


        #endregion
        
    }

    #region Funciones auxiliares para la representacion de los prefabs

    /**
    *    Crea el área de scroll
    */
    private void ScrollArea()
    {
        //Si la categoria no tiene prefabs asignados mostramos el mensaje de que esta vacio
        if (_categories[_categorySelected].prefabs.Count == 0)
        {
            EditorGUILayout.HelpBox("La categoria esta vacía",MessageType.Info);
            return;
        }

        //establecemos el máximo de prefabs mostrados en pantalla
        int rowCapacity = Mathf.FloorToInt(position.width / (button_width));

        //Posicion 2D del scroll
        sp2d =GUILayout.BeginScrollView(sp2d);

        //Colocamos los botones de cada prefab correspondiente segun el grid establecido 
        int elementSelected = -1;
        elementSelected = GUILayout.SelectionGrid(elementSelected,Get_GUIContents(),rowCapacity, SetGUIStyle());

        //Obtenemos el objeto seleccionado si se pulsase un boton de prefab
        GetSelectedItem(elementSelected);

        GUILayout.EndScrollView();
    }

    /**
    *    Devuelve un array con las configuraciones Gui de cada prefab(nombre e imagen representativa)
    */
    private GUIContent[] Get_GUIContents()
    {
        //Lista auxiliar de GuiContens
        List<GUIContent> aux_guiContents = new List<GUIContent>();

        //Tamaño de la lista de categorias asignadas
        int totalItems = _categories[_categorySelected].prefabs.Count;

        //Recorremos la lista de prefab de esa categoria y almacenamos el guicontent de cada prefab
        foreach (PrefabElement prefab in _categories[_categorySelected].prefabs)
        {
            GUIContent guiContent = prefab.GetGuiContent();
            aux_guiContents.Add(guiContent);
        }


        //Devolvemos la lista auxiliar
        return aux_guiContents.ToArray();
    }

    /**
    *     Establece el estilo de los botones de los prefabs
    */
    private GUIStyle SetGUIStyle()
    {
        //Le damos estilo de boton
        GUIStyle guiStyle = new GUIStyle(GUI.skin.button);

        //Colocamos las posiciones del texto e imagen
        guiStyle.alignment = TextAnchor.LowerCenter;
        guiStyle.imagePosition = ImagePosition.ImageAbove;

        //Dimensiones del boton
        guiStyle.fixedWidth = button_width;
        guiStyle.fixedHeight = button_heigth;


        return guiStyle;
    }

    /**
    *   Devuelve el objeto seleccionado
    */
    private void GetSelectedItem(int index)
    {
        //Si algn boton ha sido seleccionado
        if (index != -1)
        {
            //Asignamos el prefab de la opcion seleccionada
            actual_Item =_categories[_categorySelected].prefabs[index];
            Debug.Log("Selected Item is: " + actual_Item.name);
        }
    }

    #endregion

    #endregion


    #region Scene

    /**
    *  Nos tenemos que suscribir a eventos de Scena porque en editor window no existe onSceneGUI
    */
    void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    /**
    *      Nos desuscribimos 
    */
    void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    /**
    *     Funcion para poder dibujar en la escena
    */
    private void OnSceneGUI(SceneView sceneView)
    {     
        //Solo disponible si existe un grid
        if (SceneManagemnt._grid != null)
            MouseDetection();
    }

    /**
    *    Funcion para detectar los eventos generados por el raton
    */
    private void MouseDetection()
    {
        //Desactivamos la posibilidad de interaccion del usuario con los gameobjects de la escena
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

        //Si no hay ningun objeto seleccionado no hace falta seguir
        if (actual_Item == null)
            return;
        //Recogemos el evento actual
        Event _event = Event.current;

        //Recogemos la camara actual de la escena en el editor
        Camera camera =SceneView.currentDrawingSceneView.camera;

        //Averiguamos la posicion del raton en la pantalla de escena y lo pasamos a Vector 2
        Vector3 mousePosition = _event.mousePosition;
        mousePosition = new Vector2(mousePosition.x, camera.pixelHeight -  mousePosition.y);
   
        //Averiguamos las coordenadas del mundo de la  posicion averiguada
        Vector3 worldPos = camera.ScreenToWorldPoint(mousePosition);

        //Convertimos esa posicion a coordenadas del grid y lo asignamos a las variables col y row 
        Vector3 gridPos = SceneManagemnt._grid.GetComponent<Grid>().WorldToGridCoordinates(worldPos);
        int col = (int)gridPos.x;
        int row = (int)gridPos.y;


        //Comprobamos si el raton ha sido pulsado o arrastrado por la pantalla https://docs.unity3d.com/ScriptReference/Event-button.html
        if (_event.type == EventType.MouseDown || _event.type == EventType.MouseDrag)
        {
            //Boton izquierdo pintar
            if (_event.button == 0)
                Create(col, row);
            //boton derecho borrar
            else if (_event.button == 1)
                Erase(col, row);
        }
    }

    #region Opciones de Raton

    /**
    *   Crea un objeto 
    */
    private void Create(int col, int row)
    {
       
       #region Comprobacion de  los casos limite

        if (actual_Item == null)
            return;

        Grid aux_grid = SceneManagemnt._grid.GetComponent<Grid>();


        for (int i = 0; i < actual_Item.prefab.transform.localScale.x; i++ ) // X
        {
            for (int j = 0; j < actual_Item.prefab.transform.localScale.y; j++) // Y
            {

                // Comprobamos que la coordenada este dentro del grid
                if (!aux_grid.IsOnGrid(col+i, row + j) || actual_Item == null)
                {
                    return;
                }
             
                if(aux_grid.cells_occupied.ContainsKey((col + i) + (row + j) * SceneManagemnt._totalColumns) &&
                   aux_grid.cells_occupied[(col + i) + (row + j) * SceneManagemnt._totalColumns].Count != 0)
                {
                   
                    foreach (GameObject _gameobject in aux_grid.cells_occupied[(col + i) + (row + j) * SceneManagemnt._totalColumns])
                    {
                        //Si esta la celda ya asignada como key y ademas su prefab no es nulo salimos de la funcion
                        if (_gameobject.transform.position.z == _categories[_categorySelected].layer)
                        {
                            Debug.Log("Ambos objetos están en el mismo layer");
                            return;
                        }
                    }

                }
              
            }
        }

        //Comprobar las reglas
        foreach (Rule rule in SceneManagemnt.rules)
        {         
            if (rule.chosen_rule != SceneRules._Rules.NONE )
            {            
                if (SceneRules.Has_Rule(rule, categories_names[_categorySelected], col, row, actual_Item.prefab) == true)
                {
                    return;
                }
            }
        }


        #endregion

       #region Asignacion del padre 

        GameObject parent;

        //Si ya existe recogemos el gameobject del diccionario
        if (SceneManagemnt.parents_category_prefabs.ContainsKey(_categories[_categorySelected].name))
            parent = SceneManagemnt.parents_category_prefabs[_categories[_categorySelected].name];
        //Si no existe, creamos un nuevo objeto y lo asignamos al diccionario
        else
        {
            parent = new GameObject(_categories[_categorySelected].name);
            SceneManagemnt.parents_category_prefabs.Add(_categories[_categorySelected].name,parent);         
        }

        #endregion

       #region Creacion de objeto

        // Creamos el objeto con el prefab del objeto seleccionado
        GameObject obj = Instantiate(actual_Item.prefab);

        //Creamos un nuevo tile
        Tiles new_tiles = new Tiles(_categories[_categorySelected].name,  obj, col,row);

        //Le damos nombre al gameobject visible en el inspector
        if (SceneManagemnt.scene_prefab.ContainsKey(parent.name))
            new_tiles.GetPrefab().name = actual_Item.name + " " + SceneManagemnt.scene_prefab[parent.name].Count;
        else
            new_tiles.GetPrefab().name = actual_Item.name;

        //Asignamos el padre del objeto y su posicion e nel mundo
        new_tiles.GetPrefab().transform.parent = parent.transform;
        new_tiles.GetPrefab().transform.position = aux_grid.GridToWorldCoordinates(col, row, _categories[_categorySelected].layer);

        #endregion

        #region  Ajustes de posicion para objetos con escala mayor que uno

        /*
         * Se pretende conseguir una mayor precision por parte del usuario en la colocacion de los objetos.
         * Con estos ajustes, los objetos siempre se colocaran desde la esquina inferior izquierda
         * 
         * */

        //Transform auxiliar
        Transform newTile_Transform = new_tiles.GetPrefab().transform;

        //Si es par lo desplazamos 0.5 a la derecha y hacia arriba dependiendo de la escala que tengan
        if (obj.transform.localScale.x % 2 == 0)
            newTile_Transform.position = new Vector3(newTile_Transform.position.x + 0.5f*(obj.transform.localScale.x - 1),
                                                     newTile_Transform.position.y + 0.5f * (obj.transform.localScale.y - 1),
                                                     newTile_Transform.position.z);

        //Si son impares y distintos de 1 , restamos a la escala 1 y dividimosentre dos (EJ: Scala = 5 -> (5-1)/2)
        else if (obj.transform.localScale.x >= 3)
            newTile_Transform.position = new Vector3(newTile_Transform.position.x + ((obj.transform.localScale.x - 1) / 2),
                                                     newTile_Transform.position.y + ((obj.transform.localScale.y - 1) / 2),
                                                     newTile_Transform.position.z);

        #endregion

        //Añadimos al diccionario de celdas ocupadas del grid el prefab. 
        //Si ocupa varias casillas en x y en y tambien las asignamos con el mismo prefab. Eso evita solapamiento de objetos
        for (int i = 0; i < obj.transform.localScale.x; ++i)// X
        {
            for (int j = 0; j < obj.transform.localScale.y; ++j) //  Y
            {
                //Si la key ya estaba en el diccionario
                if (aux_grid.cells_occupied.ContainsKey((col + i) + (row + j) * SceneManagemnt._totalColumns))
                {
                    aux_grid.cells_occupied[(col + i) + (row + j) * SceneManagemnt._totalColumns].Add( new_tiles.GetPrefab());

                    aux_grid.cells_occupied_categorie[(col + i) + (row + j) * SceneManagemnt._totalColumns].Add(_categories[_categorySelected]);
                }
                else
                {
                    aux_grid.cells_occupied.Add((col + i) + (row + j) * SceneManagemnt._totalColumns,new List<GameObject> { new_tiles.GetPrefab() });

                    aux_grid.cells_occupied_categorie.Add((col + i) + (row + j) * SceneManagemnt._totalColumns,new List<Categories> { _categories[_categorySelected] });
                }
       
                //Añadimos al diccionario de la escena los objetos
                if (SceneManagemnt.scene_prefab.ContainsKey(parent.name)) //Si ya esta creada la categoria
                    SceneManagemnt.scene_prefab[parent.name].Add(new_tiles);
                else
                    SceneManagemnt.scene_prefab.Add(parent.name, new List<Tiles> { new_tiles });
            }
        }     
    }

    /**
    *  Borra un objeto
    */
    private void Erase(int col, int row)
    {
        Grid aux_grid = SceneManagemnt._grid.GetComponent<Grid>();

        //Comprueba que esa casilla este ocupada
        if (aux_grid.cells_occupied.ContainsKey((col) + (row) * SceneManagemnt._totalColumns)==false) return;

        //Variables auxiliares
        Tiles auxtile = null;
        GameObject aux_gm = null;
        float aux = Mathf.Infinity;

        //Averiguamos el objeto en la z de menor valor alojado en esa casilla
        foreach(GameObject _gm in aux_grid.cells_occupied[(col) + (row) * SceneManagemnt._totalColumns])
        {
            if(_gm.transform.position.z < aux)
            {
                aux_gm = _gm;
                //  DestroyImmediate(SceneManagemnt._grid.GetComponent<Grid>().cells_occupied[(col) + (row) * SceneManagemnt._totalColumns]);
                
                
            }
        }

       
        //Obtengo el Tile que tiene ese objeto
        foreach (KeyValuePair<string,List<Tiles>> pair in SceneManagemnt.scene_prefab)
        {
            foreach(Tiles tile in pair.Value)
            {
                if(tile.GetPrefab() == aux_gm)
                {
                    auxtile = tile;
                    SceneManagemnt.scene_prefab[auxtile.get_category()].Remove(tile);

                    break;
                    
                }
            }
            if (auxtile != null)
                break;
        }

        //Quito todas las key del categories
        Categories aux_category = null;
        foreach (Categories category in _categories)
        {
            if(category.name == auxtile.get_category())
            {
                aux_category = category;
                break;
            }
        }

        for (int i = 0; i < auxtile.GetPrefab().transform.localScale.x; i++) //X
        {
            for (int j = 0; j < auxtile.GetPrefab().transform.localScale.y; j++) // Y
            {

                //categories
                aux_grid.cells_occupied_categorie[(auxtile.get_Init_Col() + i) + (auxtile.get_Init_Row() + j) * SceneManagemnt._totalColumns].Remove(aux_category);
           
                if (aux_grid.cells_occupied_categorie[(auxtile.get_Init_Col() + i) + (auxtile.get_Init_Row() + j) * SceneManagemnt._totalColumns].Count == 0)
                    aux_grid.cells_occupied_categorie.Remove((auxtile.get_Init_Col() + i) + (auxtile.get_Init_Row() + j) * SceneManagemnt._totalColumns);

                //Gameobject
                aux_grid.cells_occupied[(auxtile.get_Init_Col() + i) + (auxtile.get_Init_Row() + j) * SceneManagemnt._totalColumns].Remove(aux_gm);
                
               
                if (aux_grid.cells_occupied[(auxtile.get_Init_Col() + i) + (auxtile.get_Init_Row() + j) * SceneManagemnt._totalColumns].Count == 0)
                {
                    aux_grid.cells_occupied.Remove((auxtile.get_Init_Col() + i) + (auxtile.get_Init_Row() + j) * SceneManagemnt._totalColumns);
                    
                }
            }
        }

        DestroyImmediate(aux_gm);

    }

    #endregion

    #endregion


}

