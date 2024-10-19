
namespace syncfusion.gridcontroldemos.wpf
{
    public class AdvancedDataTemplateCellViewModel
    {
        private IAdvancedDataTemplateCell mainView;
        public AdvancedDataTemplateCellViewModel(IAdvancedDataTemplateCell mainView)
        {
            this.mainView = mainView;
            Initialize();
        }

        public IAdvancedDataTemplateCell MainView
        {
            get => mainView;
        }

        public List<Queries.CurrentHoldings> Holdings
        {
            get
            {
                var list = Queries.GetHoldingsList();
                return list;
            }
        }
        private void Initialize()
        {
            if (mainView != null)
            {
                mainView.Initialize();
            }
        }
    }
}
