using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generador : MonoBehaviour
{
    public GameObject celda;
    public int width, height;
    public GameObject[][] map;
    public int bombsNumber;

    // Start is called before the first frame update
    void Start()
    {
        map = new GameObject[width][];

        for (int i = 0; i < map.Length; i++) {
            map[i] = new GameObject[height];
        }

        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                map[i][j] = Instantiate(celda, new Vector2(i, j), Quaternion.identity);
            }
        }

        // Centramos la cámara
        Camera.main.transform.position = new Vector3(((float)width / 2 - 5f), ((float)height / 2 - 5f), -10);

        // Bombardeo
        for (int i = 0; i < bombsNumber; i++) {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            if (!map[x][y].GetComponent<Celda>().bomb) {
                map[x][y].GetComponent<Celda>().bomb = true;
            } else {
                i--;
            }
        }

        /*private ArrayList getNeighbours(Block block) {
            ArrayList blocklist = new ArrayList();

            if (block.x + 1 < xColumn && block.y + 1 < yRow) {
                blocklist.Add(blockArrays[block.x + 1, block.y + 1]);
            }

            if (block.x + 1 < xColumn && block.y - 1 >= 0) {
                blocklist.Add(blockArrays[block.x + 1, block.y - 1]);
            }
            if (block.x + 1 < xColumn) {
                blocklist.Add(blockArrays[block.x + 1, block.y]);
            }

            if (block.y + 1 < yRow) {
                blocklist.Add(blockArrays[block.x, block.y + 1]);
            }
            if (block.y - 1 >= 0) {
                blocklist.Add(blockArrays[block.x, block.y - 1]);
            }
            if (block.x - 1 >= 0 && block.y + 1 < yRow) {
                blocklist.Add(blockArrays[block.x - 1, block.y + 1]);
            }
            if (block.x - 1 >= 0) {
                blocklist.Add(blockArrays[block.x - 1, block.y]);
            }
            if (block.x - 1 >= 0 && block.y - 1 >= 0) {
                blocklist.Add(blockArrays[block.x - 1, block.y - 1]);
            }
            return blocklist;
        }*/
    }

    private void Update() {
        if (Input.GetMouseButtonDown(1)) {
            
        }
    }
}
