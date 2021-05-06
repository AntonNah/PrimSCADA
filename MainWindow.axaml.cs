using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;

namespace PrimSCADA
{
    public class MainWindow : Window
    {
        public MainWindow()
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

        /*protected override void HandleWindowStateChanged(WindowState state)
        {
            if (state == WindowState.Minimized)
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    WindowState = WindowState.Normal;
                });
                
            }

            base.HandleWindowStateChanged(state);
        }*/

        private void MenuItem_OnClickExit(object? sender, RoutedEventArgs e)
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                desktopLifetime.Shutdown();
            }
        }

        private void MenuItem_OnClickNew(object? sender, RoutedEventArgs e)
        {
            NewWindow newWindow = new NewWindow();
            newWindow.ShowDialog(this);
        }

        private void MenuItemSettingsOnClick(object? sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog(this);
        }
    }
}