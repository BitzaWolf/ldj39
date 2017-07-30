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
    public GameObject canvas;
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
        DontDestroyOnLoad(canvas);

        curPower = maxPower;
        
        SceneManager.LoadScene("Level01");
    }
	
	void Update ()
    {
		
	}

    public void addPower(int power)
    {
        power = Mathf.Abs(power);

        // If we're currently over cap somehow, we want to remain overcap (don't drop -down- to the cap).
        // but if we're under the cap then set us to the cap.
        if (power + curPower > maxPower)
        {
            if (curPower < maxPower)
                power = maxPower - curPower;
            else
                power = 0;
        }

        curPower += power;
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
