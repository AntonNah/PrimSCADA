using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PrimSCADA
{
    public class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}