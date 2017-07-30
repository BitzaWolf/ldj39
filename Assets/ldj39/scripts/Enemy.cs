using UnityEngine;

public class Enemy : MonoBehaviour
{

    public delegate void DeathEvent();
    public event DeathEvent OnDeath;

	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}
}
