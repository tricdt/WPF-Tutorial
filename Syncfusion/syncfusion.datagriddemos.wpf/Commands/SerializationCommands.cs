using Syncfusion.UI.Xaml.Grid;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace syncfusion.datagriddemos.wpf
{
    public static class SerializationCommands
    {
        static SerializationCommands()
        {
            CommandManager.RegisterClassCommandBinding(typeof(SfDataGrid), new CommandBinding(Serialize, OnExecuteSerialize, OnCanExecuteSerialize));
            CommandManager.RegisterClassCommandBinding(typeof(SfDataGrid), new CommandBinding(Deserialize, OnExecuteDeserialize, OnCanExecuteDeserialize));
            CommandManager.RegisterClassCommandBinding(typeof(SfDataGrid), new CommandBinding(Reset, OnExecuteReset, OnCanExecuteReset));
            CommandManager.RegisterClassCommandBinding(typeof(SfDataGrid), new CommandBinding(Add, OnExecuteAdd, OnCanExecuteAdd));
            CommandManager.RegisterClassCommandBinding(typeof(SfDataGrid), new CommandBinding(Remove, OnExecuteRemove, OnCanExecuteRemove));
        }
        #region Serialize Command
        public static RoutedCommand Serialize = new RoutedCommand("Serialize", typeof(SfDataGrid));

        private static void OnCanExecuteSerialize(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private static void OnExecuteSerialize(object sender, ExecutedRoutedEventArgs args)
        {
            var dataGrid = args.Source as SfDataGrid;
            var options = args.Parameter as SerializationOptions;

            if (dataGrid == null || options == null)
                return;
            try
            {
                using (var file = File.Create("DataGrid.xml"))
                {
                    dataGrid.Serialize(file, options);
                }
            }
            catch (Exception)
            {

            }
        }
        #endregion
        #region Deserialize Command

        public static RoutedCommand Deserialize = new RoutedCommand("Deserialize", typeof(SfDataGrid));
        /// <summary>
        ///Occurs when the command associated with this CommandBinding executes.
        /// </summary> 
        private static void OnExecuteDeserialize(object sender, ExecutedRoutedEventArgs args)
        {
            var dataGrid = args.Source as SfDataGrid;
            var options = args.Parameter as DeserializationOptions;
            if (dataGrid == null || options == null)
                return;

            try
            {
                using (var file = File.Open("DataGrid.xml", FileMode.Open))
                {
                    dataGrid.Deserialize(file, options);
                }
            }
            catch (Exception)
            {
            }
        }

        private static void OnCanExecuteDeserialize(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = true;
        }

        #endregion
        #region Reset Command

        public static RoutedCommand Reset = new RoutedCommand("Reset", typeof(SfDataGrid));

        /// <summary>
        ///Occurs when the command associated with this CommandBinding executes.
        /// </summary>  
        private static void OnExecuteReset(object sender, ExecutedRoutedEventArgs e)
        {
            var dataGrid = e.Source as SfDataGrid;
            SerializationDemo mainwnd = (SerializationDemo)Activator.CreateInstance(typeof(SerializationDemo));

            List<String> selectedItemsCol = new List<string>();
            if (dataGrid == null) return;
            try
            {
                using (var file = File.Open("Reset.xml", FileMode.Open))
                {
                    dataGrid.Deserialize(file);
                }
            }
            catch (Exception)
            {

            }
        }


        private static void OnCanExecuteReset(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion
        #region Add Command
        /// <summary>
        ///Occurs when the command associated with this CommandBinding executes.
        /// </summary> 
        private static void OnExecuteAdd(object sender, ExecutedRoutedEventArgs e)
        {
            var manipulatorwnd = new ManipulatorView();
            manipulatorwnd.removenormalCol.Visibility = Visibility.Collapsed;
            manipulatorwnd.removecol_Tilte.Visibility = Visibility.Collapsed;
            manipulatorwnd.ShowDialog();
        }

        public static RoutedCommand Add = new RoutedCommand("Add", typeof(SfDataGrid));

        private static void OnCanExecuteAdd(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion
        #region Remove Command
        /// <summary>
        ///Occurs when the command associated with this CommandBinding executes.
        /// </summary> 
        private static void OnExecuteRemove(object sender, ExecutedRoutedEventArgs e)
        {
            var manipulatorwnd = new ManipulatorView();
            manipulatorwnd.addnormalCol.Visibility = Visibility.Collapsed;
            manipulatorwnd.addcolarea.Visibility = Visibility.Collapsed;
            manipulatorwnd.Height = 165;
            manipulatorwnd.ShowDialog();
        }

        public static RoutedCommand Remove = new RoutedCommand("Remove", typeof(SfDataGrid));

        private static void OnCanExecuteRemove(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion

    }
}
