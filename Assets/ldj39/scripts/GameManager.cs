using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gm = null;

    private GameManager() { } // private to force singleton

    /******************
     * RUB A DUB PUBS *
     ******************/
    [Header("Do not destroys")]
    public GameObject go_MainTower;
    public GameObject go_EventSystem;
    public GameObject cam;
    public MainTower MainTower;

    void Start ()
    {
        if (gm == null)
            gm = this;

        DontDestroyOnLoad(go_MainTower);
        DontDestroyOnLoad(go_EventSystem);
        DontDestroyOnLoad(cam);
        
        SceneManager.LoadScene("Level01");
    }
	
	void Update ()
    {
		
	}
}
