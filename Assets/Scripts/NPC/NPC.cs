using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public bool playerInRange;
    public bool isTalkingWithPlayer;

    private TextMeshProUGUI npcDialogText;

    private Button optionButton1;
    private TextMeshProUGUI optionButton1Text;
    private Button optionButton2;
    private TextMeshProUGUI optionButton2Text;

    public List<Quest> quests;
    public Quest currentActiveQuest = null;
    public int activeQuestIndex = 0;
    public bool firstTimeInteraction = true;
    public int currentDialog;

    public Animator animator;

    private void Start()
    {
        npcDialogText = DialogSystem.Instance.dialogText;

        optionButton1 = DialogSystem.Instance.option1BTN;
        optionButton1Text = DialogSystem.Instance.option1BTN.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

        optionButton2 = DialogSystem.Instance.option2BTN;
        optionButton2Text = DialogSystem.Instance.option2BTN.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
            animator.SetBool("isTalking", SoundManager.Instance.IsPlayingVoiceOver());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }

        animator.SetTrigger("Greet");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void StartConversation()
    {
        isTalkingWithPlayer = true;

        LookAtPlayer();

        if (firstTimeInteraction)
        {
            firstTimeInteraction = false;
            currentActiveQuest = quests[activeQuestIndex];
            StartQuestInitialDialog();
            currentDialog = 0;
        }
        else
        {
            if (currentActiveQuest.declined)
            {
                DialogSystem.Instance.OpenDialogUI();

                npcDialogText.text = currentActiveQuest.info.comebackAfterDecline;
                SoundManager.Instance.PlayVoiceOver(currentActiveQuest.info.comebackAfterDeclineClip);

                SetAcceptAndDeclineOptions();
            }

            if (currentActiveQuest.accepted && currentActiveQuest.isCompleted == false)
            {
                if (AreQuestRequirmentsCompleted())
                {
                    SubmitRequiredItems();

                    DialogSystem.Instance.OpenDialogUI();

                    npcDialogText.text = currentActiveQuest.info.comebackCompleted;
                    SoundManager.Instance.PlayVoiceOver(currentActiveQuest.info.comebackCompletedClip);

                    optionButton1Text.text = "[Take Reward]";
                    optionButton1.onClick.RemoveAllListeners();
                    optionButton1.onClick.AddListener(() =>
                    {
                        ReceiveRewardAndCompleteQuest();
                    });
                }
                else
                {
                    DialogSystem.Instance.OpenDialogUI();

                    npcDialogText.text = currentActiveQuest.info.comebackInProgress;
                    SoundManager.Instance.PlayVoiceOver(currentActiveQuest.info.comebackInProgressClip);

                    optionButton1Text.text = "[Close]";
                    optionButton1.onClick.RemoveAllListeners();
                    optionButton1.onClick.AddListener(() =>
                    {
                        DialogSystem.Instance.CloseDialogUI();
                        isTalkingWithPlayer = false;
                    });
                }
            }

            if (currentActiveQuest.isCompleted == true)
            {
                DialogSystem.Instance.OpenDialogUI();

                npcDialogText.text = currentActiveQuest.info.finalWords;
                SoundManager.Instance.PlayVoiceOver(currentActiveQuest.info.finalWordsClip);
                animator.SetTrigger("Pose");

                optionButton1Text.text = "[Close]";
                optionButton1.onClick.RemoveAllListeners();
                optionButton1.onClick.AddListener(() =>
                {
                    DialogSystem.Instance.CloseDialogUI();
                    isTalkingWithPlayer = false;
                });
            }

            if (currentActiveQuest.initialDialogCompleted == false)
            {
                StartQuestInitialDialog();
            }
        }
    }

    private void SetAcceptAndDeclineOptions()
    {
        optionButton1Text.text = currentActiveQuest.info.acceptOption;
        optionButton1.onClick.RemoveAllListeners();
        optionButton1.onClick.AddListener(() =>
        {
            AcceptedQuest();
        });

        optionButton2.gameObject.SetActive(true);
        optionButton2Text.text = currentActiveQuest.info.declineOption;
        optionButton2.onClick.RemoveAllListeners();
        optionButton2.onClick.AddListener(() =>
        {
            DeclinedQuest();
        });
    }

    private void SubmitRequiredItems()
    {
        string firstRequiredItem = currentActiveQuest.info.firstRequirmentItem;
        int firstRequiredAmount = currentActiveQuest.info.firstRequirementAmount;

        if (firstRequiredItem != "")
        {
            InventorySystem.Instance.RemoveItem(firstRequiredItem, firstRequiredAmount);
        }

        string secondtRequiredItem = currentActiveQuest.info.secondRequirmentItem;
        int secondRequiredAmount = currentActiveQuest.info.secondRequirementAmount;

        if (firstRequiredItem != "")
        {
            InventorySystem.Instance.RemoveItem(secondtRequiredItem, secondRequiredAmount);
        }
    }

    private bool AreQuestRequirmentsCompleted()
    {
        string firstRequiredItem = currentActiveQuest.info.firstRequirmentItem;
        int firstRequiredAmount = currentActiveQuest.info.firstRequirementAmount;

        var firstItemCounter = 0;

        foreach (string item in InventorySystem.Instance.itemList)
            if (item == firstRequiredItem)
                firstItemCounter++;

        currentActiveQuest.info.hasCheckpoints = currentActiveQuest.info.checkpoints.Count > 0;

        SetQuestHasCheckpoints(currentActiveQuest);

        bool allCheckpointsCompleted = false;

        if (currentActiveQuest.info.hasCheckpoints)
            foreach (Checkpoint checkpoint in currentActiveQuest.info.checkpoints)
            {
                if (!checkpoint.isCompleted)
                {
                    allCheckpointsCompleted = false;
                    break;
                }

                allCheckpointsCompleted = true;
            }

        string secondRequiredItem = currentActiveQuest.info.secondRequirmentItem;
        int secondRequiredAmount = currentActiveQuest.info.secondRequirementAmount;

        var secondItemCounter = 0;

        foreach (string item in InventorySystem.Instance.itemList)
        {
            if (item == secondRequiredItem)
            {
                secondItemCounter++;
            }
        }

        if (firstItemCounter >= firstRequiredAmount && secondItemCounter >= secondRequiredAmount)
        {
            if(currentActiveQuest.info.hasCheckpoints && !allCheckpointsCompleted)
                return false;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SetQuestHasCheckpoints(Quest activeQuest)
    {
        activeQuest.info.hasCheckpoints = activeQuest.info.checkpoints.Count > 0;
    }

    private void StartQuestInitialDialog()
    {
        DialogSystem.Instance.OpenDialogUI();

        npcDialogText.text = currentActiveQuest.info.initialDialog[currentDialog];
        SoundManager.Instance.PlayVoiceOver(currentActiveQuest.info.initialDialogClips[currentDialog]);

        optionButton1Text.text = "Next";
        optionButton1.onClick.RemoveAllListeners();
        optionButton1.onClick.AddListener(() =>
        {
            currentDialog++;
            CheckIfDialogDone();
        });

        optionButton2.gameObject.SetActive(false);
    }

    private void CheckIfDialogDone()
    {
        if (currentDialog == currentActiveQuest.info.initialDialog.Count - 1)
        {
            npcDialogText.text = currentActiveQuest.info.initialDialog[currentDialog];
            SoundManager.Instance.PlayVoiceOver(currentActiveQuest.info.initialDialogClips[currentDialog]);

            currentActiveQuest.initialDialogCompleted = true;

            SetAcceptAndDeclineOptions();
        }
        else
        {
            npcDialogText.text = currentActiveQuest.info.initialDialog[currentDialog];
            SoundManager.Instance.PlayVoiceOver(currentActiveQuest.info.initialDialogClips[currentDialog]);

            optionButton1Text.text = "Next";
            optionButton1.onClick.RemoveAllListeners();
            optionButton1.onClick.AddListener(() =>
            {
                currentDialog++;
                CheckIfDialogDone();
            });
        }
    }

    private void AcceptedQuest()
    {
        QuestManager.Instance.AddActiveQuest(currentActiveQuest);

        currentActiveQuest.accepted = true;
        currentActiveQuest.declined = false;

        if (currentActiveQuest.hasNoRequirements)
        {
            npcDialogText.text = currentActiveQuest.info.comebackCompleted;
            SoundManager.Instance.PlayVoiceOver(currentActiveQuest.info.comebackCompletedClip);

            optionButton1Text.text = "[Take Reward]";
            optionButton1.onClick.RemoveAllListeners();
            optionButton1.onClick.AddListener(() =>
            {
                ReceiveRewardAndCompleteQuest();
            });
            optionButton2.gameObject.SetActive(false);
        }
        else
        {
            npcDialogText.text = currentActiveQuest.info.acceptAnswer;
            SoundManager.Instance.PlayVoiceOver(currentActiveQuest.info.acceptAnswerClip);
            CloseDialogUI();
        }
    }

    private void CloseDialogUI()
    {
        optionButton1Text.text = "[Close]";
        optionButton1.onClick.RemoveAllListeners();
        optionButton1.onClick.AddListener(() =>
        {
            DialogSystem.Instance.CloseDialogUI();
            isTalkingWithPlayer = false;
        });
        optionButton2.gameObject.SetActive(false);
    }

    private void ReceiveRewardAndCompleteQuest()
    {
        QuestManager.Instance.MarkQuestCompleted(currentActiveQuest);

        currentActiveQuest.isCompleted = true;

        var coinsRecieved = currentActiveQuest.info.coinReward;
        print("You recieved " + coinsRecieved + " gold coins");

        if (currentActiveQuest.info.rewardItem1 != "")
        {
            InventorySystem.Instance.AddToInventory(currentActiveQuest.info.rewardItem1);
        }

        if (currentActiveQuest.info.rewardItem2 != "")
        {
            InventorySystem.Instance.AddToInventory(currentActiveQuest.info.rewardItem2);
        }

        activeQuestIndex++;

        if (activeQuestIndex < quests.Count)
        {
            currentActiveQuest = quests[activeQuestIndex];
            currentDialog = 0;
            DialogSystem.Instance.CloseDialogUI();
            isTalkingWithPlayer = false;
        }
        else
        {
            DialogSystem.Instance.CloseDialogUI();
            isTalkingWithPlayer = false;
        }
    }

    private void DeclinedQuest()
    {
        currentActiveQuest.declined = true;

        npcDialogText.text = currentActiveQuest.info.declineAnswer;
        SoundManager.Instance.PlayVoiceOver(currentActiveQuest.info.declineAnswerClip);
        animator.SetTrigger("Denied");
        CloseDialogUI();
    }

    public void LookAtPlayer()
    {
        StartCoroutine(LookAtPlayerCoroutine());
    }

    private IEnumerator LookAtPlayerCoroutine()
    {
        while (isTalkingWithPlayer)
        {
            Vector3 directionToPlayer = PlayerState.Instance.playerBody.transform.position - transform.position;
            directionToPlayer.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.1f);
            yield return null;
        }
    }
}