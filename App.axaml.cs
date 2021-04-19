using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ProtoBuf;

namespace PrimSCADA
{
    public class App : Application
    {
        public Settings Settings;
        public override void Initialize()
        {
            string s = Directory.GetCurrentDirectory() + @"\Settings.bin";
            if (!File.Exists(s))
            {
                Settings = new Settings();
                using (var file = File.Create(s))
                {
                    Serializer.Serialize(file, Settings);
                }
            }
            else
            {
                using (var file = File.OpenRead(s)) 
                {
                    Settings = Serializer.Deserialize<Settings>(file);
                }
            }
            
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}