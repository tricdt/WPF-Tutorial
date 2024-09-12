using Syncfusion.SfSkinManager;
using System.Windows;

namespace syncfusion.demoscommon.wpf
{
    public static class DemoLaucherExtension
    {
        public static T LauchDemo<T>(DemoBrowserViewModel viewmodel, DemoInfo demoInfo) where T : DependencyObject
        {
            T demo = null;
            var constructorInfo = demoInfo.DemoViewType?.GetConstructors().FirstOrDefault(cinfo => cinfo.IsPublic && cinfo.GetParameters().Length == 1 && cinfo.GetParameters()[0].Name == "themename");
            if (demoInfo.ThemeMode != ThemeMode.None && constructorInfo != null)
            {
                demo = Activator.CreateInstance(demoInfo.DemoViewType,
                    demoInfo.ThemeMode == ThemeMode.Inherit ? viewmodel.SelectedThemeName : DemoBrowserViewModel.DefaultThemeName) as T;
            }
            else if (demoInfo.DemoViewType != null)
            {
                demo = Activator.CreateInstance(demoInfo.DemoViewType) as T;
                if (demoInfo.ThemeMode == ThemeMode.Inherit)
                {
                    SfSkinManager.SetTheme(demo, new Theme() { ThemeName = viewmodel.SelectedThemeName });
                }
                else if (demoInfo.ThemeMode == ThemeMode.Default)
                {
                    SfSkinManager.SetTheme(demo, new Theme() { ThemeName = DemoBrowserViewModel.DefaultThemeName });
                }
            }
            return demo;
        }
    }
    public class LaunchDemoAction
    {
    }
}
