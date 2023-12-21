using System;
using System.Collections.Generic;

namespace Calendar;

public interface IRequestManager
{
    string GetDayType(DateTime date);

    void InsertCalendar(int year, List<char> dayTypes);
    
    bool IsDatabaseConnected();
}