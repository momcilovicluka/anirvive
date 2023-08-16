using System.Collections;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance { get; set; }

    // ----- Player Health ----- //
    public float currentHealth;

    public float maxHealth;

    // ----- Player Calories ----- //
    public float currentCalories;

    public float maxCalories;

    private float distanceTravelled = 0f;
    private Vector3 lastPosition;

    public GameObject playerBody;

    // ----- Player Hydration ----- //
    public float currentHydration;

    public float maxHydration;

    private void Start()
    {
        currentHealth = maxHealth;
        currentCalories = maxCalories;
        currentHydration = maxHydration;

        StartCoroutine(decreaseHydration());
    }

    private IEnumerator decreaseHydration()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);
            currentHydration--;
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    private void Update()
    {
        distanceTravelled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = playerBody.transform.position;

        if (distanceTravelled >= 5)
        {
            distanceTravelled = 0;
            currentCalories--;
        }
    }

    internal void setHealth(float maxHealth)
    {
        currentHealth = maxHealth;
    }

    internal void setCalories(float maxCalories)
    {
        currentCalories = maxCalories;
    }

    internal void setHydration(float v)
    {
        currentHydration = v;
    }
}