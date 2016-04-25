using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ManaFeedbackController : MonoBehaviour {

    [SerializeField]
    private UnitColor color = UnitColor.Red;
    [SerializeField]
    private Image[] coins;

    private TurnManager gameManager;

    private int manaAmount;

    void Start()
    {
        gameManager = GameObject.Find("BattleManager").GetComponent<TurnManager>();
    }


    void Update()
    {
        if (color == UnitColor.Blue)
            manaAmount = gameManager.blueMana;
        else
            manaAmount = gameManager.redMana;

        switch (manaAmount)
        {
            case 0:
                coins[0].color = Color.black;
                coins[1].color = Color.black;
                coins[2].color = Color.black;
                coins[3].color = Color.black;
                coins[4].color = Color.black;
                break;
                
            case 1:
                coins[0].color = Color.white;
                coins[1].color = Color.black;
                coins[2].color = Color.black;
                coins[3].color = Color.black;
                coins[4].color = Color.black;
                break;

            case 2:
                coins[0].color = Color.white;
                coins[1].color = Color.white;
                coins[2].color = Color.black;
                coins[3].color = Color.black;
                coins[4].color = Color.black;
                break;

            case 3:
                coins[0].color = Color.white;
                coins[1].color = Color.white;
                coins[2].color = Color.white;
                coins[3].color = Color.black;
                coins[4].color = Color.black;
                break;

            case 4:
                coins[0].color = Color.white;
                coins[1].color = Color.white;
                coins[2].color = Color.white;
                coins[3].color = Color.white;
                coins[4].color = Color.black;
                break;

            case 5:
                coins[0].color = Color.white;
                coins[1].color = Color.white;
                coins[2].color = Color.white;
                coins[3].color = Color.white;
                coins[4].color = Color.white;
                break;
        }
    }
}
