using UnityEngine;

public class Tower : Building
{
    [Header("Tower Specifics")]
    public float SearchRadius = 10;
    public float attackCooldown = 0.5f;
    public int damageTick = 1;
    public ParticleSystem lightning;
    public ParticleSystem targetLightning;
    public Light spotlight;

    private GameObject target = null;
    private Enemy targetEnemy = null;
    private Collider[] searchResults = new Collider[30];
    private float attackTimer = 0;
    private Vector3 rotVector = new Vector3(); // used for rotating lightning to target enemies
    private Quaternion rotQuat = new Quaternion();

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
            targetEnemy = target.GetComponent<Enemy>();
            targetEnemy.OnDeath += onTargetDeath;
            targetLightning.Play();
        }
    }

    private void updateAttack()
    {
        // If enemy left radius, look for a new one.
        if (Vector3.Distance(transform.position, target.transform.position) > SearchRadius)
        {
            targetEnemy.OnDeath -= onTargetDeath;
            target = null;
            targetEnemy = null;
            return;
        }

        // reposition laser
        Vector3 diff = Vector3.Normalize(target.transform.position - targetLightning.transform.position);
        float rotZ = Mathf.Asin(diff.y) * 180 / Mathf.PI;
        float rotY = Mathf.Asin(diff.z) * 180 / Mathf.PI;
        rotVector.Set(0, -rotY, rotZ);
        rotQuat.eulerAngles = rotVector;
        targetLightning.transform.rotation = rotQuat;

        // if attack's on CD. Just wait.
        if (attackTimer > 0)
            return;

        attackTimer = attackCooldown;
        targetEnemy.takeDamage(damageTick);
        // attack enemy
    }

    private void onTargetDeath()
    {
        target = null;
        targetLightning.Stop();
    }

    protected override void updateDeactive()
    {
        
    }

    protected override void onActivate()
    {
        lightning.Play();
        spotlight.enabled = true;
    }

    protected override void onDeactivate()
    {
        lightning.Stop();
        targetLightning.Stop();
        spotlight.enabled = false;
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = new Color(1, 0, 0, 0.3f);
        //Gizmos.DrawSphere(transform.position, SearchRadius);

        Gizmos.color = Color.red;
        if (target != null)
            Gizmos.DrawLine(transform.position, target.transform.position);
    }
}
