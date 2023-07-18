using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Tile : MonoBehaviour
{


    private static Color highlightColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);
    private static Tile prevSelection = null;

    private SpriteRenderer render;
    private bool isChosen = false;

    private Vector2[] directions = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };


    private bool isMatch = false;

    void Awake() {
        render = GetComponent<SpriteRenderer>();
    }
    private void Select() {
        isChosen = true;
        render.color = highlightColor;
        prevSelection = GetComponent<Tile>();
    }
    private void Deselect()
    {
        isChosen = false;
        render.color = Color.white;
        prevSelection = null;
    }

    void OnMouseDown() {
        if (render.sprite == null || BoardManager.instance.shifting) {
            return;
        }

        if (isChosen) {
            Deselect();
        }
        else {
            if (prevSelection == null) {
                Select();
            }
            else {
                if (GetAllAdjacentTiles().Contains(prevSelection.gameObject)) {
                    SwapSprite(prevSelection.render);
                    prevSelection.ClearAllMatches();
                    prevSelection.Deselect();
                    ClearAllMatches();
                }
                else {
                    prevSelection.GetComponent<Tile>().Deselect();
                    Select();
                }
            }

        }
    }


    public void SwapSprite(SpriteRenderer render2) {
        if (render.sprite == render2.sprite)
            return;

        Sprite temp = render2.sprite;
        render2.sprite = render.sprite;
        render.sprite = temp;
    }

    private GameObject GetAdjacent(Vector2 castDir) {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, castDir);
        if (hit.collider != null)
            return hit.collider.gameObject;
        
        return null;
    }
    private List<GameObject> GetAllAdjacentTiles() {
        List<GameObject> adjacentTiles = new List<GameObject>();
        for (int i = 0; i < directions.Length; i++) {
            adjacentTiles.Add(GetAdjacent(directions[i]));
        }
        return adjacentTiles;
    }
    private List<GameObject> FindMatch(Vector2 rayDir) {
        List<GameObject> matchingTiles = new List<GameObject>();
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDir);
        while (hit.collider != null && hit.collider.GetComponent<SpriteRenderer>().sprite == render.sprite) {
            matchingTiles.Add(hit.collider.gameObject);
            hit = Physics2D.Raycast(hit.collider.transform.position, rayDir);
        }
        return matchingTiles;
    }
    private void ClearMatchingTiles(Vector2[] tilePath) {
        List<GameObject> matchingTiles = new List<GameObject> ();

        for (int i = 0;i < tilePath.Length;i++) {
            matchingTiles.AddRange(FindMatch(tilePath[i]));
        }
        if (matchingTiles.Count >= 2) {
            for (int i = 0; i < matchingTiles.Count; i++) 
                matchingTiles[i].GetComponent<SpriteRenderer>().sprite = null;
            
            isMatch = true;
        }
    }

    public void ClearAllMatches() {
        if (render.sprite == null)
            return;

        ClearMatchingTiles(new Vector2[2] {Vector2.left,Vector2.right});
        ClearMatchingTiles(new Vector2[2] {Vector2.up,Vector2.down});
        if (isMatch) {
            render.sprite = null;
            isMatch = false;

            StopCoroutine(BoardManager.instance.FindEmptyTiles());
            StartCoroutine(BoardManager.instance.FindEmptyTiles());
        }
    }
}
