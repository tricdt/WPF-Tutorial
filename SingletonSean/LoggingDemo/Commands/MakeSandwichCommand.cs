using System.Windows;

namespace LoggingDemo.Commands
{
    public class MakeSandwichCommand : CommandBase
    {

        public override void Execute(object parameter)
        {

            MessageBox.Show("Successfully made sandwich.", "Done", MessageBoxButton.OK);

        }
    }
}
