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

    public enum State
    {
        MOVING,
        PLACING_BUILDING,
        GAME_OVER
    }

    /******************
     * RUB A DUB PUBS *
     ******************/
    [Header("Do not destroys")]
    public GameObject go_MainTower;
    public GameObject go_EventSystem;
    public GameObject cam;
    public GameObject canvas;
    public GameObject gameOverCanvas;
    public InputManager input;
    public MainTower MainTower;

    [Header("Game State")]
    public State currentState = State.MOVING;
    public int
        curPower,
        maxPower;
    public GameObject placementBuilding = null;
    public float SpawnRate = 0.5f;

    [Header("Prefabs")]
    public GameObject TowerPrefab;
    public GameObject
        MinerPrefab,
        TowerPlacementPrefab,
        MinerPlacementPrefab,
        MainTowerPrefab;

    void Start ()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(go_MainTower);
        DontDestroyOnLoad(go_EventSystem);
        DontDestroyOnLoad(cam);
        DontDestroyOnLoad(canvas);
        DontDestroyOnLoad(gameOverCanvas);

        curPower = maxPower;
        MainTower.OnDeath += OnMainTowerDeath;
        
        SceneManager.LoadScene("Level01");
    }
	
	void Update ()
    {
        SpawnRate += 0.001f * Time.deltaTime;
        switch(currentState)
        {
            case State.MOVING: UpdateMoving(); break;
            case State.PLACING_BUILDING: UpdatePlacing(); break;
            case State.GAME_OVER: UpdateGameOver(); break;
        }
	}

    private void UpdateMoving()
    {
        if (Input.GetButtonDown("One"))
        {
            placementBuilding = Instantiate(TowerPlacementPrefab);
            currentState = State.PLACING_BUILDING;
        }
        else if (Input.GetButtonDown("Two"))
        {
            placementBuilding = Instantiate(MinerPlacementPrefab);
            currentState = State.PLACING_BUILDING;
        }
    }

    private void UpdatePlacing()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Destroy(placementBuilding);
            placementBuilding = null;
            currentState = State.MOVING;
            return;
        }

        // move placement building to the spot where the mouse is pointing
        // or let a plcement script handle it?!
    }
    private void UpdateGameOver()
    {
        if (Input.GetButton("LeftClick"))
        {
            curPower = maxPower;
            gameOverCanvas.SetActive(false);
            GameObject g = Instantiate(MainTowerPrefab);
            go_MainTower = g;
            MainTower = go_MainTower.GetComponentInChildren<MainTower>();
            MainTower.OnDeath += OnMainTowerDeath;
            DontDestroyOnLoad(go_MainTower);
            currentState = State.MOVING;
            SceneManager.LoadScene("Level01");
        }
    }

    public void DonePlacing()
    {
        if (currentState != State.PLACING_BUILDING)
            return;

        Destroy(placementBuilding);
        placementBuilding = null;
        currentState = State.MOVING;
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

    public void removeMaxPower(int amount)
    {
        amount = Mathf.Abs(amount);
        maxPower -= amount;
        if (maxPower < 0)
            maxPower = 0;
        // don't subtract curPower! Allow for over
    }

    public void consumePower(int amount)
    {
        amount = Mathf.Abs(amount);
        curPower -= amount;
        if (curPower < 0)
            Debug.LogWarning("Current Power dropped below zero!");
    }

    public void OnMainTowerDeath()
    {
        currentState = State.GAME_OVER;
        gameOverCanvas.SetActive(true);
    }
}
