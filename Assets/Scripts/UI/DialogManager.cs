using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;

    [Header("Panel References")]
    [SerializeField] private GameObject grayOverlay;
    [SerializeField] private GameObject dialogPanel;

    [Header("NPC Display")]
    [SerializeField] private Image npcPortraitImage;
    [SerializeField] private Text npcNameText;
    [SerializeField] private Text dialogText;

    [Header("Buttons")]
    [SerializeField] private GameObject btnNext;
    [SerializeField] private GameObject btnAccept;
    [SerializeField] private GameObject btnReject;
    [SerializeField] private GameObject btnFinish;
    [SerializeField] private GameObject btnNotFinish;
    [SerializeField] private GameObject btnDone;

    private NPC currentNPC;
    private Character currentHero;
    private Quest currentQuest;
    private int dialogStep;

    private void Awake()
    {
        instance = this;
    }

    public void StartDialogue(NPC npc, Character hero)
    {
        currentNPC = npc;
        currentHero = hero;
        dialogStep = 0;

        currentQuest = npc.GetRelevantQuest();

        npcPortraitImage.sprite = npc.NpcPortrait;
        npcNameText.text = npc.NpcName;

        grayOverlay.SetActive(true);
        dialogPanel.SetActive(true);

        RefreshDialogue();
    }

    private void RefreshDialogue()
    {
        // Hide all buttons first
        btnNext.SetActive(false);
        btnAccept.SetActive(false);
        btnReject.SetActive(false);
        btnFinish.SetActive(false);
        btnNotFinish.SetActive(false);
        btnDone.SetActive(false);

        if (currentQuest == null)
        {
            dialogText.text = "Hello, traveler. I have nothing for you right now.";
            btnDone.SetActive(true);
            return;
        }

        switch (currentQuest.State)
        {
            case QuestState.New:
                ShowInitialDialogue();
                break;

            case QuestState.InProgress:
                ShowInProgressDialogue();
                break;

            case QuestState.Finished:
                dialogText.text = "Thank you for your help, adventurer!";
                btnDone.SetActive(true);
                break;

            case QuestState.Rejected:
                dialogText.text = currentQuest.Data.rejectDialogue;
                btnDone.SetActive(true);
                break;
        }
    }

    private void ShowInitialDialogue()
    {
        string[] lines = currentQuest.Data.initialDialogue;

        if (lines == null || lines.Length == 0)
        {
            dialogText.text = "I need your help.";
            btnAccept.SetActive(true);
            btnReject.SetActive(true);
            return;
        }

        if (dialogStep < lines.Length - 1)
        {
            // Still going through intro lines
            dialogText.text = lines[dialogStep];
            btnNext.SetActive(true);
        }
        else
        {
            // Last line — show accept/reject
            dialogText.text = lines[lines.Length - 1];
            btnAccept.SetActive(true);
            btnReject.SetActive(true);
        }
    }

    private void ShowInProgressDialogue()
    {
        bool done = currentQuest.IsRequirementMet(currentHero);

        if (done)
        {
            dialogText.text = currentQuest.Data.completionDialogue;
            btnFinish.SetActive(true);
        }
        else
        {
            dialogText.text = currentQuest.Data.inProgressDialogue;
            btnNotFinish.SetActive(true);
        }
    }

    // ─── Button Callbacks ───────────────────────────────────────────────────

    public void OnNextButton()
    {
        dialogStep++;
        RefreshDialogue();
    }

    public void OnAcceptButton()
    {
        if (currentQuest == null) return;
        currentQuest.State = QuestState.InProgress;
        RefreshDialogue();
    }

    public void OnRejectButton()
    {
        if (currentQuest == null) return;
        currentQuest.State = QuestState.Rejected;
        RefreshDialogue();
    }

    public void OnFinishButton()
    {
        if (currentQuest == null || currentHero == null) return;

        // Remove delivery item from inventory
        if (currentQuest.Data.type == QuestType.Delivery)
        {
            for (int i = 0; i < currentHero.InventoryItems.Length; i++)
            {
                Item item = currentHero.InventoryItems[i];
                if (item != null && item.ID == currentQuest.Data.deliveryItemId)
                {
                    InventoryManager.instance.RemoveItem(currentHero, i);
                    break;
                }
            }
        }

        // Give reward item
        if (currentQuest.Data.rewardItemId >= 0)
            InventoryManager.instance.AddItem(currentHero, currentQuest.Data.rewardItemId);

        // Give reward EXP (Week 13 will use this)
        PartyManager.instance.AddExpToParty(currentQuest.Data.rewardExp);

        currentQuest.State = QuestState.Finished;

        dialogText.text = "Well done! Here is your reward.";
        HideAllButtons();
        btnDone.SetActive(true);
    }

    public void OnNotFinishButton()
    {
        CloseDialogue();
    }

    public void OnDoneButton()
    {
        CloseDialogue();
    }

    private void HideAllButtons()
    {
        btnNext.SetActive(false);
        btnAccept.SetActive(false);
        btnReject.SetActive(false);
        btnFinish.SetActive(false);
        btnNotFinish.SetActive(false);
        btnDone.SetActive(false);
    }

    private void CloseDialogue()
    {
        currentNPC = null;
        currentHero = null;
        currentQuest = null;
        dialogStep = 0;
        grayOverlay.SetActive(false);
        dialogPanel.SetActive(false);
    }
}
