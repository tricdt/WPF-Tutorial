namespace LedSignWinform
{
    public partial class GridControl1 : Form
    {
        public GridControl1()
        {
            InitializeComponent();
            gridControl1.RowCount = 40;
            gridControl1.ColCount = 35;
            for (int row = 1; row <= gridControl1.RowCount; row++)
            {
                this.gridControl1.RowHeights[row] = 25;
                for (int col = 1; col <= gridControl1.ColCount; col++)
                {
                    this.gridControl1.ColWidths[col] = 65;
                    gridControl1.Model[row, col].CellValue = string.Format("{0}/{1}", row, col);
                }
            }
            //Copy the selection to the Clipboard.
            gridControl1.CutPaste.Copy();
            // Cuts and copies the contents of selected cells to clipboard. 
            gridControl1.CutPaste.Cut();
            //Paste the contents of the clipboard to the specific selected range.
            gridControl1.CutPaste.Paste();
        }

        private void GridControl1_Load(object sender, EventArgs e)
        {

        }
    }
}
