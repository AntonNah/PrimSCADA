using System;
using System.Collections;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Point = Avalonia.Point;
using Rectangle = Avalonia.Controls.Shapes.Rectangle;

namespace PrimSCADA
{
    public class NewWindow : Window
    {
        private Rectangle RectangleBoundWindow;
        private Grid GridMain;
        private ListBox LBSolution;
        private PointerPoint? PPHeaderClick;
        private PointerPoint PPRectangleBoundClick;
        private Point PRectangleBoundClick;
        private bool IsLeftClickRectangleBoundWindow;
        private double RectangleBoundWindowWidth;
        private bool IsRightClickRectangleBoundWindow;
        private bool IsBottomClickRectangleBoundWindow;
        private bool IsBottomLeftClickRectangleBoundWindow;
        private bool IsBottomRightClickRectangleBoundWindow;
        private bool IsCursorCapture;
        private double Xdiff;
        private double Ydiff;
        private double PointerPointRectangleBoundWindowX;
        private double PointerPointRectangleBoundWindowY;
        private List<string> CollectionLBSolution;

        private void NewWindowOnInitialized(object? sender, EventArgs e)
        {
            CollectionLBSolution = new List<string>();

            Screens screens = new Window().Screens;
            PixelRect pr = screens.Primary.Bounds;
            PixelPoint pp = new PixelPoint(pr.BottomRight.X / 5, pr.BottomRight.Y / 5) ;
            Position = pp;
        }
        private void NewWindowOnOpened(object? sender, EventArgs e)
        {
            CollectionLBSolution.Add("Empty solution");
            
            RectangleBoundWindow = this.FindControl<Rectangle>("RectangleBound");
            GridMain = this.FindControl<Grid>("GridMain");
            LBSolution = this.FindControl<ListBox>("LBSolution");
            
            ColumnDefinition column2 =  GridMain.ColumnDefinitions[0];
            column2.MaxWidth = 500;
            column2.MinWidth = 200;
            
            Binding bLBSolution = new Binding();
            bLBSolution.Source = CollectionLBSolution;
            
            LBSolution.Bind(ListBox.ItemsProperty, bLBSolution);
            LBSolution.SelectedIndex = 0;
        }
        private void NewWindowOnPointerMoved(object? sender, PointerEventArgs e)
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
        private void HeaderOnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            PPHeaderClick = e.GetCurrentPoint(null);
        }
        private void HeaderOnPointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            PPHeaderClick = null;
        }
        private void HeaderOnPointerMoved(object? sender, PointerEventArgs e)
        {
            if (PPHeaderClick != null)
            {
                double x = PPHeaderClick.Position.X - e.GetCurrentPoint(null).Position.X;
                double y = PPHeaderClick.Position.Y - e.GetCurrentPoint(null).Position.Y;
                
                PixelPoint pp = new PixelPoint((Position.X - (int)x), (Position.Y - (int)y));

                Position = pp;
            }
        }
        private void HeaderOnPointerLeave(object? sender, PointerEventArgs e)
        {
            PPHeaderClick = null;
        }
        private void RectangleBoundOnPointerMoved(object? sender, PointerEventArgs e)
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
        private void RectangleBoundOnPointerLeave(object? sender, PointerEventArgs e)
        {
            Cursor = new Cursor(StandardCursorType.Arrow);
            IsCursorCapture = false;
        }
        private void RectangleBoundOnPointerPressed(object? sender, PointerPressedEventArgs e)
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
        private void RectangleBoundOnPointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            IsLeftClickRectangleBoundWindow = false;
            IsRightClickRectangleBoundWindow = false;
            IsBottomClickRectangleBoundWindow = false;
            IsBottomLeftClickRectangleBoundWindow = false;
            IsBottomRightClickRectangleBoundWindow = false;
            IsCursorCapture = false;
        }
        private void BExitOnClick(object? sender, RoutedEventArgs e)
        {
            Close();
        }

        private void LBSolutionOnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            IList list = e.AddedItems;

            if (list[0] == "Empty solution")
            {
                Label label = new Label();
                label.FontStyle = FontStyle.Italic;
                label.Content = "Empty solution";
                Grid.SetColumn(label, 2);
                Grid.SetRow(label, 1);
                
                GridMain.Children.Add(label);
            }
        }
    }
}