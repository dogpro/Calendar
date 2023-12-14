using System;
using System.Collections.Generic;

namespace Calendar;

public interface IRequestManager
{
    DayType GetDayType(DateTime date);

    void UpdateOrInsertCalendar(int year, List<char> dayTypes);

    bool IsDatabaseConnected();
}