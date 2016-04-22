using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AttackRange: MonoBehaviour {
    
    private UnitController unitController;
    
    private UnitColor unitColor;
    private TurnManager turnScript;
    
    private List<GameObject> targetsRed = new List<GameObject>();
    private List<GameObject> targetsBlue = new List<GameObject>();
    

    void Start()
    {
        unitController = GetComponent<UnitController>();
        unitColor = unitController.unitColor;
        turnScript = GameObject.Find("BattleManager").GetComponent<TurnManager>();
        
        targetsRed = GetComponentsInChildren<AttackTargetController>(true)
            .Select(c => c.gameObject)
            .Where(go => go.tag == "TargetRed")
            .ToList();

        targetsBlue = GetComponentsInChildren<AttackTargetController>(true)
            .Select(c => c.gameObject)
            .Where(go => go.tag == "TargetBlue")
            .ToList();

        if (unitColor == UnitColor.Red)
        {
            foreach (GameObject target in targetsBlue)
            {
                target.SetActive(false);
            }

            foreach (GameObject target in targetsRed)
            {
                target.SetActive(true);
            }
        }
        else if (unitColor == UnitColor.Blue)
        {
            foreach (GameObject target in targetsRed)
            {
                target.SetActive(false);
            }

            foreach (GameObject target in targetsBlue)
            {
                target.SetActive(true);
            }
        }
    }

    void Update ()
    {
        if (turnScript.gameState == GameState.MoveRed)
            FindEnemyInRangeRed();
        else if (turnScript.gameState == GameState.MoveBlue)
            FindEnemyInRangeBlue();
    }
    
    void FindEnemyInRangeRed()
    {
        foreach (GameObject target in targetsRed)
        {
            target.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
        }

        foreach (GameObject target in targetsBlue)
        {
            target.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 0);
        }
    }

    void FindEnemyInRangeBlue()
    {
        foreach (GameObject target in targetsBlue)
        {
            target.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 1);
        }

        foreach (GameObject target in targetsRed)
        {
            target.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0);
        }
    }
}