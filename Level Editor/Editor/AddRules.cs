using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

/**
*  Ventana para crear y configurar las reglas
*/
public class AddRules : EditorWindow
{
    /**
    *   Regla elegida
    */
    SceneRules._Rules rule = SceneRules._Rules.NONE;

    /**
    *   Categoría X
    */
    [SerializeField]
    Categories X;

    /**
    *   Categoría Y
    */
    [SerializeField]
    Categories Y;

    /**
    *   Categoría Z
    */
    [SerializeField]
    Categories Z;

    /**
    *   Cantidad de casillas 
    */
    int Q;

    /**
    *   Inicializa y crea la ventana
    */
    public static void Initialize()
    {
        AddRules window = (AddRules)EditorWindow.GetWindow(typeof(AddRules), false, "Add Rule");
    }

    /**
    *  Funcion interna para customizar el inspector
    */
    void OnGUI()
    {
        //Lista desplegable de reglas a elegir
        rule = (SceneRules._Rules)EditorGUILayout.EnumPopup( rule);

        //Creamos un Scriptable object
        ScriptableObject target = this;
        //Creamos un serializable object y asignamos el target anteriormente creado
        SerializedObject so = new SerializedObject(target);
        //Vinculamos el serializeobject con la categoria y se lo asignamos a la serialized property
        SerializedProperty stringsProperty1 = so.FindProperty("X");
        //Vinculamos el serializeobject con la categoria y se lo asignamos a la serialized property
        SerializedProperty stringsProperty2 = so.FindProperty("Y");
        //Vinculamos el serializeobject con la categoria y se lo asignamos a la serialized property
        SerializedProperty stringsProperty3 = so.FindProperty("Z");


        //Dependiendo de la regla elegida mostramos unas variables u otras para evitar confusiones
        switch (rule)
        {
            
            case SceneRules._Rules.X_to_Q_cells_of_Y:
                                                        
                //Mostramos las propiedad serializada en el inspector para que pueda ser rellenada por el usuario
                EditorGUILayout.PropertyField(stringsProperty1);
                
                //Mostramos las propiedad serializada en el inspector para que pueda ser rellenada por el usuario
                EditorGUILayout.PropertyField(stringsProperty2);
          
                Q = EditorGUILayout.IntField(Q);

                Z = null;

                break;

           case SceneRules._Rules.Min_X:

                //Mostramos las propiedad serializada en el inspector para que pueda ser rellenada por el usuario
                EditorGUILayout.PropertyField(stringsProperty1);

                Q = EditorGUILayout.IntField(Q);

                Y = Z = null;

                break;

            case SceneRules._Rules.Max_X:

                //Mostramos las propiedad serializada en el inspector para que pueda ser rellenada por el usuario
                EditorGUILayout.PropertyField(stringsProperty1);

                Q = EditorGUILayout.IntField(Q);

                Y = Z = null;

                break;

            case SceneRules._Rules.X_close_to_Y:

                //Mostramos las propiedad serializada en el inspector para que pueda ser rellenada por el usuario
                EditorGUILayout.PropertyField(stringsProperty1);


                //Mostramos las propiedad serializada en el inspector para que pueda ser rellenada por el usuario
                EditorGUILayout.PropertyField(stringsProperty2);

                Z = null;

                Q = 0;

                break;

            case SceneRules._Rules.NONE:

                GUILayout.Label("Chose a rule to add", EditorStyles.boldLabel);

                break;


        }

        //Necesario para aplicar los cambios
        so.ApplyModifiedProperties();

        //Boton para añadir una regla a la lista de reglas de la escena
        if (GUILayout.Button("Add Rule") )
        {
            if(rule != SceneRules._Rules.NONE)
            {
                string x = "";
                string y = "";
                string z = "";
                int q = 0;

                if (X != null)
                    x = X.name;
                if (Y != null)
                    y = Y.name;
                if (Z != null)
                    z = Z.name;
                if (Q != 0)
                    q = Q;
                Add_Rule_to_Scene(rule, x, y, z,q);

            }
        }
    }

    /**
    *  Añade una regla a la escena
    */
    void Add_Rule_to_Scene (SceneRules._Rules rule, string x , string y , string z , int q )
    {
        Rule aux;

        aux.chosen_rule = rule;
        aux.x = x;
        aux.y = y;
        aux.z = z;
        aux.quantity = q;

        aux.showposition = false;
        aux.remove = false;

        SceneManagemnt.rules.Add(aux);
    }

}
