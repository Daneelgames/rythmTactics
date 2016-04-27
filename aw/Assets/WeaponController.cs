using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {

    public Vector2 targetPosition;
    [SerializeField]
    private float speed = 1f;
    private Vector2 origPos;


    void Start()
    {
        origPos = transform.position;
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }
}
