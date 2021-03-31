using System;
using System.Drawing;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Point = Avalonia.Point;
using Rectangle = Avalonia.Controls.Shapes.Rectangle;

namespace PrimSCADA
{
    public class NewWindow : Window
    {
        private Rectangle RectangleBoundWindow;
        private Label label;
        private PointerPoint? _PointerPoint;
        private PointerPoint PointerPointRectangleBoundWindow;
        private Point PointRectangleBoundWindow;
        private bool IsLeftClickRectangleBoundWindow;
        private double RectangleBoundWindowWidth;
        private bool IsRightClickRectangleBoundWindow;
        private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            _PointerPoint = e.GetCurrentPoint(null);
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

        private void NewWindow_OnInitialized(object? sender, EventArgs e)
        {
            Screens screens = new Window().Screens;
            PixelRect pr = screens.Primary.Bounds;
            PixelPoint pp = new PixelPoint(pr.BottomRight.X / 5, pr.BottomRight.Y / 5) ;
            Position = pp;
        }

        private void RectangleBound_OnPointerMoved(object? sender, PointerEventArgs e)
        {
            Rect rect = RectangleBoundWindow.Bounds;
            PointRectangleBoundWindow = e.GetCurrentPoint(RectangleBoundWindow).Position;

            if (PointRectangleBoundWindow.Y > RectangleBoundWindow.StrokeThickness && PointRectangleBoundWindow.X == 0)
            {
                Cursor = new Cursor(StandardCursorType.LeftSide);
            }
            else if (PointRectangleBoundWindow.X == rect.Width - 1 &&
                     PointRectangleBoundWindow.Y > RectangleBoundWindow.StrokeThickness)
                Cursor = new Cursor(StandardCursorType.RightSide);
        }

        private void TopLevel_OnOpened(object? sender, EventArgs e)
        {
            RectangleBoundWindow = this.FindControl<Rectangle>("RectangleBound");
            label = this.FindControl<Label>("Test");
        }

        private void RectangleBound_OnPointerLeave(object? sender, PointerEventArgs e)
        {
            Cursor = new Cursor(StandardCursorType.Arrow);
        }

        private void RectangleBound_OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            Rect rect = RectangleBoundWindow.Bounds;
            PointRectangleBoundWindow = e.GetCurrentPoint(RectangleBoundWindow).Position;
            PointerPointRectangleBoundWindow = e.GetCurrentPoint(null);
            RectangleBoundWindowWidth = Position.Y;
            if (PointRectangleBoundWindow.Y > RectangleBoundWindow.StrokeThickness && PointRectangleBoundWindow.X <= RectangleBoundWindow.StrokeThickness)
            {
                IsLeftClickRectangleBoundWindow = true;
            }
            else if (PointRectangleBoundWindow.X == rect.Width - 1 &&
                     PointRectangleBoundWindow.Y >= RectangleBoundWindow.StrokeThickness)
            {
                IsRightClickRectangleBoundWindow = true;
            }
        }

        private void RectangleBound_OnPointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            IsLeftClickRectangleBoundWindow = false;
            IsRightClickRectangleBoundWindow = false;
        }

        private void NewWindow_OnPointerMoved(object? sender, PointerEventArgs e)
        {
            if (IsLeftClickRectangleBoundWindow)
            {
                double x = PointerPointRectangleBoundWindow.Position.X - e.GetCurrentPoint(null).Position.X;

                PixelPoint pp = new PixelPoint((Position.X - (int)x), (int)RectangleBoundWindowWidth);
                Width = Width + x;

                Position = pp;
            }
            else if (IsRightClickRectangleBoundWindow)
            {
                label.Content = "TRUE";
                double x = PointerPointRectangleBoundWindow.Position.X - e.GetCurrentPoint(null).Position.X;
                Width = Width - x;
            }
        }

        private void NewWindow_OnPointerLeave(object? sender, PointerEventArgs e)
        {
            label.Content = "FALSE";
        }
    }
}