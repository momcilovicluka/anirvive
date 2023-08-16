using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;
    public TextMeshProUGUI healthCounter;

    public GameObject playerState;

    private float currentHealth, maxHealth;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    private void Update()
    {
        currentHealth = playerState.GetComponent<PlayerState>().currentHealth;
        maxHealth = playerState.GetComponent<PlayerState>().maxHealth;

        float fillValue = currentHealth / maxHealth;
        slider.value = fillValue;

        healthCounter.text = currentHealth + "/" + maxHealth;
    }
}