using UnityEngine;
using System.Collections;


public enum GameState {MoveRed, MoveBlue};

public class TurnManager : MonoBehaviour {
    
    public GameState gameState = GameState.MoveRed;

    [SerializeField]
    private float turnTime = 1f;
    private float curTime;
    

    void Start()
    {
        curTime = turnTime;
    }

    void Update()
    {
        if (curTime > 0)
            curTime -= 1 * Time.deltaTime;
        else
        {
            if (gameState == GameState.MoveRed)
                gameState = GameState.MoveBlue;
            else
                gameState = GameState.MoveRed;

            curTime = turnTime;
        }
    }
}
