using System.Configuration;
using System.Data;
using System.Windows;

namespace GUI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        LoadTheme();
    }

    public static void SaveTheme(string colorHex)
    {
        try
        {
            System.IO.File.WriteAllText("theme.txt", colorHex);
        }
        catch { }
    }

    private void LoadTheme()
    {
        try
        {
            if (System.IO.File.Exists("theme.txt"))
            {
                string colorHex = System.IO.File.ReadAllText("theme.txt");
                var color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(colorHex);
                this.Resources["PrimaryColor"] = color;
                this.Resources["PrimaryBrush"] = new System.Windows.Media.SolidColorBrush(color);
            }
        }
        catch { }
    }
}

