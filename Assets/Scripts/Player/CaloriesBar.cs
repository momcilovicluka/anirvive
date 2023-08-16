using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CaloriesBar : MonoBehaviour
{
    private Slider slider;
    public TextMeshProUGUI caloriesCounter;

    public GameObject playerState;

    private float currentCalories, maxCalories;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    private void Update()
    {
        currentCalories = playerState.GetComponent<PlayerState>().currentCalories;
        maxCalories = playerState.GetComponent<PlayerState>().maxCalories;

        float fillValue = currentCalories / maxCalories;
        slider.value = fillValue;

        caloriesCounter.text = currentCalories + "/" + maxCalories;
    }
}