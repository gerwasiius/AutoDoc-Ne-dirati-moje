using NodaTime.TimeZones;

namespace AutoDocService.API.ServiceInterfaces
{
    public interface ITimeZoneHelper
    {
        DateTime ConvertToLocalTime(DateTime utcDateTime, string timeZoneId);
        DateTime ConvertToUtc(DateTime localDateTime, string timeZoneId, ZoneLocalMappingResolver resolver = null);
        IReadOnlyCollection<string> GetTimeZoneList();
    }
}
