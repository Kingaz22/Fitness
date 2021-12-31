using System.Collections.Generic;
using System.Linq;
using Fitness.Model;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Separator = LiveCharts.Wpf.Separator;

namespace Fitness.ViewModel
{
    public class AppViewModel: BaseVieModel
    {
        private IEnumerable<UserData> _user;
        public IEnumerable<UserData> User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }

        private SeriesCollection _seriesCollection;
        public SeriesCollection SeriesCollection
        {
            get => _seriesCollection;
            set
            {
                _seriesCollection = value;
                OnPropertyChanged(nameof(SeriesCollection));
            }
        }
        public static Separator Separator => new() {Step = 2};

        private UserData _selectedItem;
        public UserData SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                if (_selectedItem is not null)
                {
                    Chart(_selectedItem);
                }
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public AppViewModel()
        {
            User = new List<UserData>();
        }

        private RelayCommand _exportData;
        public RelayCommand ExportData => _exportData ??= new RelayCommand(Methods.ExportUserData);

        private RelayCommand _loadData;
        public RelayCommand LoadData => _loadData ??= new RelayCommand(obj =>
        {
            User = Methods.LoadData(User);
        });

        private RelayCommand _сlearData;
        public RelayCommand ClearData => _сlearData ??= new RelayCommand(obj =>
        {
            User = new List<UserData>();
        });

        public void Chart(UserData user)
        {
            var points = new ChartValues<ObservablePoint>();
            user.DaysData.OrderBy(x => x.Day).ToList().ForEach(x => points.Add(new ObservablePoint(x.Day, x.Steps)));
            SeriesCollection = new SeriesCollection()
            {
                new LineSeries
                {
                    Values = points,
                    DataLabels = true,
                    Title = "Шагов"
                }
            };
        }

    }
}
