using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Tile : MonoBehaviour
{


    private static Color chosenColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);
    private static Tile prevSelection = null;

    private SpriteRenderer render;
    private bool isChosen = false;

    private Vector2[] directions = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };


    void Awake()
    {
        render = GetComponent<SpriteRenderer>();
    }
    private void Select()
    {
        isChosen = true;
        render.color = chosenColor;
        prevSelection = GetComponent<Tile>();
    }
    private void Deselect()
    {
        isChosen = false;
        render.color = Color.white;
        prevSelection = null;
    }

    void OnMouseDown()
    {
        // 1
        if (render.sprite == null || BoardManager.instance.Moving)
        {
            return;
        }

        if (isChosen)
        {
            Deselect();
        }
        else
        {
            if (prevSelection == null)
            {
                Select();
            }
            else
            {
                if (GetAllAdjacentTiles().Contains(prevSelection.gameObject))
                { // 1
                    SwapSprite(prevSelection.render); // 2
                    prevSelection.Deselect();
                }
                else
                { // 3
                    prevSelection.GetComponent<Tile>().Deselect();
                    Select();
                }
            }

        }
    }


    public void SwapSprite(SpriteRenderer render2)
    {
        if (render.sprite == render2.sprite)
        {
            return;
        }

        Debug.Log("Swapping SPrites");

        Sprite tempSprite = render2.sprite;
        render2.sprite = render.sprite;
        render.sprite = tempSprite;
    }

    private GameObject GetAdjacent(Vector2 castDir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, castDir);
        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        return null;
    }



    private List<GameObject> GetAllAdjacentTiles()
    {
        List<GameObject> adjacentTiles = new List<GameObject>();
        for (int i = 0; i < directions.Length; i++)
        {
            adjacentTiles.Add(GetAdjacent(directions[i]));
        }
        return adjacentTiles;
    }
    private List<GameObject> FindMatch(Vector2 castDir)
    { // 1
        List<GameObject> matchingTiles = new List<GameObject>(); // 2
        RaycastHit2D hit = Physics2D.Raycast(transform.position, castDir); // 3
        while (hit.collider != null && hit.collider.GetComponent<SpriteRenderer>().sprite == render.sprite)
        { // 4
            matchingTiles.Add(hit.collider.gameObject);
            hit = Physics2D.Raycast(hit.collider.transform.position, castDir);
        }
        return matchingTiles; // 5
    }

}
