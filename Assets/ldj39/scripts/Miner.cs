using UnityEngine;

public class Miner : Building
{
    [Header("Miner Specific")]
    public int powerPerSecond = 1;
    public int maxPower = 5;
    public float timeForMaxPower = 60;
    public float gemRadius = 30;
    public ParticleSystem targetLightning;

    private GameObject target = null;
    private PowerGem targetGem;
    private float timer, grantMaxTimer;
    private int maxPowerLeftToGive;
    private Vector3 rotVector = new Vector3(); // used for rotating lightning to target enemies
    private Quaternion rotQuat = new Quaternion();

    void Start()
    {
        curHealth = startingHealth;
        maxPowerLeftToGive = maxPower;
        Collider[] results = Physics.OverlapSphere(transform.position, gemRadius);
        float distance = 1000;
        foreach (Collider c in results)
        {
            if (c.tag == "Crystal")
            {
                float newDist = Vector3.Distance(transform.position, c.transform.position);
                PowerGem g = c.GetComponent<PowerGem>();
                if (!g.isUsed && newDist < distance)
                {
                    target = c.gameObject;
                    distance = newDist;
                }
            }
        }

        if (target == null)
        {
            Debug.LogError("Miner without gem nearby to mine!");
            return;
        }

        targetGem = target.GetComponent<PowerGem>();
        targetGem.isUsed = true;

        //orient beam to fire at the gem
        Vector3 diff = Vector3.Normalize(target.transform.position - targetLightning.transform.position);
        float rotZ = Mathf.Asin(diff.y) * 180 / Mathf.PI;
        float rotY = Mathf.Atan2(diff.z, diff.x) * 180 / Mathf.PI;
        rotVector.Set(0, -rotY, rotZ);
        rotQuat = Quaternion.Euler(rotVector);
        targetLightning.transform.rotation = rotQuat;
    }

    protected override void updateActive()
    {
        timer -= Time.deltaTime;

        if (maxPowerLeftToGive > 0)
        {
            grantMaxTimer -= Time.deltaTime;
            if (grantMaxTimer <= 0)
            {
                grantMaxTimer += timeForMaxPower / maxPower;
                GameManager.gm.addMaxPower(1);
                --maxPowerLeftToGive;
            }
        }
    }

    protected override void updateDeactive()
    {

    }

    protected override void onActivate()
    {
        timer = timeForMaxPower;
        grantMaxTimer = timeForMaxPower / maxPower;
    }

    protected override void onDeactivate()
    {
        // subtract max power
        int maxPowerReduction = maxPower - maxPowerLeftToGive;
        GameManager.gm.removeMaxPower(maxPowerReduction);
        targetGem.isUsed = false;
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = new Color(0, 1, 0, 0.5f);
        //Gizmos.DrawSphere(transform.position, gemRadius);
    }
}
