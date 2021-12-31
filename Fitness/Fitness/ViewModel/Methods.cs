using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using Fitness.Model;
using Fitness.Windows;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace Fitness.ViewModel
{
    public static class Methods
    {
        /// <summary>
        /// Экспорт данных в json
        /// </summary>
        /// <param name="obj">Колекция объектов</param>
        public static void ExportUserData(object obj)
        {
            SaveFileDialog saveFileDialog = new()
            {
                Filter = "Json file (*.json)|*.json",
                FileName = "UserData"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                var fileName = saveFileDialog.FileName;
                using StreamWriter sw = new(fileName);
                var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
                sw.WriteLine(json);
            }
        }
        
        /// <summary>
        /// Загрузка данных из json файлов
        /// </summary>
        /// <returns>Возвращает колекцию UserData</returns>
        public static IEnumerable<UserData> LoadData(IEnumerable<UserData> userDataOld)
        {
            OpenFileDialog openFileDialog = new()
            {
                Multiselect = true,
                Filter = "Json file (*.json)|*.json|All files |*.*"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                List<UserData> userDatas = new List<UserData>();
                List<JsonData> jsonData = new();

                userDataOld.SelectMany(x => x.DaysData, (user, dayData) => new
                { user.User, dayData.Rank, dayData.Status, dayData.Steps, dayData.Day })
                    .ToList().ForEach(x => jsonData.Add(new JsonData(x)));

                foreach (var fileName in openFileDialog.FileNames)
                {
                    string jsonString = File.ReadAllText(fileName);
                    var list = JsonParser(jsonString);
                    if (list is not null)
                    {
                        var numberDay = NumberDay(fileName);
                        if (numberDay is > 0 and < 31)
                        {
                            list.ToList().ForEach(x => x.Day = numberDay);
                            jsonData.AddRange(list);
                        }
                        else MessageBox.Show("Не верный номер дня. Данные не загружены");
                    }
                }
                jsonData.GroupBy(x => x.User).GroupJoin(
                        jsonData,
                        t => t.Key,
                        p => p.User,
                        (q, w) => new
                        {
                            User = q.Key,
                            Avr = (int)w.Average(x => x.Steps),
                            Min = w.Min(x => x.Steps),
                            Max = w.Max(x => x.Steps),
                            DailyDatas = w.Select(i => new { i.Day, i.Rank, i.Status, i.Steps })
                        })
                    .ToList()
                    .ForEach(x => userDatas.Add(new UserData(x)));
                return userDatas;
            }
            return null;
        }
        /// <summary>
        /// Получение номера из азвания файла или непосредственно из ввода
        /// </summary>
        /// <param name="str">Путь файла</param>
        /// <returns>Номер дня месяца</returns>
        public static int NumberDay(string str)
        {
            string name = new FileInfo(str).Name;

            var numberList = Regex.Matches(name, "\\d+")
                .Select(x => int.Parse(x.Value))
                .ToList();

            if (numberList.Count == 0)
            {
                MessageWindow messageWindow = new MessageWindow(name);
                if (messageWindow.ShowDialog() == true)
                {
                    return messageWindow.NumberDay;
                }
            }
            else
            {
                return numberList.First();
            }
            return 0;
        }
        /// <summary>
        /// Валидация и парсинг json
        /// </summary>
        /// <typeparam name="T">Тип колекции</typeparam>
        /// <param name="jsonString">Путь файла</param>
        /// <returns>Полученная колекция</returns>
        public static IEnumerable<JsonData> JsonParser(string jsonString)
        {
            try
            {
                JsonTextReader reader = new JsonTextReader(new StringReader(jsonString));
                JSchema schema = JSchema.Parse(File.ReadAllText(@"json-schema.json"));
                JSchemaValidatingReader validatingReader = new JSchemaValidatingReader(reader);
                validatingReader.Schema = schema;
                JsonSerializer serializer = new JsonSerializer();
                return serializer.Deserialize<IEnumerable<JsonData>>(validatingReader);
            }
            catch (JSchemaValidationException ex)
            {
                MessageBox.Show("Ошибка: Не верный формат данных\n" + ex.Message);
            }
            return null;
        }
    }
}
