using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttackSystem;

namespace Enemy
{
    ///ENEMY NOTES
    ///Enemies with melee attacks will follow the player and back off during a cooldown
    ///Enemies with ranged attacks will fall back to just under the attack range and if the player gets too close will swap to a melee attack

    public class EnemyClass : MonoBehaviour
    {
        public Attack mainAttack;
        public Attack secondaryAttack;
        
    }
}