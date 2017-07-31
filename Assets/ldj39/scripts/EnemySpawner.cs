using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnTimerMin;
    public float spawnTimerMax;

    private float spawnTimer;
    private Collider[] searchResults = new Collider[30];

    void Start ()
    {
        spawnTimer = Random.Range(spawnTimerMin, spawnTimerMax);
	}
	
	void Update ()
    {
        spawnTimer -= Time.deltaTime * GameManager.gm.SpawnRate;
        if (spawnTimer <= 0)
        {
            spawnTimer = Random.Range(spawnTimerMin, spawnTimerMax);
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.transform.position = transform.position;
        }

        int count = Physics.OverlapSphereNonAlloc(transform.position, 45, searchResults);
        for (int i = 0; i < count; ++i)
        {
            Collider c = searchResults[i];
            if (c.tag == "Building" && c.GetComponent<Tower>() != null)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0.6f, 0, 0.5f);
        Gizmos.DrawSphere(transform.position, 5);
    }
}
