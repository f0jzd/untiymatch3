using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;


public class GUI : MonoBehaviour
{
    public static GUI instance;

    public TextMesh scoreText;
    
    private int score = 0;

    private int movesLeft = 60;




    private void Awake() {
        instance = GetComponent<GUI>();

        scoreText.text = "Score: " + score.ToString();
        
    }

    public int Score {get { return score;} set { score = value; scoreText.text = score.ToString(); } }
    
}