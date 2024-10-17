using syncfusion.demoscommon.wpf;
using Syncfusion.Windows.ComponentModel;
using Syncfusion.Windows.Controls.Grid;
using System.Windows.Controls;

namespace syncfusion.gridcontroldemos.wpf
{
    /// <summary>
    /// Interaction logic for UndoRedo.xaml
    /// </summary>
    public partial class UndoRedo : DemoControl
    {
        private int oldRedoCount;
        private int oldUndoCount;
        public UndoRedo()
        {
            InitializeComponent();
            this.grid.Model.RowCount = 30;
            this.grid.Model.ColumnCount = 15;
            PopulateGrid();
            oldRedoCount = 0;
            oldUndoCount = 0;
            this.grid.LayoutUpdated += new EventHandler(Grid_LayoutUpdated);
            this.grid.Model.CommandStack.Enabled = true;
        }

        public UndoRedo(string themename) : base(themename)
        {
            InitializeComponent();
            this.grid.Model.RowCount = 30;
            this.grid.Model.ColumnCount = 15;
            PopulateGrid();
            oldRedoCount = 0;
            oldUndoCount = 0;
            this.grid.LayoutUpdated += new EventHandler(Grid_LayoutUpdated);
            this.grid.Model.CommandStack.Enabled = true;
        }

        private void Grid_LayoutUpdated(object? sender, EventArgs e)
        {
            ShowStacks();
        }

        private void ShowStacks()
        {
            ShowRedoStack();
            ShowUndoStack();
        }

        private void ShowUndoStack()
        {
            int numUndos = this.grid.Model.CommandStack.UndoStack.Count;
            if (numUndos != this.oldUndoCount)
            {
                this.listBox1.Items.Clear();
                this.listBox1.Items.Add(string.Format("{0} Undo items", numUndos));

                if (numUndos > 0 || this.grid.Model.CommandStack.IsRecording)
                    DisplayCommandsInStack(this.grid.Model.CommandStack.UndoStack.ToArray(), "", listBox1, true);
                this.oldUndoCount = numUndos;
            }
        }

        private void ShowRedoStack()
        {
            int numRedos = this.grid.Model.CommandStack.RedoStack.Count;
            if (numRedos != this.oldRedoCount)
            {
                this.listBox2.Items.Clear();
                this.listBox2.Items.Add(string.Format("{0} Redo items", numRedos));
                if (numRedos > 0 || this.grid.Model.CommandStack.IsRecording)
                    DisplayCommandsInStack(this.grid.Model.CommandStack.RedoStack.ToArray(), "", listBox2, false);
                this.oldRedoCount = numRedos;
            }
        }
        private void DisplayCommandsInStack(object[] items, string indent, ListBox _listbox, bool includeCurrentCommand)
        {
            string s;
            SyncfusionCommand c;
            GridTransactionCommand tc;
            int cutOff;

            //handle the case where we are recording a transaction
            if (includeCurrentCommand && this.grid.Model.CommandStack.InTransaction)
            {
                try
                {
                    tc = this.grid.Model.CommandStack.CurrentTransactionCommand;
                    DisplayCommandsInStack(tc.Stack.ToArray(), "    > ", _listbox, false);
                }
                catch { }
            }

            foreach (object o in items)
            {
                try
                {
                    c = (SyncfusionCommand)o;
                    if (c != null && c.Description != null)
                        s = c.Description;
                    else
                        s = o.ToString();
                    cutOff = 1 + Math.Max(s.LastIndexOf("+"), s.LastIndexOf("."));
                    _listbox.Items.Add(indent + s.Substring(cutOff));
                }
                catch { }

                //check if is a transaction command
                try
                {
                    tc = o as GridTransactionCommand;
                    if (tc != null)
                    {
                        DisplayCommandsInStack(tc.Stack.ToArray(), "    > ", _listbox, false);
                    }
                }
                catch { }

            }
        }
        private void PopulateGrid()
        {
            Random r = new Random();
            for (int i = 1; i <= this.grid.Model.RowCount; ++i)
                for (int j = 1; j <= this.grid.Model.ColumnCount; ++j)
                    this.grid.Model[i, j].CellValue = r.Next(1000);
        }

        private void Undo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!this.grid.Model.CommandStack.InTransaction)
            {
                this.grid.Model.CommandStack.Undo();
                ShowStacks();
            }
        }

        private void Redo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!this.grid.Model.CommandStack.InTransaction)
            {
                this.grid.Model.CommandStack.Redo();
                ShowStacks();
            }
        }

        private void BeginTrans_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.grid.Model.CommandStack.BeginTrans("Transaction beginning-");
            ShowStacks();
        }

        private void Commit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.grid.Model.CommandStack.InTransaction)
            {
                this.grid.Model.CommandStack.CommitTrans();
                ShowStacks();
            }
        }

        private void RollBack_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.grid.Model.CommandStack.InTransaction)
            {
                this.grid.Model.CommandStack.Rollback();
                ShowStacks();
            }
        }

        private void ClearUndoRedo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!this.grid.Model.CommandStack.InTransaction)
            {
                this.grid.Model.CommandStack.UndoStack.Clear();
                this.grid.Model.CommandStack.RedoStack.Clear();
                ShowStacks();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (this.grid != null)
            {
                this.grid.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
