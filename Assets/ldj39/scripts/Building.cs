/**
 * Building is the base class for all buildings in the game. Buildings are any player-controlled
 * building objects like the tower or a miner.
 */

using UnityEngine;

public abstract class Building : MonoBehaviour
{
    public int startingHealth = 100;
    public int buildingCost = 50;
    public int powerOccupy = 50;
    public float iFrames = 0.5f;

    public delegate void ActivateEvent();
    public event ActivateEvent OnActivation;
    public delegate void DeactivateEvent();
    public event DeactivateEvent OnDeactivation;
    public delegate void FullyHealedEvent();
    public event FullyHealedEvent OnFullyHealed;
    public delegate void DeathEvent();
    public event DeathEvent OnDeath;

    [Header("Pseudo Privates")] // Please ignore this TERRIBLE programming practice
    public int curHealth;

    // Back to protected/private members
    protected bool isActive = true;
    protected float iFrameTimer = 0;

	void Start ()
    {
        
	}

    private void Update()
    {
        if (isActive)
            updateActive();
        else
            updateDeactive();

        if (iFrameTimer > 0)
            iFrameTimer -= Time.deltaTime;
    }

    private void commonDeactivate()
    {
        GameManager.gm.addPower(powerOccupy);
    }

    public bool canActivate()
    {
        return GameManager.gm.curPower >= powerOccupy;
    }

    private void commonActivate()
    {
        GameManager.gm.consumePower(powerOccupy);
    }

    protected abstract void updateActive();
    protected abstract void updateDeactive();
    protected abstract void onActivate();
    protected abstract void onDeactivate();

    public void takeDamage(int amount)
    {
        amount = Mathf.Abs(amount);
        curHealth -= amount;
        
        if (curHealth <= 0)
        {
            curHealth = 0;
            commonDeactivate();
            onDeactivate();
            OnDeath();
        }
    }

    public void heal(int amount)
    {
        amount = Mathf.Abs(amount);
        if (amount + curHealth > startingHealth)
            amount = startingHealth - curHealth;

        curHealth += amount;

        if (curHealth == startingHealth)
            OnFullyHealed();
    }

    public void setActive(bool active)
    {
        if (active == isActive)
            return;

        isActive = active;
        if (isActive)
        {
            onActivate();
            OnActivation();
        }
        else
        {
            onDeactivate();
            OnDeactivation();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (iFrameTimer > 0)
                return;

            iFrameTimer = iFrames;
            takeDamage(collision.gameObject.GetComponent<Enemy>().damage);
        }
    }

    public bool isDead()
    {
        return (curHealth <= 0);
    }
}
