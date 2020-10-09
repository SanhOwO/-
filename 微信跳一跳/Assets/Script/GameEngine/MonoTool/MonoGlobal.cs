using GameEngine.Instance;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameEngine.MonoTool
{
    public class MonoGlobal : InstanceMonoAuto<MonoGlobal>
    {
        private event UnityAction updateEvent;
        private event UnityAction fixUpdateEvent;
        private void Start()
        {
            DontDestroyOnLoad(this);
        }

        private void Update()
        {
            if (updateEvent != null)
                updateEvent();
        }
        private void FixedUpdate()
        {
            if(fixUpdateEvent != null)
                fixUpdateEvent();
        }
        public void AddUpdateListener(UnityAction func)
        {
            updateEvent += func;
        }
        public void RemoveUpdateListener(UnityAction func)
        {
            updateEvent -= func;
        } 
        public void AddFixUpdateListener(UnityAction func)
        {

            fixUpdateEvent += func;
        }
        public void RemoveFixUpdateListener(UnityAction func)
        {
            fixUpdateEvent -= func;
        }
        public Coroutine StartCoroutineTool(IEnumerator routine)
        {
            return StartCoroutine(routine);
        }
    }
}
