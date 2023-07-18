using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance;
    public List<Sprite> chars = new List<Sprite>();
    public GameObject tile;
    public int xSize, ySize;

    private GameObject[,] tiles;

    public bool shifting { get; set; }

    void Start() {
        instance = GetComponent<BoardManager>();

        Vector2 offset = tile.GetComponent<SpriteRenderer>().bounds.size;
        CreateBoard(offset.x, offset.y);
    }

    private void CreateBoard(float xOffset, float yOffset) {
        tiles = new GameObject[xSize, ySize];


        float startX = transform.position.x;
        float startY = transform.position.y;

        Sprite[] prevLeft = new Sprite[ySize];
        Sprite prevDown = null;

        for (int x = 0; x < xSize; x++) {
            for (int y = 0; y < ySize; y++) {
                GameObject newTile = Instantiate(tile, new Vector3(startX + (xOffset * x), startY + (yOffset * y), 0), tile.transform.rotation);
                tiles[x, y] = newTile;
                newTile.transform.parent = transform;

                List<Sprite> possibleChars = new List<Sprite>();
                possibleChars.AddRange(chars);
                possibleChars.Remove(prevLeft[y]);
                possibleChars.Remove(prevDown);


                Sprite newSprite = possibleChars[Random.Range(0, possibleChars.Count)];
                newTile.GetComponent<SpriteRenderer>().sprite = newSprite;

                prevLeft[y] = newSprite;
                prevDown = newSprite;
            }
        }
    }

    public IEnumerator FindEmptyTiles() {
        for (int i = 0; i < xSize; i++) {
            for (int j = 0; j < ySize; j++) {
                if (tiles[i, j].GetComponent<SpriteRenderer>().sprite == null) {
                    yield return StartCoroutine(ShiftTiles(i, j));
                    break;
                }
            }

        }

        for (int x = 0; x < xSize; x++) { 
            for (int y = 0; y < ySize; y++) {
                tiles[x,y].GetComponent<Tile>().ClearAllMatches();
            }
        }
    }

    private IEnumerator ShiftTiles (int x, int yPos, float shiftDelay = 0.3f) {
        shifting = true;
        List<SpriteRenderer> renders = new List<SpriteRenderer>();
        int emptyTileCount = 0;

        for (int y = yPos; y < ySize; y++) {
            SpriteRenderer render = tiles[x, y].GetComponent<SpriteRenderer>();
            if (render.sprite == null) { 
                emptyTileCount++;
            }
            renders.Add(render);
        }

        for (int i = 0; i < emptyTileCount; i++) {
            GUI.instance.Score += 500;
            yield return new WaitForSeconds(shiftDelay);
            for (int j = 0; j < renders.Count - 1; j++) {
                renders[j].sprite = renders[j + 1].sprite;
                renders[j + 1].sprite = GetNewSprite(x,ySize-1);
            }
        }
        shifting = false;
    }

    private Sprite GetNewSprite (int x, int y) {
        List<Sprite> possibleSprites = new List<Sprite>();
        possibleSprites.AddRange(chars);

        if (x > 0)
            possibleSprites.Remove(tiles[x - 1, y].GetComponent<SpriteRenderer>().sprite);
        
        if (x < xSize - 1)
            possibleSprites.Remove(tiles[x + 1, y].GetComponent<SpriteRenderer>().sprite);

        if(y > 0)
            possibleSprites.Remove(tiles[x, y - 1].GetComponent<SpriteRenderer>().sprite);

        return possibleSprites[Random.Range(0, possibleSprites.Count)];

    }
}
