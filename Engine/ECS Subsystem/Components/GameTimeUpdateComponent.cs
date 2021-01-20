
using Engine.ECS.Components.Base;
using System;
using System.Collections;
using System.Collections.Generic;

[TestReminder("GameTimeComponent", "Time Speeds/Time Data Type")]
public class GameTimeUpdateComponent : SingletonComponent<GameTimeUpdateComponent>
{
    public static float[] TimeSpeeds = new float[]{
        60f,
        1830f,
        3600f,
    };
    float TimeAcceleration = 60f;
    public  bool Pause { get; set; }
    TimeData myData = new TimeData(0,0,0,0);
    public  void PauseTime()
    {
        TimeAcceleration = 0;
    }
   
    public  void SetTime()
    {

    }

    internal  void NormalSpeed()
    {
        TimeAcceleration = TimeSpeeds[0];
    }

    internal  void MinutePerSecondSpeed()
    {
        TimeAcceleration = TimeSpeeds[1];
    }

    internal  void HoursPerSecondSpeed()
    {
        TimeAcceleration = TimeSpeeds[2];
    }



    public  float ElapsedTime { get; private set; }
    public void UpdateTime(float i)
    {
        if(!Pause)
        {
            ElapsedTime = i * TimeAcceleration;
            myData.AddMS(ElapsedTime);           
        }    
    }


   

    public  String getDateString()
    {
        return CalendarData.getDateString();
    }

   

    public  TimeData getTimeStructure()
    {
        return myData.Copy();
    }


    public bool isDaylight()
    {
        return (myData.Hours >= 7 && myData.Hours < 14);
    }

    public  bool isNightTime()
    {
        // TODO: Auto-generated method stub
        return !isDaylight();
    }

    public  int getYear()
    {
        return CalendarData.getYear();
    }



}
