using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Areas
{
    //Added to a singleton list of scene areas, so that they can find each other
    public class AreaScene : Area 
    {
        public GameScene targetArea;
    }
}