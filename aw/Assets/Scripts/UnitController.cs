using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitController : MonoBehaviour {

    public enum UnitColor {Red, Blue};
    public UnitColor unitColor = UnitColor.Blue;

    public bool unitMoved = false;

    [SerializeField]
    private float attackRange = 1;

    private TurnManager turnManager;
    private Animator _animator;


    [SerializeField]
    private bool canMove = false;
    [SerializeField]
    private bool moving = false;

    private Vector2 pos;

    [SerializeField]
    private GameObject[] tilesAround;


    [SerializeField]
    private Vector2 lastMousePos;
    [SerializeField]
    private Vector2 curMousPos;
    private bool newPositionSet = false;

    void Start()
    {
        turnManager = GameObject.Find("BattleManager").GetComponent<TurnManager>();
        _animator = GetComponentInChildren<Animator>();

        GetTiles();
    }
    
    void Update()
    {
        if (unitColor == UnitColor.Red)
        {
            if (turnManager.gameState == GameState.MoveRed)
                Move();
            else
                Stop();
        }
        else if (unitColor == UnitColor.Blue)
        {
            if (turnManager.gameState == GameState.MoveBlue)
                Move();
            else
                Stop();
        }
    }

    void GetTiles()
    {
        pos = transform.position;
        foreach (GameObject i in tilesAround)
        {
            if (i != null)
                i.GetComponent<CellController>().ReturnColor();
        }

        for (int i = 0; i < 4; i++)
        {
            if (tilesAround[i] != null)
                tilesAround[i] = null;
        }

        RaycastHit2D hitUp = Physics2D.Raycast(pos, Vector2.up, 1.5f, 1<<8);
        RaycastHit2D hitRight = Physics2D.Raycast(pos, Vector2.right, 1.5f, 1 << 8);
        RaycastHit2D hitDown = Physics2D.Raycast(pos, Vector2.down, 1.5f, 1 << 8);
        RaycastHit2D hitLeft = Physics2D.Raycast(pos, Vector2.left, 1.5f, 1 << 8);

        if (hitUp.collider != null)
        {
            tilesAround[0] = hitUp.collider.gameObject;
        }
        if (hitRight.collider != null)
        {
            tilesAround[1] = hitRight.collider.gameObject;
        }
        if (hitDown.collider != null)
        {
            tilesAround[2] = hitDown.collider.gameObject;
        }
        if (hitLeft.collider != null)
        {
            tilesAround[3] = hitLeft.collider.gameObject;
        }
        
    }

    void Move()
    {
        _animator.SetBool("Active", true);
        if (!canMove)
        {
            canMove = true;
        }
    }

    void Stop()
    {
        _animator.SetBool("Active", false);
        if (canMove)
        {
            canMove = false;
            newPositionSet = false;
            MoveToClosestTile();
            GetTiles();
        }
    }

    void OnMouseDown()
    {
        if (canMove && !unitMoved && !turnManager.playerMoved)
        {
            moving = true;
            lastMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            foreach (GameObject i in tilesAround)
            {
                if (i != null)
                    i.GetComponent<CellController>().ShowColor();
            }
        }
    }

    void OnMouseDrag()
    {
        if (canMove && moving)
        {
            Vector2 mousePosision = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 objPosition = Camera.main.ScreenToWorldPoint(mousePosision);

            curMousPos = objPosition;

            transform.position = new Vector3(objPosition.x, objPosition.y, 0);

            if (Vector2.Distance(lastMousePos, curMousPos) > 0.75f && !newPositionSet)
            {
                newPositionSet = true;
                MoveToNewTile();
            }

        }
    }

    void OnMouseUp()
    {
        if (canMove && moving)
        {
            moving = false;

            MoveToClosestTile();

            GetTiles();
            turnManager.playerMoved = true;
            print(turnManager.playerMoved);
        }
    }

    void MoveToClosestTile()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Cell");
        GameObject closestTile = null;
        float minimumDistance = 100f;
        foreach (GameObject i in tiles)
        {
            if (Vector2.Distance(transform.position, i.transform.position) < minimumDistance)
            {
                closestTile = i;
                minimumDistance = Vector2.Distance(transform.position, i.transform.position);
            }
        }
        transform.position = new Vector3(closestTile.transform.position.x, closestTile.transform.position.y, 0);
    }

    void MoveToNewTile()
    {
        moving = false;
        GameObject closestTile = null;
        float minimumDistance = 100f;

        foreach (GameObject i in tilesAround)
        {
            if (i != null && Vector2.Distance(transform.position, i.transform.position) < minimumDistance)
            {
                closestTile = i;
                minimumDistance = Vector2.Distance(transform.position, i.transform.position);
            }
        }

        transform.position = new Vector3(closestTile.transform.position.x, closestTile.transform.position.y, 0);

        GameObject[] units = GameObject.FindGameObjectsWithTag(gameObject.tag);
        GameObject closestUnit = null;
        float distance = 0.25f;
        foreach (GameObject i in units)
        {
            if (i != gameObject && Vector2.Distance(transform.position, i.transform.position) < distance)
            {
                closestUnit = i;
                distance = Vector2.Distance(transform.position, i.transform.position);
            }
        }
        if (closestUnit != null)
            closestUnit.transform.position = pos;

        canMove = false;
        newPositionSet = false;
        MoveToClosestTile();
        GetTiles();
        unitMoved = true;
        turnManager.playerMoved = true;
        print(turnManager.playerMoved);
    }
}