
using System;
using System.Collections;
using System.Collections.Generic;

[TestReminder("TimeData")]
public class TimeData
{
    public static TimeData Zero;
    public int Days { get; private set; }
    public int Hours { get; private set; }
    public int Minutes { get; private set; }
    public int Seconds { get; private set; }

    internal TimeData Copy()
    {
        return (TimeData)this.MemberwiseClone();
    }

    public float Milliseconds { get; private set; }

    bool isAM = false;
    bool is24Hour = true;
    private int day;

    public TimeData(int day, int hours, int minutes, int seconds)
    {
        this.day = day;
        Hours = hours;
        Minutes = minutes;
        Seconds = seconds;
    }




    //Time References a specific time in history;    
    public virtual void setTimeData(int Day, int Hours, int Minutes, int Seconds, float Milliseconds = 0)
    {
        this.Milliseconds = Milliseconds;
        this.Seconds = Seconds;
        this.Days = Day;
        this.Hours = Hours;
        this.Minutes = Minutes;
        CorrectTime();
    }

    internal void AddMS(float elapsedTime)
    {
        this.Milliseconds += elapsedTime;
        CorrectTime();
    }

    public bool GreaterEqualTo(TimeData time)
    {
        return (GreaterThan(time) || EqualTo(time));
    }

    public void SubtractMS(float amount)
    {
        this.Milliseconds -= amount;
        CorrectTime();
    }

    public bool LessOrEqualTo(TimeData time)
    {
        return (LesserThan(time) || EqualTo(time));
    }



    public static bool operator==(TimeData A, TimeData B)
    {
        return (A.Hours == B.Hours && A.Minutes == B.Minutes && A.Seconds == B.Seconds);

    }

    internal void setTimeData(TimeData myResetData)
    {
        this.Milliseconds = myResetData.Milliseconds;
        this.Seconds = myResetData.Seconds;
        this.Minutes = myResetData.Minutes;
        this.Hours = myResetData.Hours;
        this.Days = myResetData.Days;
    }

    public static bool operator !=(TimeData A, TimeData B)
    {
        return !(A == B);
    }

    public bool EqualTo(TimeData time)
    {
        return (this.Hours == time.Hours && this.Minutes == time.Minutes && this.Seconds == time.Seconds);
    }

    public bool LesserThan(TimeData time)
    {
        if (this.Hours < time.Hours)
        {
            return true;
        }
        else if (this.Hours == time.Hours)
        {
            if (this.Minutes < time.Minutes)
            {
                return true;
            }
            else if (this.Minutes == time.Minutes)
            {
                if (this.Seconds < time.Seconds)
                {
                    return true;
                }
                else if (this.Seconds == time.Seconds)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool GreaterThan(TimeData time)
    {
        return !LesserThan(time);
    }



    public float toMS()
    {

        return (((21600000 * this.Hours + Minutes * 360000) + Seconds * 60000) + Milliseconds);
    }

    public void Subtract(float AmountInMS)
    {
        this.Milliseconds -= AmountInMS;
        CorrectTime();
    }


    //if less than the other just return 0 because you can't have negative times
    public TimeData subtract(TimeData lastUpdateCall)
    {
        int localYear = 0;
        int localMonth = 0;
        int localDay = 0;
        int localHours = 0;
        int localMinutes = 0;
        int localSeconds = 0;
        float localMSeconds = 0;
        TimeData myReturn;
        if (lastUpdateCall.LesserThan(this) || this.EqualTo(lastUpdateCall))
        {

            localMSeconds = this.Milliseconds - lastUpdateCall.Milliseconds;
            localSeconds = this.Seconds - lastUpdateCall.Seconds;
            localMinutes = this.Minutes - lastUpdateCall.Minutes;
            localHours = this.Hours - lastUpdateCall.Hours;
            myReturn = new TimeData(localDay,localHours,localMinutes,localSeconds);
            return myReturn;
        } 
        else
        {
            return new TimeData(0, 0, 0, 0);
        }
    }

    public bool isNegative()
    {
        return (this.Days < 0 || this.Hours < 0 || this.Minutes < 0 || this.Seconds < 0);
    }
    public bool isZero()
    {
        return ( this.Days <= 0 && this.Hours <= 0 && this.Minutes <= 0 && this.Seconds <= 0);
    }





    public String getTimeString()
    {
        if (Minutes >= 10)
        {
            return Hours + ":" + Minutes;
        }
        else
        {
            return Hours + ":0" + Minutes;
        }
    }


    public String getTimeString12Hour()
    {
        String retString;
        if (Hours < 12 && Hours > 0)
        {
            retString = Hours + ":" + CorrectMinutes() + " am";
        }
        else if (Hours == 12)
        {
            retString = Hours + ":" + CorrectMinutes() + " pm";
        }
        else if (Hours != 0)
        {
            retString = (Hours - 12) + ":" + CorrectMinutes() + " pm";
        }
        else
        {
            retString = "12:" + CorrectMinutes() + " am";
        }
        return retString;
    }

    private String CorrectMinutes()
    {
        if (Minutes >= 10)
        {
            return Minutes + "";
        }
        else
        {
            return "0" + Minutes;
        }
    }


    protected void CorrectTime()
    {
        while (Milliseconds >= 1000)
        {
            int Remainder = (int)Seconds % 1000;
            int totalSeconds = (int)((Seconds - Remainder) / 1000);
            Milliseconds = (short)Remainder;
            Seconds += totalSeconds;
        }
        while (Milliseconds < 0)
        {
            float oldMS = Milliseconds;
            Seconds--;
            Milliseconds = Milliseconds + oldMS;
        }
        while (Seconds > 59)
        {
            int Remainder = (int)Seconds % 60;
            int totalMinutes = (int)((Seconds - Remainder) / 60);
            Seconds = (short)Remainder;
            Minutes += totalMinutes;
        }
        while(Seconds < 0) //Done for timers going down
        {
            int oldSeconds = Seconds;
            Minutes--;
            Seconds = 59 + oldSeconds;
        }
        while (Minutes > 59)
        {
            int Remainder = Minutes % 60;
            int totalHours = (int)((Minutes - Remainder) / 60);
            Minutes = (short)Remainder;
            Hours += totalHours;
        }
        while(Minutes < 0)
        {
            Hours--;
            Minutes = 59;
        }
        while (Hours > 23)
        {
            int Remainder = Hours % 24;
            int totalDays = (int)((Hours - Remainder) / 24);
            Hours = (short)Remainder;
            Days += totalDays;
        }
        while (Hours < 0)
        {
            if(Days == 0)
            {
                Hours = 0;
                Minutes = 0;
                Seconds = 0;
                Days = 0;
            }
            else
            {
                Days--;
                Hours = 23 + Hours;
            }
        }



        /*while (currentDayNumber > CalendarData.numberOfDaysPerCalendarMonth[currentMonthNameIndex])
        {
            int Remainder = 0;
            if (currentDayNumber - CalendarData.getDaysPerMonthByIndex(currentMonthNameIndex) > 0)
            {
                currentDayNumber = -CalendarData.getDaysPerMonthByIndex(currentMonthNameIndex);
            }
            else
            {
                currentDayNameIndex = currentDayNumber % CalendarData.CalendarDayNames.Length;
            }
            if (currentMonthNameIndex + 1 > CalendarData.getTotalMonths() - 1)
            {
                Years++;
                currentMonthNameIndex = 0;
            }
            else
            {
                currentMonthNameIndex++;
            }
        }*/
    }

    public TimeData multiply(double Ratio)
    {
        TimeData myReturn = new TimeData((int)Math.Floor(this.Days * Ratio), (int)Math.Floor(this.Hours * Ratio), (int)Math.Floor(this.Minutes * Ratio), (int)Math.Floor(this.Seconds * Ratio));
        return myReturn;
    }




}
