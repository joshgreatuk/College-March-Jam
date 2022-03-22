using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;
using QuestSystem;

namespace QuestSystem
{
    public class QuestLog : MonoBehaviour
    {
        public List<Quest> questList = new List<Quest>();

        private void FinishObjective(Quest quest, Objective objective)
        {

        }

        private void FinishQuest(Quest quest)
        {

        }

        public void M_KillEnemy(EnemyType enemy)
        {
            foreach (Quest quest in questList)
            {
                foreach (Objective objective in quest.objList)
                {
                    if (objective.type == ObjectiveType.KillEnemies && objective.target == enemy)
                    {   
                        objective.killStatus += 1;
                        if (objective.killStatus >= objective.killTarget)
                        {
                            //Finish objective
                        }
                    }
                }
            }
        }

        public void M_GatherItem()
        {

        }

        public void M_VisitArea()
        {

        }

        public void M_TalkToNPC()
        {

        }
    }
}