using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Primitives.PopupPositioning;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using Tmds.DBus;

namespace PrimSCADA
{
    public class MessageBoxWindow : Window
    {
        public bool IsVisibleOk
        {
            set
            {
                if (BOk != null)
                {
                    BOk.IsVisible = value;
                }
            }
        }

        public string TextButtonOk
        {
            set
            {
                if (BOk != null)
                {
                    BOk.Content = value;
                }
            }
        }

        public bool IsBOk
        {
            get;
            set;
        }
        private Grid GridMain;
        private TextBox TBMessage;
        private Button BOk;
        private Button BCancel;
        private readonly string SMessage;
        private readonly string SOk;
        private readonly string SClose;
        public MessageBoxWindow()
        {
            
        }
        public MessageBoxWindow(string sMessage)
        {
            InitializeComponent();
            GridMain = this.FindControl<Grid>("GridMain");
            BOk = this.FindControl<Button>("BOk");
            BCancel = this.FindControl<Button>("BClose");
            TBMessage = this.FindControl<TextBox>("TBErrorMessage");
            TBMessage.Text = sMessage;
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        private new void NewWindowOnOpened(object? sender, EventArgs e)
        {
            Screens screens = new Window().Screens;
            PixelRect pr = screens.Primary.Bounds;
            PixelPoint pp = new PixelPoint(pr.BottomRight.X / 5, pr.BottomRight.Y / 5) ;
            Position = pp;
        }
        private void BOkOnClick(object? sender, RoutedEventArgs e)
        {
            IsBOk = true;
            Close();
        }
        private void BCancelOnClick(object? sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}