using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {

    public UnitColor bulletColor = UnitColor.Red;

    public Vector2 targetPosition;
    [SerializeField]
    private float speed = 1f;
    private Vector2 origPos;
    
    public int damage = 1;


    void Start()
    {
        origPos = transform.position;
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if ((Vector2) transform.position == targetPosition)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        HealthController collController = coll.gameObject.GetComponent<HealthController>() as HealthController;
        if (collController.unitColor != bulletColor)
            if (coll.tag == "UnitRed" || coll.tag == "UnitBlue")
                Damage(collController);

        if (coll.tag == "Bumper")
            Destroy(gameObject);
    }

    void Damage(HealthController enemy)
    {
        enemy.Damage(damage);
        Destroy(gameObject);
    }
}