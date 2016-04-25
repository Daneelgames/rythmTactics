using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GoToGame : MonoBehaviour {

    private float coolDown = 1f;

    void Start()
    {
        Screen.SetResolution(486, 864, false);
    }

    void Update()
    {
        if (coolDown > 0)
            coolDown -= 1 * Time.deltaTime;

        if (coolDown <= 0 && Input.GetKey("1"))
            SceneManager.LoadScene(1);
        if (coolDown <= 0 && Input.GetKey("2"))
            SceneManager.LoadScene(2);
        if (coolDown <= 0 && Input.GetKey("3"))
            SceneManager.LoadScene(3);
    }
    	
}
