using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using QuestSystem;
using AI;
using Inventory;

public class M_KillEnemy : UnityEvent<EnemyType>{}
public class M_GatherItem : UnityEvent<InventoryItem>{}
public class M_VisitArea : UnityEvent{}
public class M_TalkToNPC : UnityEvent<NPCClass>{}

public class EventHandler : MonoBehaviour
{
    public static EventHandler instance;

    private void Awake() 
    {
        instance = this;
        E_KillEnemy = new M_KillEnemy();
        E_GatherItem = new M_GatherItem();
        E_VisitArea = new M_VisitArea();
        E_TalkToNPC = new M_TalkToNPC();
    }

    public M_KillEnemy E_KillEnemy;
    public M_GatherItem E_GatherItem;
    public M_VisitArea E_VisitArea;
    public M_TalkToNPC E_TalkToNPC;
}