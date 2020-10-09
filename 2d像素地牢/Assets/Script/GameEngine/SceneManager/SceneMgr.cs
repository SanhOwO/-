using GameEngine.Instance;
using GameEngine.MonoTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneMgr : InstanceNull<SceneMgr>
{
    //同步加载
   public void LoadSceneSyn(string sceneName, UnityAction fun)
    {
        SceneManager.LoadScene(sceneName);
        //加载完后，运行functino
        fun();
    }
    //异步加载
    public void LoadSceneAsyn(string sceneName, UnityAction fun)
    {
        MonoGlobal.Instance.StartCoroutineTool(LoadAsyn(sceneName,fun));
    }

    private IEnumerator LoadAsyn(string sceneName, UnityAction fun)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);

        while (!ao.isDone)
        {
            //事件中心 向外分发 进度情况， 外面想用就用
            EventCenter.Instance.EventTrigger("LoadScene", ao.progress);
            //这里面去更新进度条
            yield return null;
        }
        //加载完成后，才执行func
        fun?.Invoke();
    }
}
