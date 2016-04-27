using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UnitController : MonoBehaviour {

    [SerializeField]
    private UnitColor cardColor = UnitColor.Red;
    private enum UnitState {Aim, Battle};

    public int health = 1;
    public float attackTime = 1;
    public int attackDmg = 1;

    public GameObject weapon;

    /*
    public float rangeAngle = 15;
    public float rangeLength = 1;
    */

    private Vector2 shootDirection;

    private float rangeCooldown = 0.1f;
    private UnitState state = UnitState.Battle;
    
    [SerializeField]
    private GameObject range;
    private Collider2D rangeCollider;
    [SerializeField]
    private List<GameObject> enemiesInRange;

    void Start () {
        if (cardColor == UnitColor.Red)
            range.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
        else
            range.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 270));

        rangeCollider = range.GetComponent<Collider2D>();

        InvokeRepeating("Shoot", attackTime, attackTime);
    }

    void Update()
    {
        if (rangeCooldown > 0)
            rangeCooldown -= 1 * Time.deltaTime;

        if (Input.GetMouseButton(0) && state == UnitState.Aim && rangeCooldown <= 0)
        {
            SetRangeAngle();
        }

        if (Input.GetMouseButtonUp(0) && state == UnitState.Aim && rangeCooldown <= 0)
        {
            state = UnitState.Battle;
        }

        if (state == UnitState.Battle)
        {
            Battle();
        }


    }

    void OnMouseDown()
    {
        state = UnitState.Aim;
    }

    void SetRangeAngle()
    {
        if (!range.GetComponent<SpriteRenderer>().enabled)
            range.GetComponent<SpriteRenderer>().enabled = true;

        Vector2 mousePos = Input.mousePosition;
        mousePos = new Vector3(mousePos.x, mousePos.y, 10f);
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        range.transform.rotation = Quaternion.Euler(new Vector3(0,0, Mathf.Atan2((mousePos.y - range.transform.position.y), (mousePos.x - range.transform.position.x)) * Mathf.Rad2Deg));
    }

    void Battle()
    {
        if (range.GetComponent<SpriteRenderer>().enabled)
            range.GetComponent<SpriteRenderer>().enabled = false;

    }


    
    void Shoot()
    {
        GameObject[] enemies;
        if (cardColor == UnitColor.Red)
             enemies = GameObject.FindGameObjectsWithTag("UnitBlue");
        else
            enemies = GameObject.FindGameObjectsWithTag("UnitRed");

        enemiesInRange = enemies.Where(e => e.GetComponent<Collider2D>().IsTouching(rangeCollider)).ToList();

        float maximumDistance = 10f;
        GameObject target = null;

        foreach (GameObject enemy in enemiesInRange)
        {
            if (Vector2.Distance(gameObject.transform.position, enemy.transform.position) < maximumDistance)
            {
                maximumDistance = Vector2.Distance(gameObject.transform.position, enemy.transform.position);
                target = enemy;
            }
        }

        if (target != null)
        {
            GameObject projectile = Instantiate(weapon, transform.position, transform.rotation) as GameObject;
            projectile.GetComponent<WeaponController>().targetPosition = target.transform.position;
        }
    }
}