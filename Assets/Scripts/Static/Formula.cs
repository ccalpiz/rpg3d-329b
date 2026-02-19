using Unity.VisualScripting;
using UnityEngine;

public class Formula : MonoBehaviour
{
    public static Character FindClosetEnemyChar(Character me)
    {
        LayerMask charLayer = LayerMask.GetMask("Character");
        Character closetTarget = null;
        float closetDist = 0f;

        RaycastHit[] hits = Physics.SphereCastAll(me.transform.position, me.FindingRange, Vector3.up, charLayer);

        for (int i = 0; i < hits.Length; i++)
        {
            Character target = hits[i].collider.GetComponent<Character>();

            if (target == null || target.CurHp <= 0 || target == me)
                continue;
            if (!me.IsMyEnemy(target.tag))
                continue;

            float distance = Vector3.Distance(me.transform.position, hits[i].transform.position);
            
            if (closetTarget == null || distance < closetDist)
            {
                closetTarget = target;
                closetDist = distance;
            }
        }
        return closetTarget;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
