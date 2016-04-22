using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UnitHealth : MonoBehaviour {

    [SerializeField]
    private int health = 1;

    [SerializeField]
    private GameObject explosion;
    [SerializeField]
    private GameObject damageParticles;

    private Animator _animator;
    private Text healthDisplay;

    void Start()
    {
        healthDisplay = GetComponentInChildren<Text>();
        _animator = GetComponentInChildren<Animator>();
    }

    public void Damage(int dmg)
    {
        health -= dmg;
        Instantiate(damageParticles, transform.position, transform.rotation);
        _animator.SetTrigger("Damage");
        if (health <= 0)
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    void Update()
    {
        healthDisplay.text = "" + health;
    }
}
