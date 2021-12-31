using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fitness.Model;
using Newtonsoft.Json;

namespace Fitness.Model
{
    public class UserData
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        [JsonProperty]
        public string User { get; set; }
        /// <summary>
        /// Среднее количество пройденных шагов за весь период
        /// </summary>
        [JsonProperty]
        public int AverageSteps { get; set; }
        /// <summary>
        /// Худший результат за весь период
        /// </summary>
        [JsonProperty]
        public int MinSteps { get; set; }
        /// <summary>
        /// Лучший результат за весь период
        /// </summary>
        [JsonProperty]
        public int MaxSteps { get; set; }
        /// <summary>
        /// Данные по дням
        /// </summary>
        [JsonProperty]
        public List<DayData> DaysData { get; set; }
        [JsonIgnore]
        public string Color { get; set; }

        public UserData()
        {
            Color = (float)MaxSteps / AverageSteps > 1.2 || (float)AverageSteps / MinSteps > 1.2
                ? "DarkOrange"
                : "Black";
        }

        public UserData(string user, int averageSteps, int minSteps, int maxSteps, List<DayData> daysData)
        {
            User = user;
            AverageSteps = averageSteps;
            MinSteps = minSteps;
            MaxSteps = maxSteps;
            DaysData = daysData;
            Color = (float)MaxSteps / AverageSteps > 1.2 || (float)AverageSteps / MinSteps > 1.2
                ? "DarkOrange"
                : "Black";
        }

        public UserData(object item)
        {
            User = (string)item.GetType().GetProperty("User")?.GetValue(item, null);
            AverageSteps = (int)item.GetType().GetProperty("Avr").GetValue(item, null);
            MinSteps = (int)item.GetType().GetProperty("Min").GetValue(item, null);
            MaxSteps = (int)item.GetType().GetProperty("Max").GetValue(item, null);
            DaysData = new List<DayData>();
            (((IEnumerable<object>)item.GetType().GetProperty("DailyDatas")?.GetValue(item, null)) ?? Array.Empty<object>())
                .ToList()
                .ForEach(x => DaysData.Add(new DayData(x)));

            Color = (float) MaxSteps / AverageSteps > 1.2 || (float) AverageSteps / MinSteps > 1.2
                ? "DarkOrange"
                : "Black";

        }
    }
}
