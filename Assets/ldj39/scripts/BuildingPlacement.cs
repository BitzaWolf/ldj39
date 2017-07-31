using UnityEngine;

public class BuildingPlacement : MonoBehaviour
{
    public enum Type
    {
        TOWER,
        MINER
    }

    public Type type = Type.TOWER;
    public int powerCost = 20;
    public float towerRadius = 45;
    public float minerRadius = 45;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        //Vector3 camNorm = Vector3.Normalize(GameManager.gm.cam.transform.position);
        Vector3 camP = GameManager.gm.cam.transform.position;
        Ray r = GameManager.gm.cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        Vector3 dir = r.direction;
        float scale = -camP.y / dir.y;
        dir *= scale;
        dir += camP;
        transform.position = dir;

        if (Input.GetButtonDown("LeftClick") && canPlace())
        {
            GameObject go = GameManager.gm.TowerPrefab;
            if (type == Type.TOWER)
            {
                go = GameManager.gm.TowerPrefab;
            }
            else if (type == Type.MINER)
            {
                go = GameManager.gm.MinerPrefab;
            }
            go = Instantiate(go);
            go.transform.position = dir;

            GameManager.gm.consumePower(powerCost);
            GameManager.gm.DonePlacing();
        }
    }

    private bool canPlace()
    {
        bool enoughPower = powerCost <= GameManager.gm.curPower;
        bool specifics = false;
        if (type == Type.TOWER)
            specifics = towerRequirements();
        else if (type == Type.MINER)
            specifics = minerRequirements();

        return enoughPower && specifics;
    }

    private bool towerRequirements()
    {
        Collider[] results = Physics.OverlapSphere(transform.position, towerRadius);
        foreach (Collider c in results)
        {
            if (c.tag == "Building")
            {
                Debug.Log("Found nearby tower.");
                return false;
            }
        }

        return true;
    }

    private bool minerRequirements()
    {
        Collider[] results = Physics.OverlapSphere(transform.position, minerRadius);
        float distance = 1000;
        bool towerInRadius = false;
        bool availableGem = false;
        foreach (Collider c in results)
        {
            if (!availableGem && c.tag == "Crystal")
            {
                float newDist = Vector3.Distance(transform.position, c.transform.position);
                PowerGem g = c.GetComponent<PowerGem>();
                if (!g.isUsed && newDist < distance)
                {
                    availableGem = true;
                }
            }
            if (!towerInRadius && c.tag == "Building")
            {
                towerInRadius = true;
            }
        }

        return towerInRadius && availableGem;
    }

    private void OnDrawGizmos()
    {
        /*
        Vector3 mousePos = Input.mousePosition;
        Ray r = GameManager.gm.cam.GetComponent<Camera>().ScreenPointToRay(mousePos);
        Gizmos.DrawRay(r);

        Vector3 dir = r.direction;
        float scale = 500;
        dir *= scale;
        dir += GameManager.gm.cam.transform.position;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(GameManager.gm.cam.transform.position, dir);*/
    }
}
