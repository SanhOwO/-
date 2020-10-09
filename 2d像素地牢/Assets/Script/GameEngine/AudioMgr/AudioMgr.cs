using GameEngine.Instance;
using GameEngine.MonoTool;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class AudioMgr : InstanceNull<AudioMgr>
{
    private AudioSource bgm;
    private GameObject bgmObj;
    private float bgvolume = 0.5f;

    private GameObject soundObj;
    private List<AudioSource> soundList = new List<AudioSource>();
    private float soundvolume = 0.5f;

    public AudioMgr()
    {
        MonoGlobal.Instance.AddUpdateListener(Update);
    }
    private void Update()
    {
        foreach(var sound in soundList)
        {
            if (!sound.isPlaying)
            {
                StopSound(sound);
                break;
            }      
        }
    }
    public void PlayBGMusic(string name)
    {
        if(bgm == null)
        {
            bgmObj = new GameObject();
            bgmObj.name = "BGMAudio";
        }
        ResourcesMgr.Instance.LoadAsyn<AudioClip>("Audio/BGM/" + name, (clip)=> 
        {
            bgm = bgmObj.AddComponent<AudioSource>();
            bgm.clip = clip;
            bgm.loop = true;
            bgm.volume = bgvolume;
            bgm.Play();
        });
    }
    public void StopBGMusic()
    {
        if (bgm == null)
            return;
        bgm.Stop();
    }
    public void PauseBGMusic()
    {
        if (bgm == null)
            return;
        bgm.Pause();
    }
    public void PlaySound(string name, bool isLoop, UnityAction<AudioSource> callback = null)
    {
        if(soundObj == null)
        {
            soundObj = new GameObject();
            soundObj.name = "Sound";
        }
        //音效资源异步加载完后 在添加音效
        ResourcesMgr.Instance.LoadAsyn<AudioClip>("Audio/Other/" + name, (clip) =>
        {
            AudioSource source = soundObj.AddComponent<AudioSource>();
            source.clip = clip;
            source.loop = isLoop;
            source.volume = bgvolume;
            source.Play();
            soundList.Add(source);
            //source返回给外面
            if(callback != null)
                callback(source);
        });
    }
    public void StopSound(AudioSource sources)
    {
        if (soundList.Contains(sources))
        {
            sources.Stop();
            soundList.Remove(sources);
            GameObject.Destroy(sources);
        }
    }
    public void SetBGVolume(float f)
    {
        bgvolume = f;
        if (bgm == null)
            return;
        bgm.volume = bgvolume;
    }
    public void SetVolume(float f)
    {
        soundvolume = f;
        foreach(var s in soundList)
        {
            s.volume = soundvolume;
        }
    }
    
}
