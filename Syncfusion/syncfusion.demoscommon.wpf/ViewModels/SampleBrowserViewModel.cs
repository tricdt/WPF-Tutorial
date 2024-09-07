using Syncfusion.UI.Xaml.NavigationDrawer;
using Syncfusion.Windows.Shared;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace syncfusion.demoscommon.wpf
{
    public abstract class DemoBrowserViewModel : NotificationObject
    {
        /// <summary>
        /// Property to store busy status of sample browser while launch the show case demo.
        /// </summary>
        private bool isShowCaseDemoBusy = false;
        public bool IsShowCaseDemoBusy
        {
            get { return isShowCaseDemoBusy; }
            set
            {
                isShowCaseDemoBusy = value;
                RaisePropertyChanged("IsShowCaseDemoBusy");
            }
        }

        /// <summary>
        /// Property to store visibility state of blur layer in sample browser.
        /// </summary>
        private Visibility blurVisibility = Visibility.Collapsed;
        public Visibility BlurVisibility
        {
            get { return blurVisibility; }
            set
            {
                blurVisibility = value;
                RaisePropertyChanged(nameof(BlurVisibility));
            }
        }

        /// <summary>
        /// Gets or set the selecteditem of the NavigationDrawer
        /// </summary>
        private object selectedItem;
        public object SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnSelectionChanged();
                this.RaisePropertyChanged(nameof(SelectedItem));
            }
        }
        private void OnSelectionChanged()
        {
            string pagename = string.Empty;
            if (this.SelectedItem == null)
                return;
            if (this.SelectedItem is NavigationViewModel navigationViewModel)
            {
                pagename = navigationViewModel.NavigationItem.ToString();
            }
            else if (this.SelectedItem is NavigationItem navigationItem)
            {
                pagename = navigationItem.Header.ToString();
            }
            if (pagename == "Home")
            {
                NavigationContent = new HomePage();
            }
            else if (pagename == "What's New")
            {
                NavigationContent = new WhatsNew();
            }
            else if (pagename == "Showcase")
            {
                NavigationContent = new ShowcaseApplication();
            }
            else if (pagename == "All Controls")
            {
                NavigationContent = new AllComponentsPage();
            }
        }
        /// <summary>
        /// Gets or set the pageview of the items of NavigationDrawer
        /// </summary>
        private object navigationContent;
        public object NavigationContent
        {
            get { return navigationContent; }
            set
            {
                navigationContent = value;
                this.RaisePropertyChanged(nameof(NavigationContent));
            }
        }

        /// <summary>
        /// Gets or set the HeaderItems Collection
        /// </summary>
        private ObservableCollection<NavigationViewModel> headerItems;
        public ObservableCollection<NavigationViewModel> HeaderItems
        {
            get
            {
                return headerItems;
            }
            set
            {
                headerItems = value;
                this.RaisePropertyChanged(nameof(HeaderItems));
            }
        }

        /// <summary>
        /// Gets or sets the collection of WhatsNewDemos
        /// </summary>
        public List<DemoInfo> WhatsNewDemos { get; set; }

        /// <summary>
        /// Maintains the command for the ShowAll ,Explore All Controls ,ListView and GalleryView Buttons
        /// </summary>
        private ICommand clickCommand;
        public ICommand ClickCommand
        {
            get
            {
                return clickCommand;
            }
            set
            {
                clickCommand = value;
                this.RaisePropertyChanged(nameof(ClickCommand));
            }
        }

        private void NavigationItems()
        {
            this.HeaderItems = new ObservableCollection<NavigationViewModel>();
            NavigationViewModel home = new NavigationViewModel()
            {
                NavigationItem = "Home",
                NavigationIcon = new System.Windows.Shapes.Path()
                {
                    Data = Geometry.Parse("M0.5 6.93825C0.5 6.65915 0.616638 6.39275 0.82172 6.20345L6.32172 1.12652C6.70478 0.772929 7.29522 0.77293 7.67828 1.12652L13.1783 6.20345C13.3834 6.39276 13.5 6.65915 13.5 6.93825V13.5004C13.5 14.0527 13.0523 14.5004 12.5 14.5004H9C8.72386 14.5004 8.5 14.2766 8.5 14.0004V10.0004C8.5 9.72428 8.27614 9.50042 8 9.50042H6C5.72386 9.50042 5.5 9.72428 5.5 10.0004V14.0004C5.5 14.2766 5.27614 14.5004 5 14.5004H1.5C0.947715 14.5004 0.5 14.0527 0.5 13.5004V6.93825Z"),
                    Stroke = new SolidColorBrush(Colors.Black),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                }
            };
            NavigationViewModel whatsNew = new NavigationViewModel()
            {
                NavigationItem = "What's New",
                NavigationIcon = new System.Windows.Shapes.Path()
                {
                    Data = Geometry.Parse("M5.28176 12.5C5.47648 13.1683 5.74709 13.8512 5.98382 14.3963C6.28203 15.083 6.97041 15.5 7.71907 15.5H8.80168C9.53942 15.5 10.2212 15.0951 10.5083 14.4155C10.7406 13.8657 10.9959 13.1732 11.1417 12.5H5.28176ZM5.28176 12.5C4.97352 11.4421 5 11 4.26903 10.0411C3.09433 8.5 2.5 7 2.5 6C2.5 2.96243 4.96243 0.5 8 0.5C11.0376 0.5 13.5 2.96243 13.5 6C13.5 7 13.1747 8.5 12 10.0411C11.269 11 11.4582 11.4421 11.15 12.5"),
                    Stroke = new SolidColorBrush(Colors.Black),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                }
            };
            NavigationViewModel showcaseApplication = new NavigationViewModel()
            {
                NavigationItem = "Showcase",
                NavigationIcon = new System.Windows.Shapes.Path()
                {
                    Data = Geometry.Parse("M6.50386 0.207929C6.8113 0.0322524 7.18871 0.0322534 7.49614 0.207929L13.4884 3.63205C14.1602 4.01595 14.1602 4.98465 13.4884 5.36854L7.49614 8.79266C7.1887 8.96834 6.81129 8.96834 6.50386 8.79266L0.511643 5.36854C-0.160172 4.98464 -0.160171 4.01595 0.511643 3.63205L6.50386 0.207929ZM7 1.07617L1.00778 4.5003L7 7.92442L12.9922 4.5003L7 1.07617ZM0.417638 6.80694C0.274499 6.83095 0.142952 6.91679 0.065554 7.05276C-0.0710532 7.29275 0.0127516 7.59803 0.252737 7.73464L6.50539 11.2938C6.81208 11.4684 7.18809 11.4684 7.49478 11.2938L13.7474 7.73464C13.9874 7.59803 14.0712 7.29275 13.9346 7.05276C13.8572 6.91679 13.7257 6.83095 13.5825 6.80694C13.5528 6.82847 13.5214 6.84872 13.4884 6.86757L7.49622 10.2917C7.34321 10.3791 7.17285 10.4231 7.00242 10.4234L7.00009 10.4248L6.99775 10.4234C6.82732 10.4231 6.65697 10.3791 6.50395 10.2917L0.511729 6.86757C0.478746 6.84872 0.447382 6.82847 0.417638 6.80694ZM0.065554 9.44729C0.142952 9.31132 0.274499 9.22549 0.417638 9.20147C0.447382 9.223 0.478746 9.24325 0.511729 9.2621L6.50395 12.6862C6.65697 12.7737 6.82732 12.8176 6.99775 12.818L7.00009 12.8193L7.00242 12.818C7.17285 12.8176 7.34321 12.7737 7.49622 12.6862L13.4884 9.2621C13.5214 9.24325 13.5528 9.223 13.5825 9.20147C13.7257 9.22549 13.8572 9.31132 13.9346 9.44729C14.0712 9.68728 13.9874 9.99257 13.7474 10.1292L7.49478 13.6884C7.18809 13.863 6.81208 13.863 6.50539 13.6884L0.252737 10.1292C0.0127516 9.99257 -0.0710532 9.68728 0.065554 9.44729Z"),
                    Fill = new SolidColorBrush(Colors.Black),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                }
            };
            NavigationViewModel allComponents = new NavigationViewModel()
            {
                NavigationItem = "All Controls",
                NavigationIcon = new System.Windows.Shapes.Path()
                {
                    Data = Geometry.Parse("M5.5 1.5H13.5M5.5 6.5H13.5M5.5 11.5H13.5M1 3.5H3C3.27614 3.5 3.5 3.27614 3.5 3V1C3.5 0.723858 3.27614 0.5 3 0.5H1C0.723858 0.5 0.5 0.723858 0.5 1V3C0.5 3.27614 0.723858 3.5 1 3.5ZM1 8.5H3C3.27614 8.5 3.5 8.27614 3.5 8V6C3.5 5.72386 3.27614 5.5 3 5.5H1C0.723858 5.5 0.5 5.72386 0.5 6V8C0.5 8.27614 0.723858 8.5 1 8.5ZM1 13.5H3C3.27614 13.5 3.5 13.2761 3.5 13V11C3.5 10.7239 3.27614 10.5 3 10.5H1C0.723858 10.5 0.5 10.7239 0.5 11V13C0.5 13.2761 0.723858 13.5 1 13.5Z"),
                    Stroke = new SolidColorBrush(Colors.Black),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                }
            };
            if (this.GetType().Name != "SamplesViewModel")
            {
                if (this.WhatsNewDemos.Any())
                {
                    this.HeaderItems.Add(whatsNew);
                }
                this.HeaderItems.Add(allComponents);
            }
            else
            {
                this.HeaderItems.Add(home);
                this.HeaderItems.Add(whatsNew);
                this.HeaderItems.Add(showcaseApplication);
                this.HeaderItems.Add(allComponents);
            }
        }
        /// <summary>
        /// Method helps to create collection for Demos with New,Updated and Preview Tags.
        /// </summary>
        private List<DemoInfo> PopulateWhatsNewDemos()
        {
            List<DemoInfo> products = new List<DemoInfo>();

            return products;
        }

        /// <summary>
        /// Gets or sets the collection of product demos.
        /// </summary>
        public List<ProductDemo> ProductDemos { get; set; }
        public DemoBrowserViewModel()
        {
            clickCommand = new DelegateCommand<object>(OnClicked);
            ProductDemos = GetDemosDetails();
            WhatsNewDemos = PopulateWhatsNewDemos();
            PopulateWhatsNewDemos();
            NavigationItems();
            if (this.HeaderItems.Any())
            {
                this.SelectedItem = this.GetType().Name != "SamplesViewModel" ? this.HeaderItems.Last() : this.HeaderItems.First();
            }
        }

        private void OnClicked(object paramater)
        {
            if (paramater is RadioButton)
            {
                RadioButton button = (RadioButton)paramater;
                UserControl userControl = VisualUtils.FindAncestor(button, typeof(UserControl)) as UserControl;
                if (userControl != null)
                {
                    ListView listView = VisualUtils.FindDescendant(userControl, typeof(ListView)) as ListView;
                    if (listView != null)
                    {
                        listView.GroupStyle.Clear();
                        DataTemplate FirstTemplate = userControl.Resources["ListViewTemplate"] as DataTemplate;
                        DataTemplate SecondTemplate = userControl.Resources["GalleryViewTemplate"] as DataTemplate;
                        GroupStyle groupstyle1 = userControl.Resources["ListViewGroupStyle"] as GroupStyle;
                        GroupStyle groupstyle2 = userControl.Resources["GalleryViewGroupStyle"] as GroupStyle;
                        listView.ItemTemplate = button.Name.ToString() == "galleryViewButton" ? SecondTemplate : FirstTemplate;
                        if (button.Name.ToString() == "galleryViewButton")
                        {
                            listView.GroupStyle.Add(groupstyle2);
                        }
                        else
                        {
                            listView.GroupStyle.Add(groupstyle1);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Maintains the product demo collection
        /// </summary>
        /// <returns>Product demos</returns>
        public abstract List<ProductDemo> GetDemosDetails();

    }
}
