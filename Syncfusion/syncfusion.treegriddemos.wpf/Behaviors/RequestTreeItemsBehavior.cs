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
                args.ChildItems = (IEnumerable)fileExplorerViewModel.DriveDetails;
            }
            else
            {

                FileInfoModel item = args.ParentItem as FileInfoModel;
                args.ChildItems = fileExplorerViewModel.GetChildFolderContent(item);
            }
        }
    }
}
