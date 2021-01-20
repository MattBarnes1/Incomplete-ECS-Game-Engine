
using Engine.Components;
using System;

public class TimerComponent : Component
{
    TimeData myResetData;  
    TimeData myTimerData;
    
    bool ResetAutomaticallyOnZero;
    public bool isPaused { get; set; }

    public TimerComponent Copy()
    {
        return new TimerComponent(myResetData.Copy());
    }
    public TimerComponent() { }
    public TimerComponent(TimeData Timer) {
        this.myResetData = Timer;
    
    }

    public TimerComponent(int Day, int Hours, int Minutes, int Seconds, bool shouldAutoReset, bool FireOnce)
    {
        myResetData = new TimeData(Day, Hours, Minutes, Seconds);
        ResetAutomaticallyOnZero = shouldAutoReset;
        this.FireOnce = FireOnce;
        myTimerData = new TimeData(Day,Hours,Minutes,Seconds);
    }

    public void SetTimer(int Day, int Hours, int Minutes, int Seconds, bool shouldAutoReset, bool FireOnce)
    {
        myResetData = new TimeData(Day, Hours, Minutes, Seconds);
        this.FireOnce = FireOnce;
        ResetAutomaticallyOnZero = shouldAutoReset;
        myTimerData = new TimeData(Day, Hours, Minutes, Seconds);
    }

    public void ResetTimer()
    {
        myTimerData.setTimeData(myResetData);
    }

    public bool isZero()
    {
        return myTimerData.isNegative() || myTimerData.isZero();       
    }
    
    private bool FireOnce;

    public bool ShouldDestroy { get
        {
            return shouldDestroy || (isZero() && !ResetAutomaticallyOnZero && FireOnce);
        }
    }

    bool shouldDestroy = false;

    public void Destroy()
    {
        shouldDestroy = true;
    }

    public String getTimeString()
    {
        return myTimerData.getTimeString12Hour();
    }

    public virtual void timerUpdate(float amount) //have to use amount to sync it with game time.
    {
        if(!isPaused && !isZero())
        {
            myTimerData.SubtractMS(amount);            
        }
    }

}
