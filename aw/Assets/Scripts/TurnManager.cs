using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TurnManager : MonoBehaviour {
    

    public int redMana = 1;
    public int blueMana = 1;

    public int redLives = 3;
    public int blueLives = 3;

    [SerializeField]
    private float timeMax = 1f;
    private float timeCur = 1f;

    public CardController selectedCardBlue;
    public CardController selectedCardRed;

    void Start()
    {
        timeCur = timeMax;
    }

    void Update()
    {
        if (timeCur > 0)
            timeCur -= 1 * Time.deltaTime;
        else
        {
            if (redMana < 5)
                redMana += 1;
            if (blueMana < 5)
                blueMana += 1;

            timeCur = timeMax;
        }
    }
}
