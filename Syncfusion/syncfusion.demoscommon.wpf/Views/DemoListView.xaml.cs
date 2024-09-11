using Syncfusion.SfSkinManager;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace syncfusion.demoscommon.wpf
{
    /// <summary>
    /// Interaction logic for DemoListView.xaml
    /// </summary>
    public partial class DemoListView : UserControl
    {
        public DemoListView()
        {
            InitializeComponent();
            DemosNavigationService.DemoNavigationService = this.DEMOSFRAME.NavigationService;
            this.DataContextChanged += DemoListView_DataContextChanged;
        }
        private DemoBrowserViewModel viewModel;
        private void DemoListView_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            viewModel = this.DataContext as DemoBrowserViewModel;
            if (viewModel != null)
            {
                viewModel.ThemeChanged = ThemeChanged;
            }
        }

        private void ThemeChanged()
        {
            if (viewModel.SelectedThemeName.Contains("Fluent"))
            {
                SfSkinManager.SetTheme(ThemePanel, new FluentTheme() { ThemeName = viewModel.SelectedThemeName, HoverEffectMode = HoverEffect.Background });
            }
            else
            {
                SfSkinManager.SetTheme(ThemePanel, new Theme() { ThemeName = viewModel.SelectedThemeName });
            }
        }

        private void OnAllProductsPreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RepeatButton button = (RepeatButton)sender;
            ICommand command = button.Command = viewModel.NavigateAllProductsCommand;
            if (command != null && command.CanExecute(null))
            {
                command.Execute(null);
            }
        }
    }
}
