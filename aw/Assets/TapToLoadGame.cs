using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TapToLoadGame : MonoBehaviour {

    private float coolDown = 1f;
	
	// Update is called once per frame
	void Update () {
        if (coolDown > 0)
            coolDown -= 1 * Time.deltaTime;
        else if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(1);
        }
	}
}
