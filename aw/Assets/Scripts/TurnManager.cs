using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public enum GameState {MoveRed, MoveBlue};

public class TurnManager : MonoBehaviour {
    
    public GameState gameState = GameState.MoveRed;
    public bool playerMoved = false;
    
    [SerializeField]
    private float turnTime = 1f;
    private float curTime;

    private bool canSetRedUnitMovedFalse = true;
    private bool canSetBlueUnitMovedFalse = true;

    //timer feedback

    [SerializeField]
    private Image content;

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
    }

    void HandleTimer()
    {
        curTimer = curTime;
        content.fillAmount = Map(curTimer, minTimer, maxTimer, minFill, maxFill);
    }

    float Map(float cur, float inMin, float inMax, float outMin, float outMax)
    {
        return (cur - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    void Update()
    {
        HandleTimer();

        if (curTime > 0)
            curTime -= 1 * Time.deltaTime;
        else
        {
            if (gameState == GameState.MoveRed && canSetBlueUnitMovedFalse)
            {
                playerMoved = false;
                canSetBlueUnitMovedFalse = false;
                canSetRedUnitMovedFalse = true;
                gameState = GameState.MoveBlue;
                GameObject[] blues = GameObject.FindGameObjectsWithTag("UnitBlue");
                foreach (GameObject i in blues)
                {
                    i.GetComponent<UnitController>().unitMoved = false;
                }
            }
            else if (gameState == GameState.MoveBlue && canSetRedUnitMovedFalse)
            {
                playerMoved = false;
                canSetRedUnitMovedFalse = false;
                canSetBlueUnitMovedFalse = true;
                gameState = GameState.MoveRed;
                GameObject[] blues = GameObject.FindGameObjectsWithTag("UnitRed");
                foreach (GameObject i in blues)
                {
                    i.GetComponent<UnitController>().unitMoved = false;
                }

            }

            curTime = turnTime;
        }
    }
}
