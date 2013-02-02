using System;

namespace JobSystem.Framework
{
    public class AppDateTime
    {
        public static Func<DateTime> GetUtcNow = () => DateTime.UtcNow;
    }
}