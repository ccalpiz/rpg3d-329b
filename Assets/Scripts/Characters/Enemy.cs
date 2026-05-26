using UnityEngine;

public class Enemy : Character
{
    protected override void Die()
    {
        base.Die();
        // Notify quest system for kill-count quests
        if (PartyManager.instance != null)
            PartyManager.instance.OnEnemyKilled(gameObject.tag);
    }

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
        }
    }
}
