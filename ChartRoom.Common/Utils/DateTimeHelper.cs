using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Common.Utils
{
    public static class DateTimeHelper
    {
        public static DateTime LocalDateTime => DateTime.UtcNow.AddHours(8);
        public static int TimeUnixStamp => Convert.ToInt32((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds);
        public static DateTime GetLocalTimeFromUnixTimeStamp(int ts) => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddHours(8).AddSeconds(ts);
        public static DateTime GetLocalTimeFromUnixTimeStamp(double ts) => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddHours(8).AddSeconds(ts);
    }
}
