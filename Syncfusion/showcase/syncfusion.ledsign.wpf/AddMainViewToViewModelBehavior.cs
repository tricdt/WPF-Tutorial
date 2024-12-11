using Microsoft.Xaml.Behaviors;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace syncfusion.ledsign.wpf
{
    public class AddMainViewToViewModelBehavior : Behavior<MainWindow>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            if(this.AssociatedObject.DataContext is ViewModelLocator)
                (this.AssociatedObject.DataContext as ViewModelLocator).LocationViewModel.MainWindow = this.AssociatedObject as MainWindow;
        }
    }
}
