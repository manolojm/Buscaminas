using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    [Header("Base")]
    Cell[,] cellMatrix;
    public Cell prefab;

    [Header("Sprites")]
    public Sprite vanilaSprite;
    public Sprite mineSprite;
    public Sprite freeSprite;
    public Sprite flagSprite;

    [Header("Entrada de datos")]
    public TMP_InputField inputX;
    public TMP_InputField inputY;
    public TMP_InputField inputMinas;

    [Header("Textos")]
    public TextMeshProUGUI textoVictoria;
    public TextMeshProUGUI textoDerrota;

    [Header("Variables editables")]
    private int numX;
    private int numY;
    private int numMinas;
    private int zCamara;
    private Boolean modoBandera;
    public static Boolean acabada;
    public static int puntos;

    [Header("Canvas")]
    public GameObject canvasDerrota;
    public GameObject canvasVictoria;
    public GameObject canvasJuego;

    [Header("Objetos")]
    public GameObject btnMina;
    public GameObject btnBandera;
    public Camera camara;

    // Crea la matriz y coloca las bombas
    void MatrizInstance() {

        // Dibuja el tablero
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
        
        // Rellena el tablero con casillas sin minas
        CellMatrixLoop((i, j) => {
                cellMatrix[i, j].Init(new Vector2Int(i, j), (false), Activate);
                cellMatrix[i, j].sprite = vanilaSprite;
        });

        // Rellena de forma aleatorio celdas con minas
        if (numMinas > numX * numY) {
            // Medida de seguridad
            numMinas = numX * numY - 1;
        }

        for (int k = 0; k < numMinas; k++) {
            int i = UnityEngine.Random.Range(0, numX);
            int j = UnityEngine.Random.Range(0, numY);

            if (!cellMatrix[i, j].EsMina()) {
                cellMatrix[i, j].Init(new Vector2Int(i, j), (true), Activate);
                cellMatrix[i, j].sprite = vanilaSprite;
            } else {
                k--;
            }
        }
    }

    // Comienza la partida
    public void ComenzarPartida() {
        ObtenerValores();
        AjustarCamara();
        EmpezarDeNuevo();
    }

    // Activa la celda seleccionada
    void Activate(int i, int j) {

        if (!modoBandera) {

            if (cellMatrix[i, j].showed) {
                return;
            }
            cellMatrix[i, j].showed = true;

            // Mina = fin del juego
            if (cellMatrix[i, j].mine) {

                cellMatrix[i, j].sprite = mineSprite;
                PartidaPerdida();

            } else {
                puntos++;
                cellMatrix[i, j].sprite = freeSprite;

                if (ArroundCount(i, j) == 0) {
                    ActivateArround(i, j);
                } else {
                    cellMatrix[i, j].text = ArroundCount(i, j).ToString();
                }

                if (puntos >= (numX * numY - numMinas)) {
                    PartidaGanada();
                }
            }

        } else {
            cellMatrix[i, j].sprite = flagSprite;
        }
    }

    // Activa las celdas anexas
    void ActivateArround(int i, int j) {
        if (PointIsInsideMatrix(i + 1, j)) {
            Activate(i + 1, j);
        }   
        if (PointIsInsideMatrix(i, j + 1)) {
            Activate(i, j + 1);
        }
        if (PointIsInsideMatrix(i + 1, j + 1)) {
            Activate(i + 1, j + 1);
        }
        if (PointIsInsideMatrix(i - 1, j)) {
            Activate(i - 1, j);
        }
        if (PointIsInsideMatrix(i, j - 1)) {
            Activate(i, j - 1);
        }
        if (PointIsInsideMatrix(i - 1, j - 1)) {
            Activate(i - 1, j - 1);
        }
        if (PointIsInsideMatrix(i - 1, j + 1)) {
            Activate(i - 1, j + 1);
        }
        if (PointIsInsideMatrix(i + 1, j - 1)) {
            Activate(i + 1, j - 1);
        }
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

    // Obtiene los valores de ancho, alto y número de minas
    void ObtenerValores() {

        // Por defecto los 4 valores son 1
        numX = 1;
        numY = 1;
        numMinas = 1;
        zCamara = 1;

        // Obtiene casillas ancho
        if (!String.IsNullOrEmpty(inputX.text)) {
            numX = byte.Parse(inputX.text);
            if (numX > zCamara) {
                zCamara = numX;
            }
        }

        // Obitnte casillas alto
        if (!String.IsNullOrEmpty(inputY.text)) {
            numY = byte.Parse(inputY.text);
            if (numY > zCamara) {
                zCamara = numY;
            }
        }

        // Obtiene minas
        if (!String.IsNullOrEmpty(inputMinas.text)) {
            numMinas = byte.Parse(inputMinas.text);
        }
    }

    // Ajusta la cámara para que se vea todo el tablero
    void AjustarCamara() {
        camara.transform.position = transform.position + new Vector3(0, 0, -1 - zCamara);
    }

    // Pierde la partida
    void PartidaPerdida() {
        acabada = true;
        textoDerrota.text = "Puntuación: " + puntos.ToString();
        canvasJuego.SetActive(false);
        canvasDerrota.SetActive(true);
    }

    // Gana la partida
    void PartidaGanada() {
        acabada = true;
        textoVictoria.text = "Puntuación: " + puntos.ToString();
        canvasJuego.SetActive(false);
        canvasVictoria.SetActive(true);
    }

    // Comienza de nuevo
    public void EmpezarDeNuevo() {
        puntos = 0;
        modoBandera = false;
        acabada = false;
        canvasJuego.SetActive(true);
        MatrizInstance();
    }

    // Recargar escena
    public void RecargarEscena() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Activa el modo poner bandera
    public void ActivarBandera() {
        modoBandera = true;
        btnBandera.GetComponent<Image>().color = Color.cyan;
        btnMina.GetComponent<Image>().color = Color.white;
    }

    // Activa el modo poner mina
    public void ActivarMina() {
        modoBandera = false;
        btnBandera.GetComponent<Image>().color = Color.white;
        btnMina.GetComponent<Image>().color = Color.cyan;
    }

    // Salir
    public void SalirApp() {
        Application.Quit();
    }

}