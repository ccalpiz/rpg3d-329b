using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.Rendering;

public class PartyManager : MonoBehaviour
{
    [SerializeField]
    private HeroData[] heroData;
    public HeroData[] HeroData { get { return heroData; } }

    [SerializeField]
    private List<Character> members = new List<Character>();
    public List<Character> Members { get { return members; } }

    [SerializeField]
    private List<Character> selectChars = new List<Character>();
    public List<Character> SelectChars { get { return selectChars; } }

    [SerializeField]
    private List<Quest> questList = new List<Quest>();
    public List<Quest> QuestList { get { return questList; } }

    [SerializeField]
    private int partyMoney = 1000;
    public int PartyMoney { get { return partyMoney; } set { partyMoney = value; } }

    [SerializeField]
    private int totalExp;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // foreach (Character c in members)
        // {
        //     c.CharInit(VFXManager.instance, UIManager.instance, InventoryManager.instance, this);
        // }

        SelectSingleHero(0);

        // Hero 1
        // members[0].MagicSkills.Add(new Magic(VFXManager.instance.MagicData[0]));
        // members[0].MagicSkills.Add(new Magic(VFXManager.instance.MagicData[1]));
        // members[0].MagicSkills.Add(new Magic(VFXManager.instance.MagicData[1]));
        // members[0].MagicSkills.Add(new Magic(VFXManager.instance.MagicData[1]));

        // // Hero 2
        // members[1].MagicSkills.Add(new Magic(VFXManager.instance.MagicData[1]));
        // members[1].MagicSkills.Add(new Magic(VFXManager.instance.MagicData[1]));
        // members[1].MagicSkills.Add(new Magic(VFXManager.instance.MagicData[1]));
        // members[1].MagicSkills.Add(new Magic(VFXManager.instance.MagicData[1]));

        // InventoryManager.instance.AddItem(members[0], 0);//Health Potion
        // InventoryManager.instance.AddItem(members[0], 1);//Sword A
        // InventoryManager.instance.AddItem(members[0], 4);//Sword B
        // InventoryManager.instance.AddItem(members[0], 2);//Shield A

        // InventoryManager.instance.AddItem(members[1], 0);//Health Potion
        // InventoryManager.instance.AddItem(members[1], 1);//Sword A
        // InventoryManager.instance.AddItem(members[1], 2);//Shield A
        // InventoryManager.instance.AddItem(members[1], 3);//Shield B

        UIManager.instance.ShowMagicToggles();
    }

    // Update is called once per frame
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

    public int FidIndexFromClass(Character hero)
    {
        for (int i = 0; i < members.Count; i++)
        {
            if (members[i] == hero)
                return i;
        }
        return 0;
    }

    public void SelectsingleHeroByToggle(int i)
    {
        if (selectChars.Contains(members[i]))
        {
            members[i].ToggleRingSelection(true);
            UIManager.instance.ShowMagicToggles();
        }
        else
        {
            selectChars.Add(members[i]);
            members[i].ToggleRingSelection(true);
            UIManager.instance.ShowMagicToggles();
        }
    }

    public void UnSelectSingleHeroByToggle(int i)
    {
        // if (selectChars.Count <= 1)
        // {
        //     UIManager.instance.ToggleAvatar[i].isOn = true;
        //     return;
        // }

        if (selectChars.Contains(members[i]))
        {
            selectChars.Remove(members[i]);
            members[i].ToggleRingSelection(false);
        }
    }

    public void RemoveHeroFromParty(int id)
    {
        if (id == -1 || id == 0)
            return;
        if (selectChars.Contains(members[id]))
            selectChars.Remove(members[id]);

        members.Remove(members[id]);
    }

    public void DistributeTotalExp(int n)
    {
        totalExp = n;
        int eachHeroExp = totalExp / members.Count;

        foreach (Hero hero in members)
            hero.ReceiveExp(eachHeroExp);
    }

    public bool HeroJoinParty(Character hero)
    {
        if (members.Count >= 6)
            return false;

        if (members.Contains(hero))
            return false;

        hero.CharInit(VFXManager.instance, UIManager.instance,
                InventoryManager.instance, this);

        members.Add(hero);
        return true;
    }

    public void SaveAllHeroData()
    {
        for (int i = 0; i < members.Count; i++)
        {
            Hero hero = (Hero)members[i];
            heroData[i].prefabId = hero.PrefabID;
            heroData[i].curHp = hero.CurHp;

            heroData[i].magicIds.Clear();
            for (int j = 0; j < hero.MagicSkills.Count; j++)
                heroData[i].magicIds.Add(hero.MagicSkills[j].ID);


            for (int k = 0; k < hero.InventoryItems.Length; k++)
            {
                if (hero.InventoryItems[k] == null)
                    heroData[i].inventoryItemIds[k] = -1;
                else
                    heroData[i].inventoryItemIds[k] = hero.InventoryItems[k].ID;
            }

            heroData[i].attackDamage = hero.AttackDamage;
            heroData[i].defensePower = hero.DefensePower;
            heroData[i].exp = hero.Exp;
            heroData[i].level = hero.Level;
            heroData[i].nextExp = hero.NextExp;

            heroData[i].equippedWeaponId = hero.MainWeapon != null ? hero.MainWeapon.ID : -1;
            heroData[i].equippedArmorId = hero.Shield != null ? hero.Shield.ID : -1;
        }
    }

    public void LoadAllHeroData()
    {
        foreach (Character c in members)
        {
            if (c != null)
                Destroy(c.gameObject);
        }
        members.Clear();
        selectChars.Clear();

        int enterId = Setting.enterPointId;
        Vector3 pos = MapManager.instance.EnterPoints[enterId].position;

        for (int i = 0; i < Setting.partyCount; i++)
        {
            GameObject heroObj =
                Instantiate(GameManager.instance.HeroPrefabs[heroData[i].prefabId],
                pos, Quaternion.identity);

            if (i == 0)
                heroObj.gameObject.tag = "Player";

            Hero hero = heroObj.GetComponent<Hero>();
            hero.CharInit(VFXManager.instance, UIManager.instance, InventoryManager.instance, this);
            hero.CurHp = heroData[i].curHp;

            for (int j = 0; j < heroData[i].magicIds.Count; j++)
            {
                int magicId = heroData[i].magicIds[j];
                hero.MagicSkills.Add(new Magic(VFXManager.instance.MagicData[magicId]));
            }

            for (int k = 0; k < heroData[i].inventoryItemIds.Length; k++)
            {
                int itemId = heroData[i].inventoryItemIds[k];
                if (itemId != -1)
                    hero.InventoryItems[k] =
                        new Item(InventoryManager.instance.ItemData[itemId]);
            }

            hero.AttackDamage = heroData[i].attackDamage;
            hero.DefensePower = heroData[i].defensePower;
            hero.Exp = heroData[i].exp;
            hero.Level = heroData[i].level;
            hero.NextExp = heroData[i].nextExp;

            if (heroData[i].equippedWeaponId != -1)
            {
                Item weapon = new Item(InventoryManager.instance.ItemData[heroData[i].equippedWeaponId]);
                hero.EquipWeapon(weapon);
            }

            if (heroData[i].equippedArmorId != -1)
            {
                Item armor = new Item(InventoryManager.instance.ItemData[heroData[i].equippedArmorId]);
                hero.EquipShield(armor);
            }

            members.Add(hero);
        }
    }
}
