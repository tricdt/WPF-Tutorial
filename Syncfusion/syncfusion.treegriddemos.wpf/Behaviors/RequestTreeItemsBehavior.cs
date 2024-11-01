using Microsoft.Xaml.Behaviors;
using Syncfusion.UI.Xaml.TreeGrid;
using System.Collections;

namespace syncfusion.treegriddemos.wpf
{
    public class RequestTreeItemsBehavior : Behavior<SfTreeGrid>
    {
        private FileExplorerViewModel fileExplorerViewModel;
        private EmployeeInfoViewModel viewModel;
        protected override void OnAttached()
        {
            base.OnAttached();
            if (this.AssociatedObject.DataContext is EmployeeInfoViewModel)
                viewModel = this.AssociatedObject.DataContext as EmployeeInfoViewModel;
            else
                fileExplorerViewModel = this.AssociatedObject.DataContext as FileExplorerViewModel;
            this.AssociatedObject.RequestTreeItems += AssociatedObject_RequestTreeItems;
        }

        private void AssociatedObject_RequestTreeItems(object? sender, TreeGridRequestTreeItemsEventArgs args)
        {
            if (args.ParentItem == null)
            {
                if (viewModel != null)
                    //get the root list - get all employees who have no boss 
                    args.ChildItems = viewModel.EmployeeList.Where(x => x.ReportsTo == -1); //get all employees whose boss's id is -1 (no boss)
                else
                    args.ChildItems = (IEnumerable)fileExplorerViewModel.DriveDetails;
            }
            else
            {
                if (viewModel != null)
                {
                    EmployeeInfo emp = args.ParentItem as EmployeeInfo;
                    if (emp != null)
                    {
                        //get all employees that report to the parent employee
                        args.ChildItems = viewModel.GetReportees(emp.ID);
                    }
                }
                else
                {
                    FileInfoModel item = args.ParentItem as FileInfoModel;
                    args.ChildItems = fileExplorerViewModel.GetChildFolderContent(item);
                }
            }
        }
    }
}
