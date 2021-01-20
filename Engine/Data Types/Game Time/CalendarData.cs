
using System;
using System.Collections;
using System.Collections.Generic;


public static class CalendarData
{

    public static String[] CalendarDayNames = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
    public static String[] CalendarMonthNames = { "Month 1", "Month 2", "Month 3" };
    public static short[] numberOfDaysPerCalendarMonth = { 30, 30, 30 };

    private static int currentYear = 0;
    private static int currentMonthNameIndex = 0;
    private static int currentDayNameIndex = 0;
    private static int currentDayNumber = 1;



    private static int resetDay;
    private static int resetMonth;
    private static int resetYear;

    public static void setStart(int currentMonthIndex, int CurrentDay, int CurrentYear)
    {
        resetMonth = currentMonthNameIndex = currentMonthIndex;
        resetYear = currentYear = CurrentYear;
        resetDay = CurrentDay;
        addDays(CurrentDay);
    }


    public static void addDay()
    {
        addDays(1);
    }

    public static void addDays(int amount)
    {
        currentDayNumber += amount;
        while (currentDayNumber > CalendarData.numberOfDaysPerCalendarMonth[currentMonthNameIndex])
        {
            if (currentDayNumber - CalendarData.numberOfDaysPerCalendarMonth[currentMonthNameIndex] > 0)
            {
                currentDayNumber = currentDayNumber - CalendarData.numberOfDaysPerCalendarMonth[currentMonthNameIndex];
                if (currentMonthNameIndex + 1 >= CalendarData.CalendarMonthNames.Length)
                {
                    currentMonthNameIndex = 0;
                    currentYear++;
                }
                else
                {
                    currentMonthNameIndex++;
                }
            }
        }

        currentDayNameIndex = currentDayNumber % CalendarData.CalendarDayNames.Length;
    }

    private static void IncrementCalender()
    {
        if (currentDayNumber + 1 > CalendarData.numberOfDaysPerCalendarMonth[currentMonthNameIndex])
        {
            currentDayNumber = 0;
            currentDayNameIndex = 0;
            if (currentMonthNameIndex + 1 > CalendarData.CalendarMonthNames.Length)
            {
                currentMonthNameIndex = 0;
                currentYear++;
            }
            else
            {
                currentMonthNameIndex++;
            }
        }
        else
        {
            currentDayNumber++;
            currentDayNameIndex = currentDayNumber % CalendarData.CalendarDayNames.Length;
        }
    }

    public static String getCalendarMonthNameByIndex(int i)
    {
        if (CalendarMonthNames.Length > i && i >= 0)
        {
            return CalendarMonthNames[i];
        }
        else
        {
            return null;
        }
    }

    public static String getCalendarDayNameByIndex(int i)
    {
        if (CalendarDayNames.Length > i && i >= 0)
        {
            return CalendarDayNames[i];
        }
        else
        {
            return null;
        }
    }

    public static short getDaysPerMonthByIndex(int i)
    {
        if (numberOfDaysPerCalendarMonth.Length > i && i >= 0)
        {
            return numberOfDaysPerCalendarMonth[i];
        }
        else
        {
            return -1;
        }
    }

    public static String getDateString()
    {
        return getDayName() + ", the " + GetNumberSuffixFormat(currentDayNumber)+" of " + getMonthName(); //TODO: DateSuffix
    }

    private static string GetNumberSuffixFormat(int currentDayNumber)
    {
        String myString = currentDayNumber.ToString();
        char lastChar = myString[myString.Length - 1];
        switch (lastChar)
        {
            case '1':
                return myString + "st";
            case '2':
                return myString + "nd";
            case '3':
                return myString + "rd";
            default:
                return "th";
        }
    }

    public static String getDayName()
    {
        return CalendarDayNames[currentDayNameIndex];
    }

    public static String getMonthName()
    {
        return CalendarMonthNames[currentMonthNameIndex];
    }

    public static int getDaysPerMonth()
    {
        return numberOfDaysPerCalendarMonth[currentMonthNameIndex];
    }

    public static int getCurrentMonthNameIndex()
    {
        return currentMonthNameIndex;
    }

    public static int getCurrentDayNumber()
    {
        return currentDayNumber;
    }

    public static int getCurrentYear()
    {
        return currentYear;
    }

    public static int getTotalMonths()
    {
        return CalendarMonthNames.Length;
    }

    public static int getDayIndex()
    {
        throw new Exception("Not Imp");//TODO:
    }

    public static int getYear()
    {
        return currentYear;
    }


    public static void reset()
    {
        currentMonthNameIndex = resetMonth;
        currentYear = resetYear;
        currentDayNumber = resetDay;
        addDays(0);// causes it to just reset
    }

}


