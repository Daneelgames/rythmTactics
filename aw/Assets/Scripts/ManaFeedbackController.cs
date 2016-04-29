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

        for (int i = 1; i < coins.Length + 1; i ++)
        {
            if (manaAmount < i)
                coins[i - 1].color = Color.black;
            else
                coins[i - 1].color = Color.white;
        }
    }
}
