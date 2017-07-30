using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float SearchRadius = 10;
    public float moveSpeed = 5;
    public int damage = 5;
    public int power = 5;

    public delegate void DeathEvent();
    public event DeathEvent OnDeath;

    [Header("Pseudo Privates (debug)")]
    public int health;

    private GameObject target;
    private Collider[] searchResults = new Collider[10];

	void Start ()
    {
        target = GameManager.gm.go_MainTower;
	}
	
	void Update ()
    {
        searchForTargets();
        move();
	}

    private void searchForTargets()
    {
        if (target != GameManager.gm.go_MainTower)
            return;

        int count = Physics.OverlapSphereNonAlloc(transform.position, SearchRadius, searchResults);
        float distance = SearchRadius;
        for (int i = 0; i < count; ++i)
        {
            Collider c = searchResults[i];
            if (c.tag == "Building")
            {
                float newDist = Vector3.Distance(transform.position, c.transform.position);
                if (newDist < distance)
                {
                    if (target.GetComponent<Tower>() != null && target.GetComponent<Tower>().isDead())
                        continue;
                    if (target.GetComponent<Miner>() != null && target.GetComponent<Miner>().isDead())
                        continue;
                    target = c.gameObject;
                    distance = newDist;
                }
            }
        }

        if (target != GameManager.gm.go_MainTower)
        {
            Tower t = target.GetComponent<Tower>();
            if (t != null)
                t.OnDeath += onTargetDeath;

            Miner m = target.GetComponent<Miner>();
            if (m != null)
                m.OnDeath += onTargetDeath;
        }
    }

    private void move()
    {
        Vector3 direction = Vector3.Normalize(target.transform.position - transform.position);
        Vector3 translation = direction * moveSpeed * Time.deltaTime;
        translation.y = 0;
        transform.Translate(translation, Space.World);
    }

    private void onTargetDeath()
    {
        target = GameManager.gm.go_MainTower;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Tower Projectile")
        {
            // get Tower projectile game object
            // get damage
            // take damage
        }
    }

    public void takeDamage(int amount)
    {
        amount = Mathf.Abs(amount);
        health -= amount;
        if (health <= 0)
        {
            health = 0;
            GameManager.gm.addPower(power);
            OnDeath();
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = new Color(1, 0, 0, .3f);
        //Gizmos.DrawSphere(transform.position, SearchRadius);

        if (target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, target.transform.position);
        }
    }
}
