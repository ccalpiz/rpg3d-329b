using UnityEngine;

public class NPC : Character
{
    [Header("NPC Info")]
    [SerializeField] private string npcName;
    public string NpcName { get { return npcName; } }

    [SerializeField] private Sprite npcPortrait;
    public Sprite NpcPortrait { get { return npcPortrait; } }

    [Header("Quests")]
    [SerializeField] private QuestData[] quests;
    public QuestData[] Quests { get { return quests; } }

    // Return the first active quest this NPC has for the party
    // Priority: InProgress > New (so player can turn in before getting new one)
    public Quest GetRelevantQuest()
    {
        if (quests == null || quests.Length == 0) return null;

        // First check if any quest is already InProgress
        foreach (QuestData qData in quests)
        {
            Quest existing = PartyManager.instance.GetQuest(qData.id);
            if (existing != null && existing.State == QuestState.InProgress)
                return existing;
        }

        // Then check for New quests
        foreach (QuestData qData in quests)
        {
            Quest existing = PartyManager.instance.GetQuest(qData.id);
            if (existing == null || existing.State == QuestState.New)
            {
                if (existing == null)
                {
                    Quest newQuest = new Quest(qData);
                    PartyManager.instance.RegisterQuest(newQuest);
                    return newQuest;
                }
                return existing;
            }
        }

        return null;
    }

    void Update() { } // NPCs don't use the combat state machine
}
