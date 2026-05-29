using UnityEngine;

public class Hero : Character
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case CharState.Walk:
                WalkUpdate();
                break;
            case CharState.WalkToEnemy:
                WalkToEnemyUpdate();
                break;
            case CharState.Attack:
                AttackUpdate();
                break;
            case CharState.WalkToMagicCast:
                WalkToMagicCastUpdate();
                break;
            case CharState.WalkToNPC:
                WalkToNPCUpdate();
                break;
        }
    }

    protected void WalkToNPCUpdate()
    {
        float distance = Vector3.Distance(transform.position, curCharTarget.transform.position);

        if (distance <= 2f)
        {
            navAgent.isStopped = true;
            SetState(CharState.Idle);

            NPC npc = curCharTarget.GetComponent<NPC>();

            uiManager.PrepareDialogueBox(npc);

            // if (npc != null)
            // {
            //     if (npc.IsShopKeeper)
            //         uiManager.PrepareShopPanel(npc, this);
            //     else
            //         uiManager.PrepareDialogueBox(npc);
            // }
            // else
            // {
            //     Hero hero = curCharTarget.GetComponent<Hero>();
            //     uiManager.PrepareHeroJoinParty(hero);
            // }
        }
    }
}
