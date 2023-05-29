using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public PostProcessVolume volume;
    private Vignette vignette;
    public Image bloodScreen;
    private Color originalColor;
    public AudioSource audioSource;

    public bool canDie = true;                  // Whether or not this health can die

    public bool onLava;
    private float lavaTimer = 0f;

    public float startingHealth = 100.0f;        // The amount of health to start with
    public float maxHealth = 100.0f;            // The maximum amount of health
    public float currentHealth;                // The current ammount of health
    public Text playerHealth;

    public bool replaceWhenDead = false;        // Whether or not a dead replacement should be instantiated.  (Useful for breaking/shattering/exploding effects)
    public GameObject deadReplacement;            // The prefab to instantiate when this GameObject dies
    public bool makeExplosion = false;            // Whether or not an explosion prefab should be instantiated
    public GameObject explosion;                // The explosion prefab to be instantiated

    public bool isPlayer = false;                // Whether or not this health is the player
    public GameObject deathCam;                 // The camera to activate when the player dies

    private float regenerateTimer = 0.0f;
    public float regenerateHealthInterval = 1f;
    public float regenerateHealthAmount = 0.03f;

    private bool dead = false;                    // Used to make sure the Die() function isn't called twice
    // Use this for initialization
    void Start()
    {
        originalColor = bloodScreen.color;
        // Initialize the currentHealth variable to the value specified by the user in startingHealth
        currentHealth = startingHealth;

        //volume.profile.TryGetSettings(out vignette);                             //暂时关闭
        //vignette.intensity.value = 0f;
    }

    public void ChangeHealth(float amount)
    {
        regenerateTimer = 3.0f;
        // Change the health by the amount specified in the amount variable
        currentHealth += amount;

        // If the health runs out, then Die.
        if (currentHealth <= 0 && !dead && canDie)
            //Die();
            currentHealth = 1;
        // Make sure that the health never exceeds the maximum health
        else if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        //if (volume.profile.TryGetSettings(out vignette))                                  //暂时关闭
        //{
        //    float percent = 1.0f - (currentHealth / maxHealth);
        //    vignette.intensity.value = percent;
        //}
    }

    public void Die()
    {
        // This GameObject is officially dead.  This is used to make sure the Die() function isn't called again
        dead = true;

        // Make death effects
        if (replaceWhenDead)
            Instantiate(deadReplacement, transform.position, transform.rotation);
        if (makeExplosion)
            Instantiate(explosion, transform.position, transform.rotation);
        if (isPlayer)
        {
            SceneManager.LoadScene(0);
        }
        if (isPlayer && deathCam != null)
            deathCam.SetActive(true);


        // Remove this GameObject from the scene
        Destroy(gameObject);
    }

    //private void OnGUI()
    //{
    //    if (isPlayer)
    //    {
    //        GUI.Label(new Rect(10, 10, 100, 20), "Health: " + currentHealth / maxHealth * 100);
    //    }

    //}

    private void regenerateHealth()
    {
        if (onLava)
        {
            return;
        }
        if (regenerateTimer < 0.0f)
        {
            ChangeHealth(maxHealth * regenerateHealthAmount);
            regenerateTimer = regenerateHealthInterval;
        }
    }
    private void Update()
    {
        regenerateTimer -= Time.deltaTime;
        regenerateHealth();
        playerHealth.text = currentHealth.ToString();
        BloodScreenEffect();
        LavaDamage();


    }

    private void LavaDamage()
    {

        if (onLava)
        {
            lavaTimer += Time.deltaTime;
            if (lavaTimer >= 0.5f)
            {
                lavaTimer = 0f;
                currentHealth -= 2f;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other!=null && other.gameObject.CompareTag("Lava"))
        {
            onLava = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other != null && other.gameObject.CompareTag("Lava"))
        {
            onLava = false;
        }
    }

    private void BloodScreenEffect()
    {
        if (currentHealth >= maxHealth * 0.8)
        {
            bloodScreen.color = originalColor;
        }
        else
        {
            float alpha = (maxHealth - currentHealth) / 100;
            bloodScreen.color = new Color(bloodScreen.color.r, bloodScreen.color.g, bloodScreen.color.b, alpha);
            if (currentHealth >= maxHealth / 2)
            {
                audioSource.enabled = false;
            }
            if (currentHealth < maxHealth / 2)
            {
                audioSource.enabled = true;
                audioSource.volume = ((maxHealth / 2) - currentHealth) / (maxHealth / 2);
            }
        }
    }
}