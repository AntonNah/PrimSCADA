using System;
using System.Collections;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
using Point = Avalonia.Point;
using Rectangle = Avalonia.Controls.Shapes.Rectangle;

namespace PrimSCADA
{
    public class CustomWindow : Window
    {
        public Rectangle RectangleBoundWindow;
        public Grid GridMain;
        public PointerPoint? PPHeaderClick;
        public PointerPoint PPRectangleBoundClick;
        public Point PRectangleBoundClick;
        public bool IsLeftClickRectangleBoundWindow;
        public double RectangleBoundWindowWidth;
        public bool IsRightClickRectangleBoundWindow;
        public bool IsBottomClickRectangleBoundWindow;
        public bool IsBottomLeftClickRectangleBoundWindow;
        public bool IsBottomRightClickRectangleBoundWindow;
        public bool IsCursorCapture;
        public double Xdiff;
        public double Ydiff;
        public double PointerPointRectangleBoundWindowX;
        public double PointerPointRectangleBoundWindowY;

        private void NewWindowOnInitialized(object? sender, EventArgs e)
        {
            Screens screens = new Window().Screens;
            PixelRect pr = screens.Primary.Bounds;
            PixelPoint pp = new PixelPoint(pr.BottomRight.X / 5, pr.BottomRight.Y / 5) ;
            Position = pp;
        }
        private void NewWindowOnOpened(object? sender, EventArgs e)
        {
            RectangleBoundWindow = this.FindControl<Rectangle>("RectangleBound");
            GridMain = this.FindControl<Grid>("GridMain");

            ColumnDefinition column2 =  GridMain.ColumnDefinitions[0];
            column2.MaxWidth = 500;
            column2.MinWidth = 200;
        }
        public void NewWindowOnPointerMoved(object? sender, PointerEventArgs e)
        {
            if (IsLeftClickRectangleBoundWindow)
            {
                double x = PPRectangleBoundClick.Position.X - e.GetCurrentPoint(null).Position.X;
                
                if (Width + x > MinWidth && Width + x < MaxWidth)
                {
                    PixelPoint pp = new PixelPoint((Position.X - (int)x), (int)RectangleBoundWindowWidth);
                    Width = Width + x;
                    Position = pp;
                }
            }
            else if (IsRightClickRectangleBoundWindow)
            {
                double x = PointerPointRectangleBoundWindowX - e.GetCurrentPoint(null).Position.X;
                
                if (Width - x > MinWidth && Width - x < MaxWidth)
                {
                    if(x != Xdiff)
                        Width = Width - x;
                    Xdiff = x;
                    PointerPointRectangleBoundWindowX = e.GetCurrentPoint(null).Position.X;
                }
            }
            else if (IsBottomClickRectangleBoundWindow)
            {
                double y = PointerPointRectangleBoundWindowY - e.GetCurrentPoint(null).Position.Y;
                if (Height - y > MinHeight && Height - y < MaxHeight)
                {
                    if(y != Ydiff)
                        Height = Height - y;
                    Ydiff = y;
                    PointerPointRectangleBoundWindowY = e.GetCurrentPoint(null).Position.Y;
                }
            }
            else if (IsBottomLeftClickRectangleBoundWindow)
            {
                double y = PointerPointRectangleBoundWindowY - e.GetCurrentPoint(null).Position.Y;
                double x = PointerPointRectangleBoundWindowX - e.GetCurrentPoint(null).Position.X;
                
                if (Width + x > MinWidth && Width + x < MaxWidth)
                {
                    PixelPoint pp = new PixelPoint((Position.X - (int)x), (int)RectangleBoundWindowWidth);
                    Width = Width + x;
                    Position = pp;
                }

                if (Height - y > MinHeight && Height - y < MaxHeight)
                {
                    if(y != Ydiff)
                        Height = Height - y;
                    Ydiff = y;
                    PointerPointRectangleBoundWindowY = e.GetCurrentPoint(null).Position.Y;
                }
            }
            else if (IsBottomRightClickRectangleBoundWindow)
            {
                double x = PointerPointRectangleBoundWindowX - e.GetCurrentPoint(null).Position.X;
                double y = PointerPointRectangleBoundWindowY - e.GetCurrentPoint(null).Position.Y;
                if (Width - x > MinWidth && Width - x < MaxWidth)
                {
                    if(x != Xdiff)
                        Width = Width - x;
                    Xdiff = x;
                    PointerPointRectangleBoundWindowX = e.GetCurrentPoint(null).Position.X;
                }

                if (Height - y > MinHeight && Height - y < MaxHeight)
                {
                    if(y != Ydiff)
                        Height = Height - y;
                    Ydiff = y;
                    PointerPointRectangleBoundWindowY = e.GetCurrentPoint(null).Position.Y;
                }
            }
        }
        public void HeaderOnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            PPHeaderClick = e.GetCurrentPoint(null);
        }
        public void HeaderOnPointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            PPHeaderClick = null;
        }
        public void HeaderOnPointerMoved(object? sender, PointerEventArgs e)
        {
            if (PPHeaderClick != null)
            {
                double x = PPHeaderClick.Position.X - e.GetCurrentPoint(null).Position.X;
                double y = PPHeaderClick.Position.Y - e.GetCurrentPoint(null).Position.Y;
                
                PixelPoint pp = new PixelPoint((Position.X - (int)x), (Position.Y - (int)y));

                Position = pp;
            }
        }
        public void HeaderOnPointerLeave(object? sender, PointerEventArgs e)
        {
            PPHeaderClick = null;
        }
        public void RectangleBoundOnPointerMoved(object? sender, PointerEventArgs e)
        {
            Rect rect = RectangleBoundWindow.Bounds;
            PRectangleBoundClick = e.GetCurrentPoint(RectangleBoundWindow).Position;

            if (PRectangleBoundClick.Y < rect.Height - (RectangleBoundWindow.StrokeThickness + 3) && 
                PRectangleBoundClick.X <= RectangleBoundWindow.StrokeThickness)
            {
                if (!IsCursorCapture)
                {
                    Cursor = new Cursor(StandardCursorType.LeftSide);
                }
            }
            else if (PRectangleBoundClick.X >= rect.Width - RectangleBoundWindow.StrokeThickness &&
                     PRectangleBoundClick.Y < rect.Height - (RectangleBoundWindow.StrokeThickness + 3))
            {
                if (!IsCursorCapture)
                {
                    Cursor = new Cursor(StandardCursorType.RightSide);
                }
            }
            else if (PRectangleBoundClick.Y >= rect.Height - RectangleBoundWindow.StrokeThickness && 
                     PRectangleBoundClick.X > RectangleBoundWindow.StrokeThickness + 3 &&
                     PRectangleBoundClick.X < rect.Width - (RectangleBoundWindow.StrokeThickness + 3))
            {
                if (!IsCursorCapture)
                {
                    Cursor = new Cursor(StandardCursorType.BottomSide);
                }
            }
            else if (PRectangleBoundClick.X <= RectangleBoundWindow.StrokeThickness &&
                     PRectangleBoundClick.Y >= rect.Height - (RectangleBoundWindow.StrokeThickness +3))
            {
                if (!IsCursorCapture)
                {
                    Cursor = new Cursor(StandardCursorType.BottomLeftCorner);
                }
            }
            else if (PRectangleBoundClick.X >= rect.Width - (RectangleBoundWindow.StrokeThickness +3) &&
                     PRectangleBoundClick.Y >= rect.Height - (RectangleBoundWindow.StrokeThickness +3))
            {
                if (!IsCursorCapture)
                {
                    Cursor = new Cursor(StandardCursorType.BottomRightCorner);
                }
            }
        }
        public void RectangleBoundOnPointerLeave(object? sender, PointerEventArgs e)
        {
            Cursor = new Cursor(StandardCursorType.Arrow);
            IsCursorCapture = false;
        }
        public void RectangleBoundOnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            Rect rect = RectangleBoundWindow.Bounds;
            PPRectangleBoundClick = e.GetCurrentPoint(null);
            PRectangleBoundClick = PPRectangleBoundClick.Position;
            PointerPointRectangleBoundWindowX = PPRectangleBoundClick.Position.X;
            PointerPointRectangleBoundWindowY = PPRectangleBoundClick.Position.Y;
            RectangleBoundWindowWidth = Position.Y;
            
            if (PRectangleBoundClick.Y < rect.Height - (RectangleBoundWindow.StrokeThickness + 3) && 
                PRectangleBoundClick.X <= RectangleBoundWindow.StrokeThickness)
            {
                IsCursorCapture = true;
                IsLeftClickRectangleBoundWindow = true;
            }
            else if (PRectangleBoundClick.X >= rect.Width - RectangleBoundWindow.StrokeThickness &&
                     PRectangleBoundClick.Y < rect.Height - (RectangleBoundWindow.StrokeThickness + 3))
            {
                IsCursorCapture = true;
                IsRightClickRectangleBoundWindow = true;
            }
            else if (PRectangleBoundClick.Y >= rect.Height - RectangleBoundWindow.StrokeThickness && 
                     PRectangleBoundClick.X > RectangleBoundWindow.StrokeThickness + 3 &&
                     PRectangleBoundClick.X < rect.Width - (RectangleBoundWindow.StrokeThickness + 3))
            {
                IsCursorCapture = true;
                IsBottomClickRectangleBoundWindow = true;
            }
            else if (PRectangleBoundClick.X <= RectangleBoundWindow.StrokeThickness &&
                     PRectangleBoundClick.Y >= rect.Height - (RectangleBoundWindow.StrokeThickness +3))
            {
                IsCursorCapture = true;
                IsBottomLeftClickRectangleBoundWindow = true;
            }
            else if (PRectangleBoundClick.X >= rect.Width - (RectangleBoundWindow.StrokeThickness +3) &&
                     PRectangleBoundClick.Y >= rect.Height - (RectangleBoundWindow.StrokeThickness +3))
            {
                IsCursorCapture = true;
                IsBottomRightClickRectangleBoundWindow = true;
            }
        }
        public void RectangleBoundOnPointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            IsLeftClickRectangleBoundWindow = false;
            IsRightClickRectangleBoundWindow = false;
            IsBottomClickRectangleBoundWindow = false;
            IsBottomLeftClickRectangleBoundWindow = false;
            IsBottomRightClickRectangleBoundWindow = false;
            IsCursorCapture = false;
        }
        public void BExitOnClick(object? sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}