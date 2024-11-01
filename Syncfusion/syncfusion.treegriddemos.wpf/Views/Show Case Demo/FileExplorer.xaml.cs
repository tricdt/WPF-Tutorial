using Syncfusion.SfSkinManager;
using Syncfusion.Windows.Shared;
namespace syncfusion.treegriddemos.wpf
{
    /// <summary>
    /// Interaction logic for FileExplorer.xaml
    /// </summary>
    public partial class FileExplorer : ChromelessWindow
    {
        public FileExplorer()
        {
            InitializeComponent();
        }

        public FileExplorer(string themename)
        {
            SfSkinManager.SetVisualStyle(this, (VisualStyles)Enum.Parse(typeof(VisualStyles), themename));
            InitializeComponent();
            this.Closed += FileExplorer_Closed;
        }
        private void FileExplorer_Closed(object sender, EventArgs e)
        {
            if (this.treeGrid != null)
            {
                this.treeGrid.Dispose();
                this.treeGrid = null;
            }
            this.Closed -= FileExplorer_Closed;
        }
    }
}
