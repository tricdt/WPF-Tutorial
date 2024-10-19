using syncfusion.demoscommon.wpf;
using Syncfusion.Windows.Controls.Cells;
using Syncfusion.Windows.Controls.Grid;
using System.Windows;
using System.Windows.Media;

namespace syncfusion.gridcontroldemos.wpf
{
    /// <summary>
    /// Interaction logic for BasicCellType.xaml
    /// </summary>
    public partial class BasicCellType : DemoControl
    {
        int rowIndex = 5;
        int colIndex = 2;
        List<MovieInfo> MovieDetails;
        public BasicCellType()
        {
            InitializeComponent();
            GridSettings();
        }



        public BasicCellType(string themename) : base(themename)
        {
            InitializeComponent();
            GridSettings();
        }

        private void GridSettings()
        {
            InitializeGrid();
            MovieDetails = new BasicCellTypeViewModel().MovieDetails;
            grid_ImageCellType();
            grid_StaticCellType();
            grid_HeaderCellType();
            grid_TextBoxCellType();
            grid_CheckBoxCellType();
            grid_ButtonCellType();
        }


        private void InitializeGrid()
        {
            this.grid.AllowDragColumns = true;
            this.grid.Model.RowCount = 30;
            this.grid.Model.ColumnCount = 20;
            this.grid.Model.FrozenColumns = 2;
            this.grid.FooterColumns = 1;
            this.grid.Model.FooterStyle.Background = new SolidColorBrush(Colors.AliceBlue);
            this.grid.Model.CoveredCells.Add(new CoveredCellInfo(1, 2, 2, 8));

            grid.Model[1, 2].CellValue = "Cell Type Demo";
            grid.Model[1, 2].Foreground = Brushes.Black;
            grid.Model[1, 2].Background = Brushes.LightBlue;
            grid.Model[1, 2].Font.FontSize = 18;
            grid.Model[1, 2].HorizontalAlignment = HorizontalAlignment.Center;
            grid.Model[1, 2].Font.FontWeight = FontWeights.Bold;
            grid.Model.ColumnWidths[this.grid.Model.ColumnCount - 1] = 150;
            grid.Model.ColumnWidths[1] = 150;
        }

        private void grid_ImageCellType()
        {
            rowIndex++;
            colIndex = 2;
            grid.Model[rowIndex, 1].CellValue = "Movie Poster";
            grid.Model[rowIndex, this.grid.Model.ColumnCount - 1].CellValue = "Image";
            grid.Model.RowHeights[rowIndex] = 80;
            foreach (MovieInfo movie in MovieDetails)
            {
                grid.Model[rowIndex, colIndex].CellType = "ImageCell";
                grid.Model[rowIndex, colIndex].CellValue = movie.Poster;
                grid.Model.ColumnWidths[colIndex] = 120;
                colIndex += 2;
            }
        }

        private void grid_StaticCellType()
        {
            rowIndex++;
            colIndex = 2;
            grid.Model[rowIndex, 1].CellValue = "Movie Name";
            grid.Model[rowIndex, this.grid.Model.ColumnCount - 1].CellValue = "Static";
            foreach (MovieInfo movie in MovieDetails)
            {
                grid.Model[rowIndex, colIndex].CellType = "Static";
                grid.Model[rowIndex, colIndex].CellValue = movie.MovieName;
                colIndex += 2;
            }
        }

        private void grid_HeaderCellType()
        {
            rowIndex++;
            colIndex = 2;
            grid.Model[rowIndex, 1].CellValue = "Theater Name";
            grid.Model[rowIndex, this.grid.Model.ColumnCount - 1].CellValue = "Header";
            foreach (MovieInfo movie in MovieDetails)
            {
                grid.Model[rowIndex, colIndex].CellType = "Header";
                grid.Model[rowIndex, colIndex].CellValue = movie.Theatre;
                colIndex += 2;
            }
        }

        public void grid_TextBoxCellType()
        {
            rowIndex++;
            colIndex = 2;
            grid.Model[rowIndex, 1].CellValue = "City";
            grid.Model[rowIndex, this.grid.Model.ColumnCount - 1].CellValue = "TextCell";
            foreach (MovieInfo movie in MovieDetails)
            {
                grid.Model[rowIndex, colIndex].CellValue = movie.City;
                colIndex += 2;
            }
        }

        public void grid_CheckBoxCellType()
        {
            rowIndex++;
            colIndex = 2;
            grid.Model[rowIndex, 1].CellValue = "Is Ticket Available";
            grid.Model[rowIndex, this.grid.Model.ColumnCount - 1].CellValue = "CheckBox";
            foreach (MovieInfo movie in MovieDetails)
            {
                grid.Model[rowIndex, colIndex].CellType = "CheckBox";
                grid.Model[rowIndex, colIndex].CellValue = movie.IsTicketAvailable;
                grid.Model[rowIndex, colIndex].HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                colIndex += 2;
            }
        }
        public void grid_ButtonCellType()
        {
            rowIndex++;
            colIndex = 2;
            grid.Model[rowIndex, 1].CellValue = "Book Ticket";
            grid.Model[rowIndex, this.grid.Model.ColumnCount - 1].CellValue = "Button";
            foreach (MovieInfo movie in MovieDetails)
            {
                grid.Model[rowIndex, colIndex].Description = "Book Ticket";
                grid.Model[rowIndex, colIndex].CellType = "Button";
                colIndex += 2;
            }
            grid.CellButtonClick += new GridCellButtonClickEventHandler(grid_CellButtonClick);
        }

        public void grid_CellButtonClick(object sender, GridCellButtonClickEventArgs e)
        {
            MessageBox.Show("Ticket Booked");
        }

        protected override void Dispose(bool disposing)
        {
            if (this.grid != null)
            {
                this.grid.Dispose();
                this.grid = null;
            }
            base.Dispose(disposing);
        }

    }
}
