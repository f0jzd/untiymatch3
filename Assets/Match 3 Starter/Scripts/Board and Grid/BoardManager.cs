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

    public bool Moving { get; set; }

    void Start()
    {
        instance = GetComponent<BoardManager>();

        Vector2 offset = tile.GetComponent<SpriteRenderer>().bounds.size;
        CreateBoard(offset.x, offset.y);
    }

    private void CreateBoard(float xOffset, float yOffset)
    {
        tiles = new GameObject[xSize, ySize];


        float startX = transform.position.x;
        float startY = transform.position.y;

        Sprite[] prevLeft = new Sprite[ySize];
        Sprite prevDown = null;

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
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

}
