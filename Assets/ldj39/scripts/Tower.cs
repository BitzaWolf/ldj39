using UnityEngine;

public class Tower : Building
{
    [Header("Tower Specifics")]
    public float SearchRadius = 10;
    public float attackCooldown = 3;

    private GameObject target = null;
    private Collider[] searchResults = new Collider[30];
    private float attackTimer = 0;

    void Start()
    {
        curHealth = startingHealth;
    }

    protected override void updateActive()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }

        if (target == null)
            updateSearch();
        else
            updateAttack();
    }

    private void updateSearch()
    {
        // search for nearby enemies
        int count = Physics.OverlapSphereNonAlloc(transform.position, SearchRadius, searchResults);
        float distance = SearchRadius;
        for (int i = 0; i < count; ++i)
        {
            Collider c = searchResults[i];
            if (c.tag == "Enemy")
            {
                float newDist = Vector3.Distance(transform.position, c.transform.position);
                if (newDist < distance)
                {
                    target = c.gameObject;
                    distance = newDist;
                }
            }
        }

        // if found one, attach to its OnDeath event
        if (target != null)
        {
            target.GetComponent<Enemy>().OnDeath += onTargetDeath;
        }
    }

    private void updateAttack()
    {
        // If enemy left radius, look for a new one.
        if (Vector3.Distance(transform.position, target.transform.position) > SearchRadius)
        {
            target.GetComponent<Enemy>().OnDeath -= onTargetDeath;
            target = null;
            return;
        }

        // if attack's on CD. Just wait.
        if (attackTimer > 0)
            return;

        attackTimer = attackCooldown;
        // attack enemy
    }

    private void onTargetDeath()
    {
        target = null;
    }

    protected override void updateDeactive()
    {

    }

    protected override void onActivate()
    {
        
    }

    protected override void onDeactivate()
    {
        // empty
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 1);
        //Gizmos.DrawSphere(transform.position, SearchRadius); // Draw search sphere

        if (target != null)
            Gizmos.DrawLine(transform.position, target.transform.position);
    }
}
