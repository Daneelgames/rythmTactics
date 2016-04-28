using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public enum UnitColor {Red, Blue};

public class CardController : MonoBehaviour {

    public enum UnitType {Soldier, Helicopter, Tank, Wall};
    [SerializeField]
    private UnitType card = UnitType.Soldier;
    
    [SerializeField]
    private UnitColor cardColor = UnitColor.Red;
    private int cost = 1;

    public GameObject[] units;
    public Sprite[] unitSprites;

    private SpriteRenderer currentSprite;
    [SerializeField]
    private SpriteRenderer backgroundSprite;
    private Text costText;
    private GameObject curUnit;

    private TurnManager gameManager;

    [SerializeField]
    private bool isActive = false;

    private Animator _animator;

    [SerializeField]
    private List<CellController> availableCells;
    [SerializeField]
    private bool moving = false;

    private float mouseUpCooldown = .2f;
    private Vector2 originalPosition;

    Vector2 touchPos_1;
    Vector2 touchPos_2;

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

        transform.position = originalPosition;

        int random = Random.Range(0, 4);

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

            case 3:
                card = UnitType.Wall ;
                currentSprite.sprite = unitSprites[3];
                cost = 3;
                curUnit = units[3];
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
        MouseUp();
        SetActive();

        if (mouseUpCooldown > 0)
            mouseUpCooldown -= 1 * Time.deltaTime;


        if (Input.touchCount > 0)
        {
            touchPos_1 = Input.GetTouch(0).position;
            touchPos_1 = Camera.main.ScreenToWorldPoint(touchPos_1);

            if (Vector2.Distance(transform.position, (Vector3)touchPos_1) < 1.3)
            {
                MouseDown();
            }
        }
        if (Input.touchCount > 1)
        {
            touchPos_2 = Input.GetTouch(1).position;
            touchPos_2 = Camera.main.ScreenToWorldPoint(touchPos_2);

            if (Vector2.Distance(transform.position, (Vector3)touchPos_2) < 1.3)
            {
                MouseDown();
            }
        }
        /*
        Vector2 mousePos = Input.mousePosition;
        mousePos = new Vector3(mousePos.x, mousePos.y, 10f);
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        if (Vector2.Distance(transform.position, mousePos) < 1.5 && Input.GetMouseButtonDown(0))
            MouseDown();
        */
    }

    void SetActive()
    {
        if (cardColor == UnitColor.Red)
        {
            if (cost <= gameManager.redMana)
            {
                isActive = true;
                _animator.SetBool("Active", true);
            }
            else
            {
                isActive = false;
                _animator.SetBool("Active", false);
                backgroundSprite.color = Color.black;
            }

            if (gameManager.selectedCardRed == this)
            {
                moving = true;
                backgroundSprite.color = Color.green;
            }
            else
            {
                moving = false;
                backgroundSprite.color = Color.black;
            }
        }

        if (cardColor == UnitColor.Blue)
        {
            if (cost <= gameManager.blueMana)
            {
                isActive = true;
                _animator.SetBool("Active", true);
            }
            else
            {
                isActive = false;
                _animator.SetBool("Active", false);
                backgroundSprite.color = Color.black;
            }

            if (gameManager.selectedCardBlue == this)
            {
                moving = true;
                backgroundSprite.color = Color.green;
            }
            else
            {
                moving = false;
                backgroundSprite.color = Color.black;
            }
        }
    }
    
    void MouseDown()
    {
        if (isActive && !moving)
        {
            availableCells = GameObject.FindGameObjectsWithTag("Cell").Select(o => o.GetComponent<CellController>()).ToList();
            moving = true;

            mouseUpCooldown = 0.1f;

            if (cardColor == UnitColor.Red)
                gameManager.selectedCardRed = this;
            else
                gameManager.selectedCardBlue = this;
            
            if (cardColor == UnitColor.Red)
            {
                for (int i = availableCells.Count - 1; i >= 0; i--)
                {
                    if (availableCells[i].transform.position.y > .5f)
                        availableCells.Remove(availableCells[i]);
                }
            }
            else if (cardColor == UnitColor.Blue)
            {

                for (int i = availableCells.Count - 1; i >= 0; i--)
                {
                    if (availableCells[i].transform.position.y < 2f)
                        availableCells.Remove(availableCells[i]);
                }
            }

            for (int i = availableCells.Count -1; i >= 0; i--)
            {
                GameObject[] allies;
                if (cardColor == UnitColor.Red)
                    allies = GameObject.FindGameObjectsWithTag("UnitRed");
                else
                    allies = GameObject.FindGameObjectsWithTag("UnitBlue");

                foreach (GameObject ally in allies)
                {
                    if (Vector2.Distance(availableCells[i].gameObject.transform.position, ally.transform.position) < 0.5f)
                    {
                        availableCells[i].ReturnColor();
                        availableCells.RemoveAt(i);
                        break;
                    }
                        
                }
                /* if (availableCells.Count == i)
                    availableCells[i].ShowColor(); */
            }
            foreach (CellController cell in availableCells)
            {
                cell.ShowColor();
            }
        }
        //if moving
        else if (isActive && moving)
        {
            moving = false;
            if (cardColor == UnitColor.Red)
                gameManager.selectedCardRed = null;
            else
                gameManager.selectedCardBlue = null;

            backgroundSprite.color = Color.black;
            foreach (CellController cell in availableCells)
            {
                cell.ReturnColor();
            }
        }
    }
    
    void MouseUp()
    {
        if (Input.GetMouseButtonUp(0) && moving && isActive && mouseUpCooldown <= 0)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            
            GameObject closestCell = null;
            foreach (CellController cell in availableCells)
            {

                if (Vector2.Distance(mousePos, cell.gameObject.transform.position) < 0.5)
                    closestCell = cell.gameObject;
            }

            if (closestCell != null)
                BuyUnit(closestCell.transform.position);
            else
            {
                GameObject closestAllie = null;
                GameObject[] alliesCards;

                if (cardColor == UnitColor.Red)
                    alliesCards = GameObject.FindGameObjectsWithTag("UnitRed");
                else
                    alliesCards = GameObject.FindGameObjectsWithTag("UnitBlue");

                if (Vector2.Distance(mousePos, gameObject.transform.position) > 0.5)
                {
                    foreach (GameObject card in alliesCards)
                    {
                        if (Vector2.Distance(mousePos, card.transform.position) < 0.5)
                        {
                            closestAllie = card;
                        }
                    }
                }
              /*  if (closestAllie == null)
                {
                    moving = false;
                    if (cardColor == UnitColor.Red)
                        gameManager.selectedCardRed = null;
                    else
                        gameManager.selectedCardBlue = null;

                    backgroundSprite.color = Color.black;
                    foreach (CellController cell in availableCells)
                    {
                        cell.ReturnColor();
                    }
                } */
            }
        }
    }

    void BuyUnit(Vector2 spawnPosition)
    {
        print("buy unit");

        if (cardColor == UnitColor.Blue)
        {
            gameManager.blueMana -= cost;
            gameManager.selectedCardBlue = null;
        }
        else
        {
            gameManager.redMana -= cost;
            gameManager.selectedCardRed = null;
        }
        
        moving = false;
        backgroundSprite.color = Color.black;

        foreach (CellController cell in availableCells)
        {
            cell.ReturnColor();
        }

        Instantiate(curUnit, spawnPosition, Quaternion.Euler(0,0,0));

        ChooseUnit();
    }
}
