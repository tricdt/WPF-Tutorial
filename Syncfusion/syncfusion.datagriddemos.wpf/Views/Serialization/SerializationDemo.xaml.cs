using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for SerializationDemo.xaml
    /// </summary>
    public partial class SerializationDemo : DemoControl
    {
        public static SerializationDemo demoControl;
        public SerializationDemo()
        {
            InitializeComponent();
            demoControl = this;
        }
        public SerializationDemo(string themename) : base(themename)
        {
            InitializeComponent();
            demoControl = this;
        }
    }
}
