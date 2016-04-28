using UnityEngine;
using System.Collections;

public class DestroyOnTime : MonoBehaviour {
    
	void Start () {
        Destroy(gameObject, 1f);
	}
}
