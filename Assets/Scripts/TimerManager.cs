using System;
using System.Collections.Generic;
using Pool;
using Unity.Mathematics;
using UnityEngine;

public enum TimerType
{
    DrawCard,
    TriggerBuilding,
    ConsumeResource,
    GenerateFlyingItem,
    
}

public class TimerManager : Singleton<TimerManager>
{
    public Dictionary<TimerType, Timer> timers = new Dictionary<TimerType, Timer>();

    public Dictionary<TimerType, int> timerValues()
    {
        Dictionary<TimerType, int> res = new Dictionary<TimerType, int>();
        foreach (var timerPair in timers)
        {
            res[timerPair.Key] = (int)timerPair.Value.TimeRemaining;
        }

        return res;
    }

    public bool IsTimerRunning(TimerType type)
    {
        return timers[type].IsRunning;
    }

    public void Init()
    {
        foreach (var timerInfo in CSVLoader.Instance.timerDict.Values)
        {
            var timerType = Enum.Parse<TimerType>(timerInfo.identifier);
            var timer = CreateTimer(timerType, timerInfo, timerInfo);
        }
    }

    private float logTimer = 0;
    private float logTime = 1;
    private int playedTime = 0;
    public int PlayedTime => playedTime;
    void Update()
    {
        logTimer += Time.deltaTime;
        if (logTimer >= logTime)
        {
            logTimer -= logTime;
            
            playedTime += (int)logTime;
            //GameAnalyticsManager.Instance.LogTime(SaveDataManager.Instance.data.playedTime);
        }
        
        
        bool anyTimerRunning = false;
        foreach (var timer in timers.Values)
        {
            if (timer.IsRunning)
            {
                anyTimerRunning = true;
            }

            timer.Update(Time.deltaTime);
        }

        // if (anyTimerRunning)
        // {
        //     SaveDataManager.Instance.data.timers = timerValues();
        // }

        if (Input.GetKeyDown(KeyCode.A))
        {
            foreach (var timer in timers)
            {
                Debug.Log($"{timer.Key}: {timer.Value.TimeRemaining} {timer.Value.IsRunning}");
            }
        }

        //UpdateWhenCrossMidnight();
    }

    // public void UpdateWhenCrossMidnight()
    // {
    //     
    //     if (HasCrossedMidnight())
    //     {
    //         // 执行跨越午夜后的操作
    //         SaveDataManager.Instance.data.currentValuePerDayDict.Clear();
    //         foreach (var timer in timers)
    //         {
    //             if (timer.Value.Info.shouldResetEveryday)
    //             {
    //                 timer.Value.Reset();
    //             }
    //         }
    //         EventPool.Trigger(EventPoolNames.UpdateDay);
    //     }
    // }
    // public DateTime lastCheckedDate;
    // public bool HasCrossedMidnight()
    // {
    //     DateTime currentDate = DateTime.Now.Date;
    //
    //     // 如果当前日期与上次检查的日期不同，说明已经跨越了午夜
    //     if (currentDate != lastCheckedDate)
    //     {
    //         // 更新记录的日期
    //         lastCheckedDate = currentDate;
    //         // SaveDataManager.Instance.data.lastCheckedDate =  lastCheckedDate.ToString("o");
    //         // SaveDataManager.Instance.SaveData();
    //         return true;
    //     }
    //     return false;
    // }
    public Timer GetTimer(TimerType type)
    {
        return timers[type];
    }

    public void RestartTimer(TimerType type)
    {
        var timer = GetTimer(type);
        if (!timer.IsRunning)
        {
            timer.Restart();
        }
    }

    public void CheckAllTimerShouldStart()
    {
        foreach (var timerInfo in CSVLoader.Instance.timerDict.Values)
        {
            var timerType = Enum.Parse<TimerType>(timerInfo.identifier);
            var timer = GetTimer(timerType);

            CheckTimerShouldStart(timer, timerType);
            // {
            // timer.Start();
            // }
        }
    }

    public void CheckTimerShouldStart(Timer newTimer, TimerType type)
    {
        if (!newTimer.IsRunning)
        {
            
            newTimer.Start();
        }
        switch (type)
        {
            // case TimerType.Dice:
            //     if (!ResourceManager.Instance.IsMoreThanMax(ResourceType.Dice))
            //     {
            //         newTimer.Start();
            //     }
            //
            //     break;
            //
            // case TimerType.DailyChestProgress:
            //     if (!DailyChestManager.Instance.HasFullChest())
            //     {
            //         newTimer.Start();
            //     }
            //
            //     break;
            // case TimerType.treasureGoblin:
            //     if (!GridCellItemGenerationManager.Instance.hasEnoughItems(GridCellItemType.treasureGoblin))
            //     {
            //         newTimer.Start();
            //     }
            //
            //     break;
            // case TimerType.treasureChest:
            //     if (!GridCellItemGenerationManager.Instance.hasEnoughItems(GridCellItemType.treasureChest))
            //     {
            //         newTimer.Start();
            //     }
            //
            //     break;
            // case TimerType.FlyChest:
            //     newTimer.Start();
            //     break;
            // case TimerType.EquipmentGachaAds:
            // case TimerType.MinionGachaAds:
            // case TimerType.SkillGachaAds:
            //     if (newTimer.TimeRemaining != newTimer.Duration)
            //     {
            //         newTimer.Start();
            //     }
            //         break;
        }
    }

    public Timer CreateTimer(TimerType type, TimerInfo info, TimerInfo timerInfo)
    {
        Timer newTimer = new Timer(info);
        timers[type] = newTimer;

        newTimer.OnComplete += () =>
        {
            EventPool.Trigger(type + "TimerComplete");
            newTimer.Reset();
            switch (type)
            {
                case TimerType.DrawCard:
                    FindObjectOfType< SelectCardMenu>().Show();
                    break;
                case TimerType.TriggerBuilding:
                    BuildingManager.Instance.TriggerAllBuildings();
                    break;
                case TimerType.ConsumeResource:
                    ResourceManager.Instance.RemoveGold(1);
                    break;
                case TimerType.GenerateFlyingItem:
                    break;
            }
                
            

            CheckTimerShouldStart(newTimer, type);
        };

        newTimer.OnTick += (time) =>
        {
            EventPool.Trigger(type + "TimerTick");
            // switch (type)
            // {
            //     case TimerType.Dice:
            //         if (ResourceManager.Instance.IsMoreThanMax(ResourceType.Dice))
            //         {
            //             newTimer.Stop();
            //         }
            //
            //         break;
            //
            //     case TimerType.DailyChestProgress:
            //         if (DailyChestManager.Instance.HasFullChest())
            //         {
            //             newTimer.Stop();
            //         }
            //
            //         break;
            // }
        };
        //newTimer.Start();
        return newTimer;
    }

    // public void RemoveTimer(Timer timer)
    // {
    //     if (timers.Contains(timer))
    //     {
    //         timer.Stop();
    //         timers.Remove(timer);
    //     }
    // }

    // public void ProcessOfflineDuration(TimeSpan offlineDuration)
    // {
    //     float offlineSeconds = (float)offlineDuration.TotalSeconds;
    //     foreach (var timer in timers.Values)
    //     {
    //         var remainingSeconds = offlineSeconds;
    //         while (remainingSeconds > 0)
    //         {
    //             remainingSeconds = timer.ProcessOfflineTime(remainingSeconds);
    //         }
    //     }
    // }
    //
    // public float TimeBeforeFull(TimerType type)
    // {
    //     var timer = timers[type];
    //     var info = CSVLoader.Instance.timerDict[type.ToString()];
    //     //得到当前这个达成需要的时间
    //     var currentTime =timer.IsRunning? timer.TimeRemaining:0;
    //     //得到剩下达成需要的时间
    //     var maxValue = 0;
    //     var currentValue = 0;
    //     var remainTime = 0;
    //     var valuePerTimer = info.values[0];
    //     switch (type)
    //     {
    //         // case TimerType.Dice:
    //         //     currentValue = ResourceManager.Instance.GetResource(ResourceType.Dice);
    //         //     maxValue = ResourceManager.Instance.GetResourceMaxCount((ResourceType.Dice));
    //         //     var remainValue =  maxValue - currentValue;
    //         //     if (remainValue <= valuePerTimer)
    //         //     {
    //         //     }
    //         //     else
    //         //     {
    //         //         
    //         //         var leftCount = (int)(math.ceil((float)remainValue / valuePerTimer)) - 1;
    //         //         if (leftCount > 0)
    //         //         {
    //         //         
    //         //             remainTime = leftCount* info.time;
    //         //         }
    //         //     }
    //         //
    //         //     break;
    //         // case TimerType.DailyChestProgress:
    //         //     currentValue = DailyChestManager.Instance.chests.Count;
    //         //     maxValue = DailyChestManager.Instance.StoredChestMaxCount;
    //         //     var chestLeft = maxValue - currentValue-1;
    //         //     if (chestLeft <= 1)
    //         //     {
    //         //         return 0;
    //         //     }
    //         //
    //         //     var chestLeftProgress = (chestLeft-2) * DailyChestManager.Instance.oneChestPoint;
    //         //     var currentChestLeft = DailyChestManager.Instance.oneChestPoint-
    //         //         ResourceManager.Instance.GetResourceCurrentCount(ResourceType.DailyChestProgress);
    //         //     chestLeftProgress += currentChestLeft;
    //         //     remainTime = (int)(math.ceil(chestLeftProgress/valuePerTimer) )* info.time;
    //         //     break;
    //         // default:
    //         //     Debug.LogError(("Unhandled TimerType"));
    //         //     break;
    //     }
    //    // remainValue = math.max(0, remainValue);
    //     
    //     return math.max(0,remainTime + currentTime);
    // }
}