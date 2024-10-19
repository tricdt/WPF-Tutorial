using Syncfusion.UI.Xaml.Grid;

namespace syncfusion.datagriddemos.wpf
{
    public class CellAnimationCustomRowGenerator : RowGenerator
    {
        public CellAnimationCustomRowGenerator(SfDataGrid owner) : base(owner)
        {
        }
        protected override GridCell GetGridCell<T>()
        {
            if (typeof(T) == typeof(GridCell))
            {
                return new CustomGridCell();
            }
            return base.GetGridCell<T>();
        }
    }
}
