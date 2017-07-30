using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gm = null;

    private void OnEnable()
    {
        if (gm == null)
            gm = this;
    }

    private GameManager() { } // private to force singleton

    /******************
     * RUB A DUB PUBS *
     ******************/
    [Header("Do not destroys")]
    public GameObject go_MainTower;
    public GameObject go_EventSystem;
    public GameObject cam;
    public MainTower MainTower;

    [Header("Game State")]
    public int
        curPower,
        maxPower;

    void Start ()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(go_MainTower);
        DontDestroyOnLoad(go_EventSystem);
        DontDestroyOnLoad(cam);

        curPower = maxPower;
        
        SceneManager.LoadScene("Level01");
    }
	
	void Update ()
    {
		
	}

    public void addPower(int power)
    {
        power = Mathf.Abs(power);
        curPower += power;
        if (curPower > maxPower)
            curPower = maxPower;
    }

    public void addMaxPower(int amount)
    {
        amount = Mathf.Abs(amount);
        maxPower += amount;
        curPower += amount;
    }

    public void consumePower(int amount)
    {
        amount = Mathf.Abs(amount);
        curPower -= amount;
        if (curPower < 0)
            Debug.LogWarning("Current Power dropped below zero!");
    }
}
