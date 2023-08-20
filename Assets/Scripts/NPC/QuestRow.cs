using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestRow : MonoBehaviour
{
    public TextMeshProUGUI questName;
    public TextMeshProUGUI questGiver;

    public Button trackingButton;

    public bool isActive;
    public bool isTracking;

    public Text coinAmount;

    public Image firstReward;
    public Text firstRewardAmount;

    public Image secondReward;
    public Text secondRewardAmount;

    public Quest thisQuest;

    private void Start()
    {
        trackingButton.onClick.AddListener(() =>
        {
            if (isActive)
            {
                if (isTracking)
                {
                    isTracking = false;
                    trackingButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Track";
                    QuestManager.Instance.UnTrackQuest(thisQuest);
                }
                else
                {
                    isTracking = true;
                    trackingButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Untrack";
                    QuestManager.Instance.TrackQuest(thisQuest);
                }
            }
        });
    }
}