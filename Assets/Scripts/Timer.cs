using System;
using UnityEngine;

[Serializable]
public class Timer
{
    public float Duration { get;  set; }
    public float TimeRemaining { get; private set; }
    public bool IsRunning { get; private set; }
    private TimerInfo info;
    public TimerInfo Info => info;

    private int lastSecond;

    public event Action<float> OnTick;  // 每秒更新事件
    public event Action OnComplete;     // 完成事件

    public Timer(TimerInfo info)
    {
        this.info = info;
        Duration = info.time;
        
        Reset();
    }

    public void SetTimeRemaining(float timeRemaining)
    {
        TimeRemaining = timeRemaining;
    }
    
    public void Update(float deltaTime)
    {
        if (IsRunning)
        {
            TimeRemaining -= deltaTime;
            int currentSecond = Mathf.FloorToInt(TimeRemaining);

            if (currentSecond != lastSecond) // 仅当秒数发生变化时才触发
            {
                OnTick?.Invoke(TimeRemaining);
                lastSecond = currentSecond;
            }

            if (TimeRemaining <= 0)
            {
                CompleteTimer();
            }
        }
    }
    
    private float AdvanceTime(float time)
    {
        time -= TimeRemaining;
        if (time >= 0)
        {
            TimeRemaining = 0;
            CompleteTimer();
            return time;
        }
        else
        {
            TimeRemaining = -time;
            return 0;
        }

    }

    private void CompleteTimer()
    {
        IsRunning = false;
        TimeRemaining = 0;
        OnComplete?.Invoke();
    }

    public void Restart()
    {
        Reset();
        Start();
    }
    public void Start()
    {
        IsRunning = true;
       // TimeRemaining = Duration;
        lastSecond = Mathf.FloorToInt(TimeRemaining);
        OnTick?.Invoke(TimeRemaining); // 在开始时立即触发一次
    }

    public void Stop()
    {
        IsRunning = false;
    }

    public void Reset()
    {
        TimeRemaining = Duration;
        lastSecond = -1; // 重置上一秒时间，确保重新开始时可以触发
        IsRunning = false;
    }
    
    public float ProcessOfflineTime(float offlineSeconds)
    {
        if (IsRunning)
        {
            return AdvanceTime(offlineSeconds);
        }
        return 0; // 没有剩余的离线时间
    }
}