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
    [Flags]
    public enum MessageWindowButtons
    {
        None = 0,
        Yes = 1,
        No = 2,
        Cancel = 4,
        All = ~None,
    }
    
    public class BitOperations
    {
        public MessageWindowButtons Value { get; private set; }

        public void Add(MessageWindowButtons value)
        {
            Value |= value;
        }

        public void Remove(MessageWindowButtons value)
        {
            Value ^= value;
        }

        public bool Contains(MessageWindowButtons value)
        {
            return (Value & value) == value;
        }

        public override string ToString()
        {
            Value.ToString("G");
        }

        public BitOperations()
        {
            Value = MessageWindowButtons.None;
        }

        public BitOperations(MessageWindowButtons value)
        {
            Value = value;
        }
    }
    public class MessageWindow : Window
    {
        private Grid GridMain;
        private TextBox TBErrorMessage;
        private Button BOk;
        private Button BClose;
        private readonly string SErrorMessage;
        private readonly string SOk;
        private readonly string SClose;

        public MessageWindow()
        {
            
        }
        public MessageWindow(string s, string ok, string close)
        {
            InitializeComponent();
            SErrorMessage = s;
            SOk = ok;
            SClose = close;
        }
        public MessageWindow(string s, string close)
        {
            InitializeComponent();
            SErrorMessage = s;
            SClose = close;
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
            
            GridMain = this.FindControl<Grid>("GridMain");
            BOk = this.FindControl<Button>("BOk");
            BClose = this.FindControl<Button>("BClose");
            TBErrorMessage = this.FindControl<TextBox>("TBErrorMessage");
            TBErrorMessage.Text = SErrorMessage;
            
            if()

        }
        private void BOkOnClick(object? sender, RoutedEventArgs e)
        {
            Close();
        }
        private void BCloseOnClick(object? sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}