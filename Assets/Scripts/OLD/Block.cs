using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
    public enum BlockSpriteType {
        Block,
        BlockClicked,
        Bomb,
        BlockFlagged,
        one,
        tow,
        three,
        four,
        five,
        six,
        seven,
        eight
    }

    [System.Serializable]
    public struct BlockSpriteStruct {
        public BlockSpriteType type;
        public Sprite sprite;
    }

    private GameManager gameManger;
    public BlockSpriteStruct[] blockSpriteStructs;
    public int x;
    public int y;
    public Dictionary<BlockSpriteType, Sprite> blockSpriteDict;
    [HideInInspector]
    public SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = transform.Find("blockmodel").GetComponent<SpriteRenderer>();
        blockSpriteDict = new Dictionary<BlockSpriteType, Sprite>();
        for (int i = 0; i < blockSpriteStructs.Length; i++) {
            if (!blockSpriteDict.ContainsKey(blockSpriteStructs[i].type)) {
                blockSpriteDict.Add(blockSpriteStructs[i].type, blockSpriteStructs[i].sprite);
            }
        }
    }
    [HideInInspector]
    public BlockSpriteType type;
    public void Init(int x, int y, BlockSpriteType type, GameManager gameManger) {
        this.x = x;
        this.y = y;
        this.type = type;
        this.gameManger = gameManger;
        setSprite(type);
    }
    public void setImage(int number) {
        switch (number) {
            case 0:
                setSprite(BlockSpriteType.BlockClicked);
                break;
            case 1:
                setSprite(BlockSpriteType.one);
                break;
            case 2:
                setSprite(BlockSpriteType.tow);
                break;
            case 3:
                setSprite(BlockSpriteType.three);
                break;
            case 4:
                setSprite(BlockSpriteType.four);
                break;
            case 5:
                setSprite(BlockSpriteType.five);
                break;
            case 6:
                setSprite(BlockSpriteType.six);
                break;
            case 8:
                setSprite(BlockSpriteType.eight);
                break;
            case 7:
                setSprite(BlockSpriteType.seven);
                break;
            default:
                break;
        }
    }
    public void setSprite(BlockSpriteType type) {
        this.type = type;
        spriteRenderer.sprite = blockSpriteDict[type];
    }

    public bool isbombs = false;
    public bool isOpen = false;
    public bool isFlaged = false;

    void Start() {
        if (Input.GetMouseButtonDown(1)) {
            Debug.Log("GetMouseButtonDown(1)");
            if (isFlaged == false && isOpen == false) {
                isFlaged = true;
                setSprite(Block.BlockSpriteType.BlockFlagged);
            } else {
                isFlaged = false;
                setSprite(Block.BlockSpriteType.Block);
            }
        }
    }
    /*private void OnMouseDown() {
        Debug.Log(Input.GetMouseButton(1));
        gameManger.buttonPress(this);
    }*/
}