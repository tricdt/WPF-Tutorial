﻿using syncfusion.demoscommon.wpf;

namespace syncfusion.treegriddemos.wpf
{
    /// <summary>
    /// Interaction logic for ExcelLikeFilteringDemo.xaml
    /// </summary>
    public partial class ExcelLikeFilteringDemo : DemoControl
    {
        public ExcelLikeFilteringDemo()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            this.Resources.Clear();

            // Release all managed resources
            if (this.treeGrid != null)
            {
                this.treeGrid.Dispose();
                this.treeGrid = null;
            }

            if (this.DataContext != null)
            {
                var dataContext = this.DataContext as EmployeeInfoViewModel;
                dataContext.Dispose();
                this.DataContext = null;
            }

            if (this.textBlock1 != null)
                this.textBlock1 = null;

            if (this.textBlock2 != null)
                this.textBlock2 = null;

            if (this.textBlock3 != null)
                this.textBlock3 = null;

            if (this.textBlock4 != null)
                this.textBlock4 = null;

            if (this.textBlock5 != null)
                this.textBlock5 = null;

            if (this.textBlock6 != null)
                this.textBlock6 = null;

            if (this.textBlock6 != null)
                this.textBlock6 = null;

            if (this.textBlock7 != null)
                this.textBlock7 = null;

            if (this.textBlock8 != null)
                this.textBlock8 = null;

            if (this.ckbAllowFilters != null)
                this.ckbAllowFilters = null;

            if (this.filterLevelComboBox != null)
                this.filterLevelComboBox = null;

            if (this.ckbAllowFilterFirstName != null)
                this.ckbAllowFilterFirstName = null;

            if (this.ckbImmediateUpdateColumnFilterFirstName != null)
                this.ckbImmediateUpdateColumnFilterFirstName = null;

            if (this.ckbAllowBlankFiltersLastName != null)
                this.ckbAllowBlankFiltersLastName = null;

            if (this.ckbAllowFilterEmployeeID != null)
                this.ckbAllowFilterEmployeeID = null;

            if (this.ckbImmediateUpdateColumnFilterEmployeeID != null)
                this.ckbImmediateUpdateColumnFilterEmployeeID = null;

            if (this.FilterModeforEmployeeID != null)
                this.FilterModeforEmployeeID = null;

            if (this.ckbAllowFilterDOJ != null)
                this.ckbAllowFilterDOJ = null;

            if (this.ckbImmediateUpdateColumnFilterDOJ != null)
                this.ckbImmediateUpdateColumnFilterDOJ = null;

            if (this.ckbAllowBlankFiltersDOJ != null)
                this.ckbAllowBlankFiltersDOJ = null;

            if (this.FilterModeforDOJ != null)
                this.FilterModeforDOJ = null;

            base.Dispose(disposing);
        }
    }
}
