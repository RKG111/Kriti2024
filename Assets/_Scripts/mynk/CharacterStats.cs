
using UnityEngine;

/* Base class that player and enemies can derive from to include stats. */

public class CharacterStats : MonoBehaviour {

    // Health
    public int maxHealth = 100;
    public int currentHealth { get; private set; }
    public int maxlanternTime = 90; // Initial lantern time
    public int lanternRange = 2;
	public int stamina=100;
	public int coins=0;
	public int greed=100;

    [SerializeField]
    private int damageBase = 20;
    public Stat damage;
    public Stat armor;
	public Stat shoes; //add modifier to increase stamina

    private bool lanternActive = false; // Flag to track lantern state
    public int currentLanternTime { get; private set; } // Current lantern time
    private float timeSinceLastUpdate = 0f; // Time since last timer update

    // Set current health to max health
    // when starting the game.
    public UIManager uIManager;
    public GameManager gameManager;
    void OnEnable ()
    {   
        uIManager = GameObject.FindObjectOfType<UIManager>();
        gameManager = GameObject.FindAnyObjectByType<GameManager>();
        currentHealth = maxHealth;
        uIManager.healthBar.maxValue = maxHealth;
        uIManager.staminaBar.maxValue = stamina;
        damage.AddModifier(damageBase); // Add modifier here
        currentLanternTime = maxlanternTime; // Initialize current lantern time
        uIManager.timer.maxValue = maxlanternTime;
		lanternActive=true;
        uIManager.greedValue.text = "Greed\t:"+greed.ToString();
        uIManager.currentValue.text = "Collected\t:"+"0";
		// Debug.Log("Game started");
    }

    void Update()
    {
        // Update timer if lantern is active
        if (lanternActive)
        {
            timeSinceLastUpdate += Time.deltaTime;
            if (timeSinceLastUpdate >= 1f) // Decrease timer every second
            {
                currentLanternTime--;
				//Debug.Log(currentLanternTime);
                uIManager.timer.value = currentLanternTime;
                timeSinceLastUpdate = 0f; // Reset time since last update
            }

            if (currentLanternTime <= 0)
            {
                Escape(); // Call Die() function when lantern time runs out
            }
        }
    }

	public void increaseLanternRadius(int radius)
	{
		lanternRange+=radius;
	}
	public void greedIncrease()
	{
		greed+=Mathf.RoundToInt(greed/3);
        uIManager.greedValue.text = "Greed\t:"+greed.ToString();
	}


	public void AddCoin(int coin)
	{
		coins+=coin;
		// Debug.Log("Coins");
		// Debug.Log("coins"+ coins);
	}

    // Damage the character
    public void TakeDamage (int damage)
    {
        // Subtract the armor value
        damage -= armor.GetValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        // Damage the character
        currentHealth -= damage;
        // Debug.Log(transform.name + " takes " + damage + " damage.");
        uIManager.healthBar.value = currentHealth;

        // If health reaches zero
        if (currentHealth <= 0)
        {
            // gameManager.Death();
            Die();
            uIManager.ActivatePanel("death");
        }
    }


	public void HealthUpgrade (int health)
    {    
     // Damage the character
	 
	    // Debug.Log(" Previous Health " + currentHealth);

        currentHealth += health;
        uIManager.healthBar.value = currentHealth;
        // Debug.Log(transform.name + " takes " + damage + " healthupgrade");
        // Debug.Log(" Current Health " + currentHealth);

    }

    public void ActivateLantern()
    {
        lanternActive = true;
    }

    public void DeactivateLantern()
    {
        lanternActive = false;
        currentLanternTime = maxlanternTime; // Reset lantern time when deactivated
    }

    public virtual void Die ()
    {
		coins=coins/2;
		GetComponent<Inventory>().emptyInventory();
        
        // Die in some way
        // This method is meant to be overwritten
        // Debug.Log(transform.name + " died.");
    }

    public void Escape(){
        // uIManager.ActivatePanel("main");
        // DungeonState.PlayerInstance.transform.position = new Vector3(-10,-10,0);
        coins+= (int)DungeonState.PlayerInstance.GetComponent<Inventory>().GetTotalCost()/3;
        // uIManager.coins.text = coins.ToString();
        DungeonState.PlayerInstance.GetComponent<Inventory>().emptyInventory();
    }
}

