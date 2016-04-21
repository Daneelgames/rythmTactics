using UnityEngine;
using System.Collections;

public class ShadowController : MonoBehaviour {

    [SerializeField]
    private float destroyTime = 0.5f;

    private SpriteRenderer sprite;
    private float opacity = 1f;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        opacity -= 1 * Time.deltaTime / destroyTime;
        sprite.color = new Color(1, 1, 1, opacity);
    }
}
