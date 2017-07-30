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

    public delegate void ActivateEvent();
    public event ActivateEvent OnActivation;
    public delegate void DeactivateEvent();
    public event DeactivateEvent OnDeactivation;
    public delegate void FullyHealedEvent();
    public event FullyHealedEvent OnFullyHealed;
    public delegate void DeathEvent();
    public event DeathEvent OnDeath;

    protected int curHealth;
    protected bool isActive = true;

	void Start ()
    {
        curHealth = startingHealth;
	}

    private void Update()
    {
        if (isActive)
            updateActive();
        else
            updateDeactive();
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
}
