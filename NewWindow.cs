using System;
using System.Drawing;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Rectangle = Avalonia.Controls.Shapes.Rectangle;

namespace PrimSCADA
{
    public class NewWindow : Window
    {
        private PointerPoint? _PointerPoint;
        private PixelPoint _PixelPoint;
        private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            _PointerPoint = e.GetCurrentPoint(null);
            _PixelPoint = Position;
        }

        private void InputElement_OnPointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            _PointerPoint = null;
        }

        private void InputElement_OnPointerMoved(object? sender, PointerEventArgs e)
        {
            if (_PointerPoint != null)
            {
                double x = _PointerPoint.Position.X - e.GetCurrentPoint(null).Position.X;
                double y = _PointerPoint.Position.Y - e.GetCurrentPoint(null).Position.Y;
                
                PixelPoint pp = new PixelPoint((Position.X - (int)x), (Position.Y - (int)y));

                Position = pp;
            }
        }

        private void InputElement_OnPointerLeave(object? sender, PointerEventArgs e)
        {
            _PointerPoint = null;
        }

        private void Button_OnClick(object? sender, RoutedEventArgs e)
        {
            Close();
        }

        private void StyledElement_OnInitialized(object? sender, EventArgs e)
        {
            Screens screens = new Window().Screens;
            PixelRect pr = screens.Primary.Bounds;
            PixelPoint pp = new PixelPoint(pr.BottomRight.X / 5, pr.BottomRight.Y / 5) ;
            Position = pp;
        }

        private void RectangleBound_OnPointerMoved(object? sender, PointerEventArgs e)
        {
            PointerPoint pp = e.GetCurrentPoint(this.FindControl<Rectangle>("RectangleBound"));
            ;
        }
    }
}