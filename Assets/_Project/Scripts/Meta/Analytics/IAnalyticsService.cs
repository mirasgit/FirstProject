using System.Collections.Generic;

namespace FirstProject.Meta.Analytics
{
    public interface IAnalyticsService
    {
        void LogEvent(string eventName, Dictionary<string,object> parameters = null);
    }
}