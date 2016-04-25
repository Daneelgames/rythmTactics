using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HeartsController : MonoBehaviour {

    [SerializeField]
    private UnitColor _color = UnitColor.Red;

    [SerializeField]
    private Image[] hearts;

    private TurnManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("BattleManager").GetComponent<TurnManager>();
    }

    void Update()
    {
        if (_color == UnitColor.Red)
        {
            switch (gameManager.redLives)
            {
                case 3:
                    hearts[0].enabled = true;
                    hearts[1].enabled = true;
                    hearts[2].enabled = true;
                    break;

                case 2:
                    hearts[0].enabled = true;
                    hearts[1].enabled = true;
                    hearts[2].enabled = false;
                    break;

                case 1:
                    hearts[0].enabled = true;
                    hearts[1].enabled = false;
                    hearts[2].enabled = false;
                    break;

                case 0:
                    hearts[0].enabled = false;
                    hearts[1].enabled = false;
                    hearts[2].enabled = false;
                    break;

                default:
                    hearts[0].enabled = false;
                    hearts[1].enabled = false;
                    hearts[2].enabled = false;
                    break;
            }
        }
        else
            switch (gameManager.blueLives)
            {
                case 3:
                    hearts[0].enabled = true;
                    hearts[1].enabled = true;
                    hearts[2].enabled = true;
                    break;

                case 2:
                    hearts[0].enabled = true;
                    hearts[1].enabled = true;
                    hearts[2].enabled = false;
                    break;

                case 1:
                    hearts[0].enabled = true;
                    hearts[1].enabled = false;
                    hearts[2].enabled = false;
                    break;

                case 0:
                    hearts[0].enabled = false;
                    hearts[1].enabled = false;
                    hearts[2].enabled = false;
                    break;

                default:
                    hearts[0].enabled = false;
                    hearts[1].enabled = false;
                    hearts[2].enabled = false;
                    break;
            }
    }
}
