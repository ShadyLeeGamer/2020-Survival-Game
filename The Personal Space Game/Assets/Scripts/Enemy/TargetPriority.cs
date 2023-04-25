using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPriority : MonoBehaviour
{
    public List<GameObject> priority;
    public List<GameObject> fixedPriority;

    public EnemyMovement enemy;

    public void PriorityManager()
    {
        priority = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));

        for (int i = 0; i < priority.Count; i++)
        {
            if (priority[i])
            {
                if (!priority[i].GetComponent<TargetPriority>() && priority[i].GetComponent<EnemyMovement>().type != 3 && !priority[i].GetComponent<EnemyMovement>().coronaMode)
                {
                    if (!fixedPriority.Contains(priority[i]))
                        fixedPriority.Add(priority[i]);
                }
            }
            else
                fixedPriority.Remove(priority[i]);
        }

        if (fixedPriority.Count > 0)
        {
            for (int i = 0; i < fixedPriority.Count; i++)
            {
                if (fixedPriority[i])
                {
                    if (!fixedPriority[i].GetComponent<EnemyMovement>().coronaMode)
                    {
                        if (fixedPriority[i].GetComponent<EnemyMovement>().HP > 0 && Vector3.Distance(transform.position,
                                                                                      fixedPriority[i].transform.position) <= 1000)
                            enemy.target = fixedPriority[i].transform;
                    }
                    else
                    {
                        fixedPriority.Remove(fixedPriority[i]);
                        enemy.target = enemy.player.transform;
                    }
                }
                else
                    fixedPriority.Remove(fixedPriority[i]);
            }
        }
        else
            enemy.target = enemy.player.transform;
    }

}
