// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEditor;

//DEPRECIATED FOR SCRIPTABLEOBJECT SUPPORT

// //Run in editor so that AttackCollection.cs can get the prefabs
// namespace AttackSystem
// {
//     public enum ProjectilePrefabs
//     {
//         None,
//         PR_Rock,
//         PR_Laser
//     }

//     public class ProjectileRefs : MonoBehaviour
//     {
//         public GameObject PR_Rock;
//         public GameObject PR_Laser;

//         public static ProjectileRefs instance;
//         [ExecuteAlways]
//         private void Awake() 
//         {
//             instance = this;
//         }

//         public GameObject GetProjectilePrefab(ProjectilePrefabs projectileWanted)
//         {
//             GameObject toReturn = null;
//             switch (projectileWanted)
//             {
//                 case ProjectilePrefabs.PR_Rock:
//                     toReturn = PR_Rock;
//                     break;
//                 case ProjectilePrefabs.PR_Laser:
//                     toReturn = PR_Laser;
//                     break;
//             }
//             return toReturn;
//         }
//     }
// }