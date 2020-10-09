using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using GameEngine.Instance;

namespace GameEngine.Instance { 
    public class InstanceNull<T> where T:new()
    {
        // Start is called before the first frame update
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                    instance = new T();
                return instance;
            }
        }
    }
}

 
