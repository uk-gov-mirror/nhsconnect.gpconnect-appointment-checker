﻿using System;
using System.Runtime.CompilerServices;

namespace gpconnect_appointment_checker.Helpers
{
    public static class DateTimeExtensions
    {
        public static double DurationBetweenTwoDates(this DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null || endDate == null) return 0;
            var durationTimeSpan = endDate - startDate;
            return durationTimeSpan.Value.TotalMinutes;
        }
    }
}