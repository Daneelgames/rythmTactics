using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Linq;

public class UnitController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    
    public UnitColor cardColor = UnitColor.Red;
    private enum UnitState {Aim, Battle};
    
    public float attackTime = 1;
    public int attackDmg = 1;

    private Animator _animator;

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

    Vector2 touchPos;

    void Start () {
        if (cardColor == UnitColor.Red)
            range.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2((5 - range.transform.position.y), (0 - range.transform.position.x)) * Mathf.Rad2Deg));
        else
            range.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2((-2.5f - range.transform.position.y), (0 - range.transform.position.x)) * Mathf.Rad2Deg));

        rangeCollider = range.GetComponent<Collider2D>();
        _animator = GetComponentInChildren<Animator>();
            
        if (weapon != null)
            InvokeRepeating("Shoot", attackTime, attackTime);
        
        DisableAim();
            
    }

    void Update()
    {
        /*
        if (rangeCooldown > 0)
            rangeCooldown -= 1 * Time.deltaTime;

        if (state == UnitState.Aim)
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
        */

        /*
                Vector2 mousePos = Input.mousePosition;
                mousePos = new Vector3(mousePos.x, mousePos.y, 10f);
                mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        */
        
       
/*
        if (Vector2.Distance(transform.position, mousePos) < 1.5 && Input.GetMouseButtonDown(0))
            MouseDown();
*/
    }

    void MouseDown()
    {
        if (weapon != null && rangeCooldown <= 0)
            state = UnitState.Aim;
    }

    void EnableAim()
    {
        if (!range.GetComponent<SpriteRenderer>().enabled)
            range.GetComponent<SpriteRenderer>().enabled = true;
    }


    public void OnBeginDrag(PointerEventData data)
    {
        EnableAim();
    }

    public void OnEndDrag(PointerEventData data)
    {
        DisableAim();
    }

    public void OnDrag(PointerEventData data)
    {

        touchPos = data.position;
        touchPos = Camera.main.ScreenToWorldPoint(touchPos);
        print(touchPos);
        range.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2((range.transform.position.y - touchPos.y), (range.transform.position.x - touchPos.x)) * Mathf.Rad2Deg));
    }

    void DisableAim()
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

        enemiesInRange = enemies.Where(e => e.GetComponent<CircleCollider2D>().IsTouching(rangeCollider)).ToList();

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
            _animator.SetTrigger("Attack");
            GameObject projectile = Instantiate(weapon, transform.position, transform.rotation) as GameObject;
            WeaponController bulletController = projectile.GetComponent<WeaponController>();
            bulletController.targetPosition = target.transform.position;
            bulletController.damage = attackDmg;
            bulletController.bulletColor = cardColor;
        }
    }
}