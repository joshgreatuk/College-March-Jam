using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttackSystem
{
    public enum AttackList
    {
        None,
        A_Punch,
        A_StrongPunch,
        A_ThrowRock
    }

    public class AttackCollection
    {
        public static Attack GetAttack(AttackList attackToPass)
        {
            Attack attackClass = null;
            switch(attackToPass)
            {
                case AttackList.A_Punch:
                    attackClass = new A_Punch();
                    break;
                case AttackList.A_ThrowRock:
                    attackClass = new A_ThrowRock();
                    break;
                case AttackList.A_StrongPunch:
                    attackClass = new A_StrongPunch();
                    break;
            }
            return attackClass;
        }

        public class A_Punch : Attack
        {
            public  A_Punch()
            {
                attackName = "Punch";
                attackType = AttackType.Melee;
                attackCooldown = 0.75f;
                attackDamage = 5;
                attackRange = 2f;
                attackStun = 1f;
            }
        }

        public class A_StrongPunch : Attack
        {
            public  A_StrongPunch()
            {
                attackName = "Strong Punch";
                attackType = AttackType.Melee;
                attackCooldown = 3f;
                attackDamage = 20;
                attackRange = 2f;
                attackStun = 3f;
            }
        }

        public class A_ThrowRock : Attack
        {
            public A_ThrowRock()
            {
                attackName = "Throw Rock";
                attackType = AttackType.Ranged;
                attackCooldown = 10;
                attackDamage = 25;
                attackRange = 15f;
                attackStun = 1f;
            }
        }
    }
}