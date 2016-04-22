using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum UnitColor { Red, Blue };

public class UnitController : MonoBehaviour {

    public UnitColor unitColor = UnitColor.Blue;
    public int damage = 1;

    public bool unitMoved = false;

    public List<GameObject> enemyInRange;
    
    private TurnManager turnManager;
    private Animator _animator;


    [SerializeField]
    private bool canMove = false;
    [SerializeField]
    private bool moving = false;

    [HideInInspector]
    public Vector2 pos;

    [SerializeField]
    private GameObject[] tilesAround;


    [SerializeField]
    private Vector2 lastMousePos;
    [SerializeField]
    private Vector2 curMousPos;

    private bool newPositionSet = false;
    
    public GameObject shadow;

    GameObject closestDragTile = null;
    float minimumDragDistance = 100f;
   
    /*
    private bool _flag = false;
    private bool flag
    {
        get
        {
            return _flag;
        }
        set
        {
            _flag = value;

        }
    }*/

    void Start()
    {
        enemyInRange = new List<GameObject>();
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
        ClearEnemies();
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

        if (unitColor == UnitColor.Red)
        { //ЗДЕСЬ НАДО УБИРАТЬ ДОСТУПНЫЕ ТАЙЛЫ ДЛЯ ДВИЖЕНИЯ ЕЖЕЛИ НА НИХ СТОИТ ВРАГ
            RaycastHit2D hitUp = Physics2D.Raycast(pos, Vector2.up, 1.5f, 1 << 2);
            RaycastHit2D hitRight = Physics2D.Raycast(pos, Vector2.right, 1.5f, 1 << 2);
            RaycastHit2D hitLeft = Physics2D.Raycast(pos, Vector2.left, 1.5f, 1 << 2);

            GameObject[] enemyList = GameObject.FindGameObjectsWithTag("UnitBlue");
            Debug.Log("Objects: " + string.Join(",", enemyList.Select(o => o.ToString()).ToArray()));

            if (hitUp.collider != null && hitUp.collider.tag == "Cell")
            {
                foreach (GameObject j in enemyList)
                {
                    if (Vector2.Distance(hitUp.collider.gameObject.transform.position, j.transform.position) > 1)
                    {
                        tilesAround[0] = hitUp.collider.gameObject;
                    }
                }
            }
            if (hitRight.collider != null && hitRight.collider.tag == "Cell")
            {
                foreach (GameObject j in enemyList)
                {
                    if (Vector2.Distance(hitRight.collider.gameObject.transform.position, j.transform.position) > 1)
                    {
                        tilesAround[1] = hitRight.collider.gameObject;
                    }
                }
            }
            if (hitLeft.collider != null && hitLeft.collider.tag == "Cell")
            {
                foreach (GameObject j in enemyList)
                {
                    if (Vector2.Distance(hitLeft.collider.gameObject.transform.position, j.transform.position) > 1)
                    {
                        tilesAround[2] = hitLeft.collider.gameObject;
                    }
                }
            }

            GameObject[] tiles = GameObject.FindGameObjectsWithTag("Cell");
            foreach (GameObject i in tiles)
            {
                if (Vector2.Distance(transform.position, i.transform.position) < 0.1f)
                    tilesAround[3] = i;
            }
        }
        else
        {
            RaycastHit2D hitRight = Physics2D.Raycast(pos, Vector2.right, 1.5f, 1 << 2);
            RaycastHit2D hitDown = Physics2D.Raycast(pos, Vector2.down, 1.5f, 1 << 2);
            RaycastHit2D hitLeft = Physics2D.Raycast(pos, Vector2.left, 1.5f, 1 << 2);
            
            GameObject[] enemyList = GameObject.FindGameObjectsWithTag("UnitRed");
            Debug.Log("Objects: " + string.Join(",", enemyList.Select(o => o.ToString()).ToArray()));

            if (hitRight.collider != null && hitRight.collider.tag == "Cell")
            {
                foreach (GameObject j in enemyList)
                {
                    if (Vector2.Distance(hitRight.collider.gameObject.transform.position, j.transform.position) > 1)
                    {
                        tilesAround[0] = hitRight.collider.gameObject;
                    }
                }
            }
            if (hitDown.collider != null && hitDown.collider.tag == "Cell")
            {
                foreach (GameObject j in enemyList)
                {
                    if (Vector2.Distance(hitDown.collider.gameObject.transform.position, j.transform.position) > 1)
                    {
                        tilesAround[1] = hitDown.collider.gameObject;
                    }
                }
            }
            if (hitLeft.collider != null && hitLeft.collider.tag == "Cell")
            {
                foreach (GameObject j in enemyList)
                {
                    if (Vector2.Distance(hitLeft.collider.gameObject.transform.position, j.transform.position) > 1)
                    {
                        tilesAround[2] = hitLeft.collider.gameObject;
                    }
                }
            }
            GameObject[] tiles = GameObject.FindGameObjectsWithTag("Cell");
            foreach (GameObject i in tiles)
            {
                if (Vector2.Distance(transform.position, i.transform.position) < 0.1f)
                    tilesAround[3] = i;
            }
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
            //MoveToClosestTile();
            //MoveToNewTile();

           if (enemyInRange.Count(e => e != null) == 0)
                MoveForward();
            else if (enemyInRange.Count > 0)
                AttackEnemy();

            GetTiles();

        }
    }

    void OnMouseDown()
    {
        if (enemyInRange.Count == 0 && canMove && !unitMoved && !turnManager.playerMoved)
        {
            moving = true;
            lastMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            foreach (GameObject i in tilesAround)
            {
                if (i != null)
                    i.GetComponent<CellController>().ShowColor();
            }

            closestDragTile = null;
            minimumDragDistance = 100f;
        }
        else if (enemyInRange.Count > 0 && !unitMoved && !turnManager.playerMoved)
        {
            print("click on target");
            AttackEnemy();
            turnManager.playerMoved = true;
        }
    }

    void OnMouseDrag()
    {
        if (canMove && moving)
        {
            Vector2 mousePosision = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 objPosition = Camera.main.ScreenToWorldPoint(mousePosision);

            curMousPos = objPosition;


            foreach (GameObject i in tilesAround)
            {
                if (i != null && Vector2.Distance(objPosition, i.transform.position) < 1)
                {
                    closestDragTile = i;
                    //minimumDragDistance = Vector2.Distance(objPosition, i.transform.position);
                }
            }

            transform.position = new Vector3(closestDragTile.transform.position.x, closestDragTile.transform.position.y, 0);

            /*if (Vector2.Distance(lastMousePos, curMousPos) > 0.75f && !newPositionSet)
            {
                newPositionSet = true;
                MoveToNewTile();
            }
            
            if (tilesAround[0] != null && Vector2.Distance(tilesAround[0].transform.position, curMousPos) < 0.25f && !newPositionSet) {

                newPositionSet = true;
                MoveToNewTile();
            }
            else if (tilesAround[1] != null && Vector2.Distance(tilesAround[1].transform.position, curMousPos) < 0.25f && !newPositionSet)
            {

                newPositionSet = true;
                MoveToNewTile();
            }
            else if (tilesAround[2] != null && Vector2.Distance(tilesAround[2].transform.position, curMousPos) < 0.25f && !newPositionSet)
            {

                newPositionSet = true;
                MoveToNewTile();
            }*/
        }
    }

    void OnMouseUp()
    {
        if (enemyInRange.Count == 0 && canMove && moving)
        {
            //moving = false;

            //MoveToClosestTile();
            MoveToNewTile();

            GetTiles();
            turnManager.playerMoved = true;
        }
    }
    /*
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
    }*/

    void MoveToNewTile()
    {
        if (moving)
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

            //move other unit
            GameObject[] units = GameObject.FindGameObjectsWithTag(gameObject.tag);
            GameObject closestUnit = null;
            float distance = 0.5f;
            foreach (GameObject i in units)
            {
                if (i != gameObject && Vector2.Distance(transform.position, i.transform.position) < distance)
                {
                    closestUnit = i;
                    distance = Vector2.Distance(transform.position, i.transform.position);
                }
            }
            if (closestUnit != null)
            {
                GameObject anotherShadow = closestUnit.GetComponent<UnitController>().shadow;
                Instantiate(anotherShadow, closestUnit.gameObject.transform.position, closestUnit.gameObject.transform.rotation);
                closestUnit.transform.position = pos;
                closestUnit.GetComponent<UnitController>().pos = closestUnit.transform.position;
            }

            //move unit
            Instantiate(shadow, transform.position, transform.rotation);
            transform.position = new Vector3(closestTile.transform.position.x, closestTile.transform.position.y, 0);


            canMove = false;
            newPositionSet = false;
            //MoveToClosestTile();
            GetTiles();
            unitMoved = true;
        }
    }

    void MoveForward()
    {
        if (unitColor == UnitColor.Red)
        {
            RaycastHit2D hitUp = Physics2D.Raycast(pos, Vector2.up, 1.5f, 1 << 2);
            RaycastHit2D hitUpUnit = Physics2D.Raycast(pos, Vector2.up, 1.5f, 1 << 9);
            if (hitUp.collider != null)
            {
                if (!hitUpUnit || hitUpUnit.collider.tag == gameObject.tag)
                {
                    Instantiate(shadow, transform.position, transform.rotation);
                    transform.position = hitUp.collider.gameObject.transform.position;
                    _animator.SetTrigger("MoveForward");
                }
            }
        }
        else if (unitColor == UnitColor.Blue)
        {
            RaycastHit2D hitDown = Physics2D.Raycast(pos, Vector2.down, 1.5f, 1 << 2);
            RaycastHit2D hitDownUnit = Physics2D.Raycast(pos, Vector2.down, 1.5f, 1 << 9);
            if (hitDown.collider != null)
            {
                if (!hitDownUnit || hitDownUnit.collider.tag == gameObject.tag)
                {
                    Instantiate(shadow, transform.position, transform.rotation);
                    transform.position = hitDown.collider.gameObject.transform.position;
                    _animator.SetTrigger("MoveForward");
                }
            }
        }
    }

    void AttackEnemy()
    {
        print("ATTACK ENEMY");
        _animator.SetTrigger("Attack");
        foreach (GameObject enemy in enemyInRange)
        {
            enemy.GetComponent<UnitHealth>().Damage(damage);
        }
    }

    void ClearEnemies()
    {
        for (int i = enemyInRange.Count - 1; i >= 0; i--)
        {
            if (enemyInRange[i] == null)
                enemyInRange.RemoveAt(i);
        }
    }
}