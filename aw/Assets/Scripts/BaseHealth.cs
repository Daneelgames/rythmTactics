using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BaseHealth : MonoBehaviour {

    [SerializeField]
    private Image content;
    
    public int maxBossHealth = 100;
    [HideInInspector]
    public int curBossHealth = 100;

    private int minBossHealth = 0;

    private float minFill = 0f;
    private float maxFill = 1f;

    void Start()
    {
        curBossHealth = maxBossHealth;
    }

    // Use this for initialization
    void Update()
    {
        HandleBar();
    }

    private void HandleBar()
    {
        content.fillAmount = Map(curBossHealth, minBossHealth, maxBossHealth, minFill, maxFill);
    }

    private float Map(int curHealth, int inMin, int inMax, float outMin, float outMax)
    {
        return (curHealth * 1.0f - inMin) * (outMax - outMin) / (inMax * 1.0f - inMin * 1.0f) + outMin;
    }

    public void Damage(int damage)
    {
        curBossHealth -= damage;
    }
}
