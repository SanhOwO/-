using DG.Tweening;
using GameEngine.Instance;
using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleAD : InstanceMonoAuto<GoogleAD>
{
    //private string id = "ca-app-pub-8673851261048499/3830172489";
    private string id = "ca-app-pub-3940256099942544/5224354917";//测试用ID
    private RewardedAd rewardedAd;
    private bool autoplay = false;
    void Start()
    {
        MobileAds.Initialize(InitOver);
    }

    private void InitOver(InitializationStatus obj)
    {
        Debug.Log("广告初始化完成");
        CreateAndLoadAD();
    }

    private void CreateAndLoadAD()
    {
        string adId;//测试需要用官方测试ID,不然会被封号
#if UNITY_ANDRIOD
            adId = id;
#else
        adId = id;
#endif
        rewardedAd = new RewardedAd(adId);
        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;
    }
    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: "
                             + args.Message);
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args.Message);
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdClosed event received");
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);
    }
    private void LoadAdSense()
    {
        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);
    }
    public void PlayAD()
    {
        //广告已经准备就绪,直接播放
        //如果因为延迟没有准备就绪,重新加载
        if(rewardedAd!= null)
        {
            if (rewardedAd.IsLoaded())
            {
                rewardedAd.Show();
                autoplay = false; 
            }
        }
        else
        {
            if (autoplay == false)
            {
                CreateAndLoadAD();
                autoplay = true;
                Debug.Log("广告对象没有生成");
            }
            else
                Debug.Log("广告加载多次失败"); 
        }
    }
    void Update()
    {
        
    }
}
