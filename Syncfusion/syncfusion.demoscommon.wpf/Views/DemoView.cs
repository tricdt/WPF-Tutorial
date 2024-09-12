using Syncfusion.SfSkinManager;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace syncfusion.demoscommon.wpf
{
    public class DemoControl : UserControl, IDisposable
    {
        public DemoControl()
        {
            this.DefaultStyleKey = typeof(DemoControl);
        }
        public DemoControl(string themename) : this()
        {
            SfSkinManager.SetTheme(this, new Theme() { ThemeName = themename });
        }
        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {

        }
        /// <summary>
        ///  Gets or sets the options view.
        /// </summary>
        public object Options
        {
            get { return (object)GetValue(OptionsProperty); }
            set { SetValue(OptionsProperty, value); }
        }

        public static readonly DependencyProperty OptionsProperty =
            DependencyProperty.Register("Options", typeof(object), typeof(DemoControl), new PropertyMetadata(null));


        /// <summary>
        ///  Gets or sets position of <see cref="DemoControl.Options"/> view. You can position left, right, top or button and contorl the width or height using <see cref="DemoControl.OptionsSize"/>
        /// </summary>
        public Dock OptionsPosition
        {
            get { return (Dock)GetValue(OptionsPositionProperty); }
            set { SetValue(OptionsPositionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OptionsPosition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OptionsPositionProperty =
            DependencyProperty.Register("OptionsPosition", typeof(Dock), typeof(DemoControl), new PropertyMetadata(Dock.Right));

        /// <summary>
        /// Gets or sets the width of <see cref="DemoControl.Options"/> view when <see cref="DemoControl.OptionsPosition"/> is <see cref="Dock.Left"/> and <see cref="Dock.Right"/>.
        /// Gets or sets the height of <see cref="DemoControl.Options"/> view when <see cref="DemoControl.OptionsPosition"/> is <see cref="Dock.Top"/> and <see cref="Dock.Bottom"/>.
        /// </summary>
        public GridLength OptionsSize
        {
            get { return (GridLength)GetValue(OptionsSizeProperty); }
            set { SetValue(OptionsSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OptionsSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OptionsSizeProperty =
            DependencyProperty.Register("OptionsSize", typeof(GridLength), typeof(DemoControl), new PropertyMetadata(new GridLength(200)));

        /// <summary>
        /// Gets or sets the text displayed as options heading. you can hide the same by setting empty string.
        /// </summary>
        public string OptionsTitle
        {
            get { return (string)GetValue(OptionsTitleProperty); }
            set { SetValue(OptionsTitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OptionsTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OptionsTitleProperty =
            DependencyProperty.Register("OptionsTitle", typeof(string), typeof(DemoControl), new PropertyMetadata("OPTIONS"));

        [Browsable(false)]
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(DemoControl), new PropertyMetadata(string.Empty));

        [Browsable(false)]
        public string ControlName
        {
            get { return (string)GetValue(ControlNameProperty); }
            set { SetValue(ControlNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ControlNameProperty =
            DependencyProperty.Register("ControlName", typeof(string), typeof(DemoControl), new PropertyMetadata(string.Empty));

        [Browsable(false)]
        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Description.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(DemoControl), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Gets or sets the item source for a collection of helplinks in the documentation
        /// </summary>
        public object DocumentsItemSource
        {
            get { return (object)GetValue(DocumentsItemSourceProperty); }
            set { SetValue(DocumentsItemSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DocumentsItemSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DocumentsItemSourceProperty =
            DependencyProperty.Register("DocumentsItemSource", typeof(object), typeof(DemoControl), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the HyperLinkStyle for the collection of helplinks in the documentation
        /// </summary>
        public Style HyperLinkStyle
        {
            get { return (Style)GetValue(HyperLinkStyleProperty); }
            set { SetValue(HyperLinkStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HyperLinkStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HyperLinkStyleProperty =
            DependencyProperty.Register("HyperLinkStyle", typeof(Style), typeof(DemoControl), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the HorizontalScrollBarVisibility of the option panel
        /// </summary>
        public ScrollBarVisibility OptionsHorizontalScrollBarVisibility
        {
            get { return (ScrollBarVisibility)GetValue(OptionsHorizontalScrollBarVisibilityProperty); }
            set { SetValue(OptionsHorizontalScrollBarVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OptionHorizantalScrollBarVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OptionsHorizontalScrollBarVisibilityProperty =
            DependencyProperty.Register("OptionsHorizontalScrollBarVisibility", typeof(ScrollBarVisibility), typeof(DemoControl), new PropertyMetadata(ScrollBarVisibility.Disabled));


        /// <summary>
        /// Gets or Sets the VerticalScrollBarVisibility of the option panel
        /// </summary>
        public ScrollBarVisibility OptionsVerticalScrollBarVisibility
        {
            get { return (ScrollBarVisibility)GetValue(OptionsVerticalScrollBarVisibilityProperty); }
            set { SetValue(OptionsVerticalScrollBarVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OptionVerticalScrollBarVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OptionsVerticalScrollBarVisibilityProperty =
            DependencyProperty.Register("OptionsVerticalScrollBarVisibility", typeof(ScrollBarVisibility), typeof(DemoControl), new PropertyMetadata(ScrollBarVisibility.Disabled));

        /// <summary>
        /// Gets or sets the SubCategoryDemos property that provides access to a list of DemoInfo objects.
        /// </summary>
        public List<DemoInfo> SubCategoryDemos
        {
            get { return (List<DemoInfo>)GetValue(SubCategoryDemosProperty); }
            set { SetValue(SubCategoryDemosProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SubCategoryDemos.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SubCategoryDemosProperty =
            DependencyProperty.Register("SubCategoryDemos", typeof(List<DemoInfo>), typeof(DemoControl), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the value of SubCategorySelectedItem property in the Demo window.
        /// </summary>
        public DemoInfo SubCategorySelectedItem
        {
            get { return (DemoInfo)GetValue(SubCategorySelectedItemProperty); }
            set { SetValue(SubCategorySelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SubCategorySelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SubCategorySelectedItemProperty =
            DependencyProperty.Register("SubCategorySelectedItem", typeof(DemoInfo), typeof(DemoControl), new PropertyMetadata(null));

    }
}
