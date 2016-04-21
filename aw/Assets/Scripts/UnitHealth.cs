using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UnitHealth : MonoBehaviour {

    [SerializeField]
    private int health = 1;

    private Text healthDisplay;

    void Start()
    {
        healthDisplay = GetComponentInChildren<Text>();
    }

    public void Damage(int dmg)
    {
        health -= dmg;
        if (health <= 0)
            Destroy(gameObject);
    }

    void Update()
    {
        healthDisplay.text = "" + health;
    }
}
