using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthController : MonoBehaviour {

    public UnitColor unitColor = UnitColor.Red;

    [SerializeField]
    private float timeToSuddenDeath = 5;

    [SerializeField]
    private GameObject explosion;
    
    private Image content;

    public float maxHealth = 10;
    [HideInInspector]
    public float curHealth = 10;

    private float minHealth = 0;

    private Animator _animator;

    private float minFill = 0f;
    private float maxFill = 1f;

    private bool dying = false;

    private TurnManager gameManager;

    [SerializeField]
    private bool isBase = false;

    void Awake()
    {
        gameManager = GameObject.Find("BattleManager").GetComponent<TurnManager>();
        if (unitColor == UnitColor.Red)
            gameManager.redUnits += 1;
        else
            gameManager.blueUnits += 1;

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

        if (timeToSuddenDeath > 0)
            timeToSuddenDeath -= 1 * Time.deltaTime;
        else
            dying = true;

        if (dying && unitColor == UnitColor.Red)
            curHealth -= 1 * Time.deltaTime + gameManager.redUnits / 10;
        else if (dying && unitColor == UnitColor.Blue)
            curHealth -= 1 * Time.deltaTime + gameManager.blueUnits / 10;

        if (curHealth <= 0)
                DestroyUnit();
    }

    private void HandleBar()
    {
        content.fillAmount = Map(curHealth, minHealth, maxHealth, minFill, maxFill);
    }

    private float Map(float curHealth, float inMin, float inMax, float outMin, float outMax)
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
        
        if (unitColor == UnitColor.Red)
            gameManager.redUnits -= 1;
        else
            gameManager.blueUnits -= 1;

        Destroy(gameObject);
    }
}
