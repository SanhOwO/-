using GameEngine.Instance;
using GameEngine.MonoTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMgr : InstanceNull<InputMgr>
{
    private bool isOn = false;
    public InputMgr()
    {
        MonoGlobal.Instance.AddUpdateListener(Update);
        
    }

    private void Update()
    {
        if (isOn == false)
            return;
        CheckKeyCode(KeyCode.W);
        CheckKeyCode(KeyCode.A);
        CheckKeyCode(KeyCode.S);
        CheckKeyCode(KeyCode.D);
    }

    private void CheckKeyCode(KeyCode key)
    {
        if (Input.GetKeyDown(key))
            EventCenter.Instance.EventTrigger("KeyDown", key);
        if (Input.GetKeyUp(key))
            EventCenter.Instance.EventTrigger("KeyUp", key);
    }
    //检测是否要关闭输入
    public void OnOrOffCheck(bool On)
    {
        isOn = On;
    }
}
