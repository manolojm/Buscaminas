using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {
    Cell[,] cellMatrix;
    bool ingame;

    public Vector2Int dimension;
    public Cell prefab;

    [Range(0, 100)]
    public int minePercent = 10;

    public Sprite vanilaSprite;
    public Sprite mineSprite;
    public Sprite freeSprite;

    public GameObject canvasFin;

    [Header("InputField")]
    public TMP_InputField inputX;
    public TMP_InputField inputY;
    public TMP_InputField inputMinas;

    private int numX;
    private int numY;
    private int numMinas;

    public Camera camara;

    void Start() {
        //MatrizInstance();
    }

    void MatrizInstance() {
        if (cellMatrix == null) {
            cellMatrix = new Cell[numX, numY];
            CellMatrixLoop((i, j) => {
                Cell go = Instantiate(prefab,
                    new Vector3(i - numX / 2f, j - numY / 2f),
                    Quaternion.identity,
                    transform);
                go.name = string.Format("(X:{0},Y:{1})", i, j);
                cellMatrix[i, j] = go;
            });
        }
        
        CellMatrixLoop((i, j) => {
            Debug.Log(i);
            cellMatrix[i, j].Init(new Vector2Int(i, j),
            (UnityEngine.Random.Range(0, 100) > minePercent ? false : true),
            Activate);
            cellMatrix[i, j].sprite = vanilaSprite;
        });
    }

    public void ComenzarPartida() {
        ObtenerValores();
        AjustarCamara();
        MatrizInstance();
    }

    // Activa la celda seleccionada
    void Activate(int i, int j) {
        if (cellMatrix[i, j].showed)
            return;
        cellMatrix[i, j].showed = true;

        if (cellMatrix[i, j].mine) {
            // FAIL STATE
            cellMatrix[i, j].sprite = mineSprite;
            FinJuego();

        } else {
            cellMatrix[i, j].sprite = freeSprite;

            if (ArroundCount(i, j) == 0) {
                ActivateArround(i, j);
            } else {
                cellMatrix[i, j].text = ArroundCount(i, j).ToString();
            }
        }
    }

    // Activa las celdas anexas
    void ActivateArround(int i, int j) {
        if (PointIsInsideMatrix(i + 1, j))
            Activate(i + 1, j);
        if (PointIsInsideMatrix(i, j + 1))
            Activate(i, j + 1);
        if (PointIsInsideMatrix(i + 1, j + 1))
            Activate(i + 1, j + 1);
        if (PointIsInsideMatrix(i - 1, j))
            Activate(i - 1, j);
        if (PointIsInsideMatrix(i, j - 1))
            Activate(i, j - 1);
        if (PointIsInsideMatrix(i - 1, j - 1))
            Activate(i - 1, j - 1);
        if (PointIsInsideMatrix(i - 1, j + 1))
            Activate(i - 1, j + 1);
        if (PointIsInsideMatrix(i + 1, j - 1))
            Activate(i + 1, j - 1);
    }
    void CellMatrixLoop(Action<int, int> e) {
        for (int i = 0; i < cellMatrix.GetLength(0); i++) {
            for (int j = 0; j < cellMatrix.GetLength(1); j++) {
                e(i, j);
            }
        }
    }

    bool PointIsInsideMatrix(int i, int j) {
        if (i >= cellMatrix.GetLength(0))
            return false;
        if (i < 0)
            return false;
        if (j >= cellMatrix.GetLength(1))
            return false;
        if (j < 0)
            return false;

        return true;
    }
    int ArroundCount(int i, int j) {
        int arround = 0;

        if (PointIsInsideMatrix(i + 1, j) && cellMatrix[i + 1, j].mine)
            arround++;
        if (PointIsInsideMatrix(i, j + 1) && cellMatrix[i, j + 1].mine)
            arround++;
        if (PointIsInsideMatrix(i + 1, j + 1) && cellMatrix[i + 1, j + 1].mine)
            arround++;
        if (PointIsInsideMatrix(i - 1, j) && cellMatrix[i - 1, j].mine)
            arround++;
        if (PointIsInsideMatrix(i, j - 1) && cellMatrix[i, j - 1].mine)
            arround++;
        if (PointIsInsideMatrix(i - 1, j - 1) && cellMatrix[i - 1, j - 1].mine)
            arround++;
        if (PointIsInsideMatrix(i - 1, j + 1) && cellMatrix[i - 1, j + 1].mine)
            arround++;
        if (PointIsInsideMatrix(i + 1, j - 1) && cellMatrix[i + 1, j - 1].mine)
            arround++;

        return arround;
    }

    void ObtenerValores() {

        // Por defecto los 3 valores son 10
        numX = 10;
        numY = 10;
        numMinas = 10;

        // Obtiene casillas ancho
        if (!String.IsNullOrEmpty(inputX.text)) {
            numX = byte.Parse(inputX.text);
        }

        // Obitnte casillas alto
        if (!String.IsNullOrEmpty(inputY.text)) {
            numY = byte.Parse(inputY.text);
        }

        // Obtiene minas
        if (!String.IsNullOrEmpty(inputMinas.text)) {
            numMinas = byte.Parse(inputMinas.text);
        }
    }

    void AjustarCamara() {
        camara.transform.position = transform.position + new Vector3(0, 0, -1 - numY);
    }

    void FinJuego() {
        canvasFin.SetActive(true);
    }

    public void EmpezarDeNuevo() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}