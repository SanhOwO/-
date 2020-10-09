using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Jobs;
using GameEngine.Instance;

namespace GameEngine.Instance
{
    public class InstanceMonoAuto<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).ToString();
                    DontDestroyOnLoad(obj);
                    instance = obj.AddComponent<T>();
                }
                return instance;
            }

        }
    }
}


