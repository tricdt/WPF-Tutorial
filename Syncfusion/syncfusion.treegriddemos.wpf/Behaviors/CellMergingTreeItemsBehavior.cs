using Microsoft.Xaml.Behaviors;
using Syncfusion.UI.Xaml.TreeGrid;

namespace syncfusion.treegriddemos.wpf
{
    public class CellMergingTreeItemsBehavior : Behavior<SfTreeGrid>
    {
        protected override void OnAttached()
        {
            this.AssociatedObject.RequestTreeItems += AssociatedObject_RequestTreeItems;
            base.OnAttached();
        }

        private void AssociatedObject_RequestTreeItems(object? sender, TreeGridRequestTreeItemsEventArgs args)
        {
            if (args.ParentItem == null)
            {
                //get the root list - get all employees who have no boss 
                args.ChildItems = EmployeeInfoViewModel.GetEmployees().Where(x => x.ReportsTo == -1); //get all employees whose boss's id is -1 (no boss)
            }
            else //if ParentItem not null, then set args.ChildList to the child items for the given ParentItem.
            {
                //get the children of the parent object
                EmployeeInfo emp = args.ParentItem as EmployeeInfo;
                if (emp != null)
                {
                    //get all employees that report to the parent employee
                    args.ChildItems = EmployeeInfoViewModel.GetEmployees().Where(x => x.ReportsTo == emp.ID);
                }
            }
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.RequestTreeItems -= AssociatedObject_RequestTreeItems;
            base.OnDetaching();
        }
    }
}
