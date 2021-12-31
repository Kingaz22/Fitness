using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Fitness.Model
{
    public class JsonData
    {
        /// <summary>
        /// Рейтинг пользователя за текущий день
        /// </summary>
        public int Rank { get; set; }
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string User { get; set; }
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
        [JsonIgnore]
        public int Day { get; set; }

        public JsonData()
        {

        }

        public JsonData(int day, int rank, string user, string status, int steps)
        {
            Day = day;
            Rank = rank;
            User = user;
            Status = status;
            Steps = steps;
        }

        public JsonData(object item)
        {
            User = (string)item.GetType().GetProperty("User")?.GetValue(item, null);
            Day = (int)item.GetType().GetProperty("Day")?.GetValue(item, null);
            Rank = (int)item.GetType().GetProperty("Rank")?.GetValue(item, null);
            Status = (string)item.GetType().GetProperty("Status")?.GetValue(item, null);
            Steps = (int)item.GetType().GetProperty("Steps")?.GetValue(item, null);

        }
    }
}
