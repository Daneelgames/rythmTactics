using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TurnManager : MonoBehaviour {
    
    static TurnManager _instance;
    
    public static TurnManager instance {
        get {
            return _instance;
        }
    }

    public int redMana = 1;
    public int blueMana = 1;

    public int redLives = 3;
    public int blueLives = 3;

    [SerializeField]
    private float timeMax = 1f;
    private float timeCur = 1f;

    public CardController selectedCardBlue;
    public CardController selectedCardRed;

    [SerializeField]
    private GameObject redBase;
    [SerializeField]
    private GameObject blueBase;

    public int redUnits;
    public int blueUnits;

    void Awake(){
        if (_instance == null){
            _instance = this;
        } else if (_instance != this){
            Destroy(gameObject);
        }
    }

    void Start()
    {
        timeCur = timeMax;
    }

    void Update()
    {
        if (timeCur > 0)
            timeCur -= 1 * Time.deltaTime;
        else
        {
            if (redMana < 10)
                redMana += 1;
            if (blueMana < 10)
                blueMana += 1;

            timeCur = timeMax;
        }

        if (redBase == null)
            SceneManager.LoadScene(3);
        else if (blueBase == null)
            SceneManager.LoadScene(2);
    }
}
