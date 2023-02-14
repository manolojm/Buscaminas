using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager2 : MonoBehaviour {
    public GameObject BlockPrefab;
    public int xColumn = 20;
    public int yRow = 18;
    public AudioClip bombClip;
    private float gameTIme = 120;
    public Text gameText;
    public Block[,] blockArrays;
    public CanvasGroup gamePanle;
    private bool isgame;

    /*private void Awake() {
        blockArrays = new Block[xColumn, yRow];
        // blocks= Block[xColumn,yRow];
        for (int i = 0; i < xColumn; i++) {
            for (int j = 0; j < yRow; j++) {
                GameObject tempBlock = Instantiate(BlockPrefab, correctPositive(i, j), Quaternion.identity);
                tempBlock.transform.parent = transform;
                blockArrays[i, j] = tempBlock.GetComponent<Block>();
                blockArrays[i, j].Init(i, j, Block.BlockSpriteType.Block, this);
            }
        }
        CreateBoobs();
        gamePanle.alpha = 0;
        gamePanle.blocksRaycasts = false;
        gamePanle.interactable = false;
        isgame = true;
    }*/

    // Update is called once per frame
    void Update() {
        if (isgame == false) {
            return;
        }
        if (gameTIme <= 0) {
            isgame = false;
            gamePanle.alpha = 1;
            gamePanle.blocksRaycasts = true;
            gamePanle.interactable = true;
        }
        gameText.text = (gameTIme -= Time.deltaTime).ToString("0.0");

        if (Input.GetMouseButtonDown(1)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool isCollider = Physics.Raycast(ray, out hit, int.MaxValue, LayerMask.GetMask("Blocks"));
            if (isCollider) {
                GameObject blockObject = hit.collider.gameObject;

                Block block = blockObject.GetComponent<Block>();
                Debug.Log(block.name);

                if (block.isFlaged == false && block.isOpen == false) {
                    block.isFlaged = true;
                    block.setSprite(Block.BlockSpriteType.BlockFlagged);
                } else {
                    Debug.Log("GetMouseButtonDown(1)");
                    block.isFlaged = false;
                    block.setSprite(Block.BlockSpriteType.Block);
                }
            }

        }

    }
    void CreateBoobs() {
        int count = 0;
        while (count < 40) {
            int x = Random.Range(0, xColumn);
            int y = Random.Range(0, yRow);
            if (blockArrays[x, y].isbombs == false) {
                blockArrays[x, y].isbombs = true;
                count++;
                // blockArrays[x, y].setSprite(Block.BlockSpriteType.Bomb);
            }
        }
    }
    public Vector3 correctPositive(float x, float y) {

        return new Vector3((transform.position.x - (xColumn) / 2f + x) / 2f, (transform.position.y + (yRow - 1) / 2f - y) / 2f, 0);
    }

    private ArrayList getNeighbours(Block block) {
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
    }

    public int getBoobNumber(Block block) {
        int bombNumber = 0;
        ArrayList blocks = getNeighbours(block);
        foreach (Block nblock in blocks) {
            if (nblock.isbombs == true)
                bombNumber++;
        }
        return bombNumber;
    }

    public void buttonPress(Block block) {
        if (Input.GetMouseButtonDown(0)) {
            if (block.isbombs == false && block.isFlaged == false) {

                if (block.isOpen == false) {
                    openBlock(block);
                }
            } else if (block.isbombs == true && block.isFlaged == false) {
                AudioSource.PlayClipAtPoint(bombClip, transform.position);
                block.setSprite(Block.BlockSpriteType.Bomb);
                Debug.Log("game over");
                isgame = false;
                gamePanle.alpha = 1;
                gamePanle.blocksRaycasts = true;
                gamePanle.interactable = true;
            }
        }
    }

    private void openBlock(Block block) {
        block.isOpen = true;
        int bombNumber = getBoobNumber(block);
        block.setImage(bombNumber);
        if (bombNumber == 0) {
            ArrayList blocksList = getNeighbours(block);
            foreach (Block nblock in blocksList) {
                if (nblock.isOpen == false) {
                    openBlock(nblock);
                }
            }
        }
    }

    public void restart() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame() {

        Application.Quit();
    }
}