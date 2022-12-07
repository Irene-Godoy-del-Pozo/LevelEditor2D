using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Clase que se encarga de manejar y dibujar el grid 
 */
public class Grid : MonoBehaviour
{
    /**
     * Total de columnas
     */
    int _totalColumns;

    /**
     *  Total de filas
     */
    int _totalRows ;

    /**
     *  Tamaño de las celdas respecto a las unidades de Unity
     */
    int Cellsize ;

    /**
     *   Diccionario que relaciona una casilla del grid con el objeto que lo ocupa
     */
    public Dictionary<int,List< GameObject>> cells_occupied = new Dictionary<int, List<GameObject>>();

    /**
     *   Diccionario que relaciona una casilla del grid con la categoria del objeto que lo ocupa
     */
    public Dictionary<int,List< Categories>> cells_occupied_categorie = new Dictionary<int, List<Categories>>();

    /**
     *   Cuando es creado vaciamos el diccionario
     */
    private void Start()
    {
        cells_occupied.Clear();
    }

    /**
     *   Settea / actualiza los valores del grid
     */
    public void Set_Variables(int cols, int row, int cellsize)
    {
        _totalColumns = cols;
        _totalRows = row;
        Cellsize = cellsize;
    }

    #region Gizmos

    /**
     *  Dibuja los Bordes exteriores del grid
     */
    private void OutsideGridGizmo(int cols, int rows)
    {
        //Izquierda
        Gizmos.DrawLine(new Vector3(0, 0, 0), new Vector3(0, rows * Cellsize, 0));

        //Abajo
        Gizmos.DrawLine(new Vector3(0, 0, 0), new Vector3(cols * Cellsize, 0, 0));

        //Derecha
        Gizmos.DrawLine(new Vector3(cols * Cellsize, 0, 0), new Vector3(cols * Cellsize, rows * Cellsize, 0));

        //Arriba
        Gizmos.DrawLine(new Vector3(0, rows * Cellsize, 0), new Vector3(cols * Cellsize, rows * Cellsize, 0));
    }

    /**
     *   Dibuja las lineas interiores del grid
     */
    private void InsideGridGizmo(int cols, int rows)
    {
        //Dibuja las columnas
        for (int i = 1; i < cols; i++)
        {
            Gizmos.DrawLine(new Vector3(i * Cellsize, 0, 0), new Vector3(i * Cellsize, rows * Cellsize, 0));
        }

        //Dibuja las filas
        for (int j = 1; j < rows; j++)
        {
            Gizmos.DrawLine(new Vector3(0, j * Cellsize, 0), new Vector3(cols * Cellsize, j * Cellsize, 0));
        }
    }

    /**
     *   Funcion interna que hace posible dibujar gizmos
     */
    private void OnDrawGizmos()
    {
        //Color del Grid
        Gizmos.color = Color.white;
        InsideGridGizmo(_totalColumns, _totalRows);

        Gizmos.color = Color.red;
        OutsideGridGizmo(_totalColumns, _totalRows);
    }

    #endregion

    #region Traducciones de coordenadas

    /**
     *  Traduce las coordenadas del mundo a coordenadas del grid
     */
    public Vector3 WorldToGridCoordinates(Vector3 point)
    {
        Vector3 gridPoint = new Vector3((int)((point.x - transform.position.x) / Cellsize),
                                        (int)((point.y - transform.position.y) / Cellsize), 
                                        0.0f); //El grid siempre estará en la posición z = 0

        return gridPoint;
    }

    /**
     *   Traduce las coordenadas del grid a coordenadas del mundo
     */
    public Vector3 GridToWorldCoordinates(int col, int row, float z = 0) 
    {
        Vector3 worldPoint = new Vector3(transform.position.x + (col * Cellsize + Cellsize / 2.0f),
                                         transform.position.y + (row * Cellsize + Cellsize / 2.0f),
                                         z);
        return worldPoint;
    }
    #endregion

    #region Comprobaciones de posicion

    //Comprueba si el punto dado esta dentro del grid:

    /**
     *   Pasandole un vector 3 (coordenadas del mundo)
     */
    public bool IsOnGrid(Vector3 point)
    {
        //Averiguamos los valores minimos y maximos de los ejes x e y
        float minX = transform.position.x;
        float maxX = minX + _totalColumns * Cellsize;

        float minY = transform.position.y;
        float maxY = minY + _totalRows * Cellsize;

        return (point.x >= minX && point.x <= maxX && point.y >= minY && point.y <= maxY);
    }

    /**
     *    Pasandole un columna y fila
     */
    public bool IsOnGrid(int col, int row)
    {
        return (col >= 0 && col < _totalColumns && row >= 0 && row < _totalRows);
    }

    #endregion


}

