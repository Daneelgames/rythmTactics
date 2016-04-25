using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AttackTargetController : MonoBehaviour {

    public List<GameObject> target = new List<GameObject>();

    private SpriteRenderer _sprite;
    [SerializeField]
    private UnitController unit;

    [SerializeField]
    private GameObject child;

    private bool haveTarget = false;

    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _sprite.enabled = false;
        child.SetActive(false);

    }

	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "UnitBlue" && unit.unitColor == UnitColor.Red)
        {
            haveTarget = true;
          //  print(unit.gameObject.name + " found " + other.gameObject.name);
            unit.enemyInRange.Add(other.gameObject);
            target.Add (other.gameObject);
            _sprite.enabled = true;
            child.SetActive(true);
        }
            
        else if (other.tag == "UnitRed" && unit.unitColor == UnitColor.Blue)
        {
            haveTarget = true;
           // print(unit.gameObject.name + " found " + other.gameObject.name);
            unit.enemyInRange.Add(other.gameObject);
            target.Add(other.gameObject);
            _sprite.enabled = true;
            child.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "UnitBlue" || other.tag == "UnitRed")
        {
            unit.enemyInRange.Clear();
            _sprite.enabled = false;
            child.SetActive(false);
            target.Clear();
        }
    }

    void Update()
    {
        if (haveTarget && unit.enemyInRange.Count(e => e != null) == 0)
        {
            haveTarget = false;
            _sprite.enabled = false;
            child.SetActive(false);
            /* unit.enemyInRange.Clear();
             print("clear " + unit.gameObject.name); */

        }
    }
}