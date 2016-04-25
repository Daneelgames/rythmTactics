using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState {MoveRed, MoveBlue};

public class TurnManager : MonoBehaviour {
    
    public GameState gameState = GameState.MoveRed;
    public bool playerMoved = false;

    public int redMana = 1;
    public int blueMana = 1;

    public int redLives = 3;
    public int blueLives = 3;

    [SerializeField]
    private float turnTime = 1f;
    [HideInInspector]
    public float curTime;

    private bool canSetRedUnitMovedFalse = true;
    private bool canSetBlueUnitMovedFalse = true;

    //timer feedback

    [SerializeField]
    private Image contentBlue;
    [SerializeField]
    private Image contentRed;

    private float maxTimer = 2;
    private float curTimer = 2;
    private float minTimer = 0;

    private float minFill = 0f;
    private float maxFill = 1f;

    void Start()
    {
        curTime = turnTime;
        maxTimer = turnTime;
        curTimer = turnTime;

        contentRed.enabled = true;
        contentBlue.enabled = false;
    }

    void HandleTimer()
    {
        curTimer = curTime;
        contentBlue.fillAmount = Map(curTimer, minTimer, maxTimer, minFill, maxFill);
        contentRed.fillAmount = Map(curTimer, minTimer, maxTimer, minFill, maxFill);
    }

    float Map(float cur, float inMin, float inMax, float outMin, float outMax)
    {
        return (cur - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    void Update()
    {
        HandleTimer();
        GameOver();

        if (curTime > 0)
            curTime -= 1 * Time.deltaTime;
        else
        {
            if (gameState == GameState.MoveRed && canSetBlueUnitMovedFalse)
            {
                contentRed.enabled = false;
                contentBlue.enabled = true;

                playerMoved = false;
                canSetBlueUnitMovedFalse = false;
                canSetRedUnitMovedFalse = true;
                gameState = GameState.MoveBlue;

                if (blueMana < 5)
                    blueMana += 1;

                GameObject[] blues = GameObject.FindGameObjectsWithTag("UnitBlue");
                foreach (GameObject i in blues)
                {
                    i.GetComponent<UnitController>().unitMoved = false;
                }
            }
            else if (gameState == GameState.MoveBlue && canSetRedUnitMovedFalse)
            {
                contentRed.enabled = true;
                contentBlue.enabled = false;

                playerMoved = false;
                canSetRedUnitMovedFalse = false;
                canSetBlueUnitMovedFalse = true;
                gameState = GameState.MoveRed;

                if (redMana < 5)
                    redMana += 1;

                GameObject[] blues = GameObject.FindGameObjectsWithTag("UnitRed");
                foreach (GameObject i in blues)
                {
                    i.GetComponent<UnitController>().unitMoved = false;
                }
            }

            curTime = turnTime;
        }
    }

    void GameOver()
    {
        if (blueLives <= 0 || redLives <= 0)
            StartCoroutine("NewScene");
    }

    IEnumerator NewScene()
    {
        yield return new WaitForSeconds(.5F);
        if (blueLives <= 0)
            SceneManager.LoadScene(4);
        else
            SceneManager.LoadScene(5);
    }
}
