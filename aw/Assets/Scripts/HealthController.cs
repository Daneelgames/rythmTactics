using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthController : MonoBehaviour {

    public UnitColor unitColor = UnitColor.Red;
    
    [SerializeField]
    private GameObject explosion;
    
    private Image content;

    public int maxHealth = 10;
    [HideInInspector]
    public int curHealth = 10;

    private int minHealth = 0;

    private Animator _animator;

    private float minFill = 0f;
    private float maxFill = 1f;
    
    void Awake()
    {
        content = transform.Find("Canvas/HealthBack/Health").GetComponent<Image>();
    }

    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        curHealth = maxHealth;
    }

    // Use this for initialization
    void Update()
    {
        HandleBar();
    }

    private void HandleBar()
    {
        content.fillAmount = Map(curHealth, minHealth, maxHealth, minFill, maxFill);
    }

    private float Map(int curHealth, int inMin, int inMax, float outMin, float outMax)
    {
        return (curHealth * 1.0f - inMin) * (outMax - outMin) / (inMax * 1.0f - inMin * 1.0f) + outMin;
    }

    public void Damage(int damage)
    {
            if (curHealth > damage)
        {
            curHealth -= damage;

            if (_animator != null)
                _animator.SetTrigger("Damage");
        }
            else
                DestroyUnit();
    }

    public void DestroyUnit()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
