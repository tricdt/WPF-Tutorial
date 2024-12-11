using Syncfusion.SfSkinManager;
using Syncfusion.Windows.Controls.Grid;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace syncfusion.ledsign.wpf
{
    public class LocationViewModel : NotificationObject
    {
        public MainWindow MainWindow { get; set; }
        ViewModelLocator _viewModelLocator;
        public LocationViewModel(ViewModelLocator viewModel)
        {
            _viewModelLocator = viewModel;
            ThemeName = lightTheme;
            GridLed = new ObservableCollection<GridControl>();
            GridLed.Add(new SampleGrid());
            GridLed.Add(new SampleGrid());

            GroupLed = new ObservableCollection<GroupLed>();
            GroupLed.Add(new GroupLed() { UpDown = new UpDown(), GridLed = new GridControl()});
        }
        private ICommand themeChanged;
        public ICommand ThemeChanged
        {
            get
            {
                if (themeChanged == null)
                {
                    themeChanged = new RelayCommand(ThemeChangedExecute, CanThemeChanged);
                }

                return themeChanged;
            }
        }
        private string themeName;
        public string ThemeName
        {
            get
            {
                return themeName;
            }

            set
            {
                if (themeName != value)
                {
                    themeName = value;
                    UpdateTheme();
                }
            }
        }
        string lightTheme = "Windows11Light";
        string darkTheme = "Windows11Dark";
        private bool CanThemeChanged(object parameter)
        {
            return true;
        }

        private void ThemeChangedExecute(object parameter)
        {
            ThemeName = ThemeName == lightTheme ? darkTheme : lightTheme;
            UpdateTheme();
            foreach (SampleGrid item in GridLed)
            {
                item.InvalidateCells();
            }
        }
        void UpdateTheme()
        {
          
            if (ThemeName == darkTheme)
            {
                SfSkinManager.SetTheme(WindowHelper.MainWindow, new Theme() { ThemeName = darkTheme });
                PathData = "M8.33203 1V2.33333M8.33203 14.334V15.6673M3.14453 3.14648L4.0912 4.09315M12.5703 12.5742L13.517 13.5209M1 8.33398H2.33333M14.332 8.33398H15.6654M3.14453 13.5209L4.0912 12.5742M12.5703 4.09315L13.517 3.14648M11.6667 8.33333C11.6667 10.1743 10.1743 11.6667 8.33333 11.6667C6.49238 11.6667 5 10.1743 5 8.33333C5 6.49238 6.49238 5 8.33333 5C10.1743 5 11.6667 6.49238 11.6667 8.33333Z";
            }
            else
            {
                SfSkinManager.SetTheme(WindowHelper.MainWindow, new Theme() { ThemeName = lightTheme });
                PathData = "M1 7.88484C1 11.8144 4.18557 15 8.11516 15C11.422 15 14.2019 12.7442 15 9.68742C14.4243 9.83773 13.8202 9.91774 13.1974 9.91774C9.26783 9.91774 6.08226 6.73217 6.08226 2.80258C6.08226 2.17979 6.16227 1.57569 6.31258 1C3.25584 1.7981 1 4.57803 1 7.88484Z";
            }

        }

        private string pathData;

        public string PathData
        {
            get { return pathData; }
            set { pathData = value; RaisePropertyChanged(nameof(PathData)); }
        }

        private ObservableCollection<GridControl> _gridLed;

        public ObservableCollection<GridControl> GridLed
        {
            get { return _gridLed; }
            set { _gridLed = value; }
        }
        private ObservableCollection<GroupLed> _groupLed;
        public ObservableCollection<GroupLed> GroupLed
        {
            get { return _groupLed; }
            set { _groupLed = value; }
        }
    }
    public class GroupLed
    {
        public GridControl GridLed { get; set; }
        public UpDown UpDown { get; set; }
    }
}
