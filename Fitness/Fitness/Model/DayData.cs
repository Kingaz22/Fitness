using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Model
{
    public class DayData
    {
        /// <summary>
        /// Рейтинг пользователя за текущий день
        /// </summary>
        public int Rank { get; set; }
        /// <summary>
        /// Статус (завершил, отказался и др.)
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Количество пройденных шагов
        /// </summary>
        public int Steps { get; set; }
        /// <summary>
        /// День
        /// </summary>
        public int Day { get; set; }

        public DayData(int day, int rank, string status, int steps)
        {
            Day = day;
            Rank = rank;
            Status = status;
            Steps = steps;
        }

        public DayData(object item)
        {
            Day = (int)item.GetType().GetProperty("Day")?.GetValue(item, null);
            Rank = (int)item.GetType().GetProperty("Rank")?.GetValue(item, null);
            Status = (string)item.GetType().GetProperty("Status")?.GetValue(item, null);
            Steps = (int)item.GetType().GetProperty("Steps")?.GetValue(item, null);
        }
    }
}
