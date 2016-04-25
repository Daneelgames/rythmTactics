using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class CardController : MonoBehaviour {

    public enum UnitType {Soldier, Helicopter, Tank};
    [SerializeField]
    private UnitType card = UnitType.Soldier;
    
    [SerializeField]
    private UnitColor cardColor = UnitColor.Red;
    private int cost = 1;

    public GameObject[] units;
    public Sprite[] unitSprites;

    private SpriteRenderer currentSprite;
    private Text costText;
    private GameObject curUnit;

    private TurnManager gameManager;

    private bool isActive = false;

    private Animator _animator;

    [SerializeField]
    private List<CellController> availableCells;
    private bool moving = false;

    private Vector2 originalPosition;

    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        gameManager = GameObject.Find("BattleManager").GetComponent<TurnManager>();

        currentSprite = GetComponentInChildren<SpriteRenderer>();
        costText = GetComponentInChildren<Text>();


        originalPosition = transform.position;

        ChooseUnit();

    }

    void ChooseUnit()
    {
        availableCells = GameObject.FindGameObjectsWithTag("Cell").Select(o => o.GetComponent<CellController>()).ToList();

        transform.position = originalPosition;

        int random = Random.Range(0, 3);

        switch (random)
        {
            case 0:
                card = UnitType.Soldier;
                currentSprite.sprite = unitSprites[0];
                cost = 1;
                curUnit = units[0];
                break;

            case 1:
                card = UnitType.Helicopter;
                currentSprite.sprite = unitSprites[1];
                cost = 2;
                curUnit = units[1];
                break;

            case 2:
                card = UnitType.Tank;
                currentSprite.sprite = unitSprites[2];
                cost = 3;
                curUnit = units[2];
                break;

            default:
                card = UnitType.Soldier;
                currentSprite.sprite = unitSprites[0];
                cost = 1;
                curUnit = units[0];
                break;
        }

        costText.text = "" + cost;
    }

    void Update()
    {
        if (cardColor == UnitColor.Red)
        {
            if (gameManager.gameState == GameState.MoveRed && cost <= gameManager.redMana && !gameManager.playerMoved)
            {
                isActive = true;
                _animator.SetBool("Active", true);

            }
            else
            {
                if (moving)
                {
                    BuyUnit();
                }
                isActive = false;
                _animator.SetBool("Active", false);
            }
        }

        if (cardColor == UnitColor.Blue)
        {
            if (gameManager.gameState == GameState.MoveBlue && cost <= gameManager.blueMana && !gameManager.playerMoved)
            {
                isActive = true;
                _animator.SetBool("Active", true);

            }
            else
            {
                if (moving)
                {
                    BuyUnit();
                }
                isActive = false;
                _animator.SetBool("Active", false);
            }
        }
    }
    
    void OnMouseDown()
    {
        if (isActive)
        {
            //spend mana
            if (cardColor == UnitColor.Red)
            {
                
                for (int i = availableCells.Count - 1; i >= 0; i--)
                {
                    if (availableCells[i].transform.position.y > -2)
                        availableCells.Remove(availableCells[i]);
                }
            }
            else if (cardColor == UnitColor.Blue)
            {

                for (int i = availableCells.Count - 1; i >= 0; i--)
                {
                    if (availableCells[i].transform.position.y < 4.5f)
                        availableCells.Remove(availableCells[i]);
                }
            }

            for (int i = availableCells.Count -1; i >= 0; i--)
            {
                GameObject[] allies;
                if (cardColor == UnitColor.Red)
                {
                    allies = GameObject.FindGameObjectsWithTag("UnitRed");
                }
                else
                {
                    allies = GameObject.FindGameObjectsWithTag("UnitBlue");
                }
                foreach (GameObject ally in allies)
                {
                    if (Vector2.Distance(availableCells[i].gameObject.transform.position, ally.transform.position) < 0.5f)
                    {
                        availableCells.Remove(availableCells[i]);
                    }
                }
                if (availableCells[i] != null)
                    availableCells[i].ShowColor();

                moving = true;
            }
        }
    }

    void OnMouseDrag()
    {
        if (isActive)
        {
            Vector2 mousePosision = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 objPosition = Camera.main.ScreenToWorldPoint(mousePosision);

            float minimumDistance = 2;
            foreach (CellController cell in availableCells)
            {
                float curCellDistance = Vector2.Distance(objPosition, cell.gameObject.transform.position);
                if (curCellDistance < minimumDistance)
                {
                    transform.position = cell.gameObject.transform.position;
                    minimumDistance = curCellDistance;
                }
            }
        }
    }

    void OnMouseUp()
    {
        if (moving && isActive)
        {
            BuyUnit();
        }
    }

    void BuyUnit()
    {
        if (cardColor == UnitColor.Blue)
            gameManager.blueMana -= cost;
        else
            gameManager.redMana -= cost;

        moving = false;
        isActive = false;
        gameManager.playerMoved = true;

        foreach (CellController cell in availableCells)
        {
            cell.ReturnColor();
        }

        Instantiate(curUnit, transform.position, Quaternion.Euler(0,0,0));

        ChooseUnit();
    }
}
