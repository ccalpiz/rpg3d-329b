using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.Rendering;

public class PartyManager : MonoBehaviour
{
    [SerializeField]
    private List<Character> members = new List<Character>();
    public List<Character> Members { get { return members; } }

    [SerializeField]
    private List<Character> selectChars = new List<Character>();
    public List<Character> SelectChars { get { return selectChars; } }

    //[Header("Quests")]
    //[SerializeField]
    //private List<Quest> activeQuests = new List<Quest>();
    //public List<Quest> ActiveQuests { get { return activeQuests; } }

    //public Quest GetQuest(int questId)
    //{
    //    return activeQuests.Find(q => q.Data.id == questId);
    //}

    //public void RegisterQuest(Quest quest)
    //{
    //    if (GetQuest(quest.Data.id) == null)
    //        activeQuests.Add(quest);
    //}

    //public void OnEnemyKilled(string enemyTag)
    //{
    //    foreach (Quest q in activeQuests)
    //    {
    //        if (q.State == QuestState.InProgress &&
    //            q.Data.type == QuestType.KillCount &&
    //            q.Data.targetTag == enemyTag)
    //        {
    //            q.CurrentKillCount++;
    //            Debug.Log($"Kill count: {q.CurrentKillCount}/{q.Data.killCount} for quest {q.Data.questName}");
    //        }
    //    }
    //}

    public void AddExpToParty(int exp)
    {
        if (exp <= 0) return;
        Debug.Log($"Party gained {exp} EXP.");
    }

    public static PartyManager instance;

    void Awake()
    {
        instance = this;
    }

    public void SelectSingleHero(int i)
    {
        foreach (Character c in members)
            c.ToggleRingSelection(false);

        selectChars.Clear();

        selectChars.Add(members[i]);
        selectChars[0].ToggleRingSelection(true);
    }

    public void HeroSelectMagicSkill(int i)
    {
        if (selectChars.Count <= 0)
            return;

        selectChars[0].IsMagicMode = true;
        selectChars[0].CurMagicCast = selectChars[0].MagicSkills[i];
    }

    void Start()
    {
        foreach (Character c in members)
        {
            c.charInit(VFXManager.instance, UIManager.instance);
        }

        SelectSingleHero(0);

        // Hero 1
        members[0].MagicSkills.Add(new Magic(VFXManager.instance.MagicData[0]));
        members[1].MagicSkills.Add(new Magic(VFXManager.instance.MagicData[1]));
        members[2].MagicSkills.Add(new Magic(VFXManager.instance.MagicData[2]));
        members[3].MagicSkills.Add(new Magic(VFXManager.instance.MagicData[3]));

        // Hero 2
        members[4].MagicSkills.Add(new Magic(VFXManager.instance.MagicData[4]));
        members[5].MagicSkills.Add(new Magic(VFXManager.instance.MagicData[5]));
        members[6].MagicSkills.Add(new Magic(VFXManager.instance.MagicData[6]));
        members[7].MagicSkills.Add(new Magic(VFXManager.instance.MagicData[7]));

        InventoryManager.instance.AddItem(members[0], 0);//Health Potion
        InventoryManager.instance.AddItem(members[0], 1);//Sword

        InventoryManager.instance.AddItem(members[1], 0);//Health Potion
        InventoryManager.instance.AddItem(members[1], 1);//Sword
        InventoryManager.instance.AddItem(members[1], 2);//Shield

        UIManager.instance.ShowMagicToggles();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (selectChars.Count > 0)
            {
                selectChars[0].IsMagicMode = true;
                selectChars[0].CurMagicCast = selectChars[0].MagicSkills[0];
            }
        }
    }
}
