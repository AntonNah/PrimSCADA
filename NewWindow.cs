﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
    public class NewWindow : CustomWindow
    {
        private Grid GridMain;
        private ListBox LBSolution;
        private TextBox TBSolutionName;
        private bool IsShowToolTip;
        private string ValidSolutionName;
        private List<string> CollectionLBSolution;

        private void NewWindowOnInitialized(object? sender, EventArgs e)
        {
            CollectionLBSolution = new List<string>();
            
            TBSolutionName = new TextBox();

            Screens screens = new Window().Screens;
            PixelRect pr = screens.Primary.Bounds;
            PixelPoint pp = new PixelPoint(pr.BottomRight.X / 5, pr.BottomRight.Y / 5) ;
            Position = pp;
        }
        private void NewWindowOnOpened(object? sender, EventArgs e)
        {
            CollectionLBSolution.Add("Empty solution");
            
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
        private void LBSolutionOnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            IList list = e.AddedItems;

            if (list[0] == "Empty solution")
            {
                Grid gridEmptySolution = new Grid();
                Grid.SetColumn(gridEmptySolution, 2);
                Grid.SetRow(gridEmptySolution, 1);
                GridMain.Children.Add(gridEmptySolution);

                RowDefinition rw = new RowDefinition(GridLength.Auto);
                RowDefinition rw2 = new RowDefinition(GridLength.Auto);
                RowDefinition rw3 = new RowDefinition(GridLength.Auto);
                RowDefinition rw4 = new RowDefinition(GridLength.Auto);
                RowDefinition rw5 = new RowDefinition(GridLength.Auto);

                ColumnDefinition cm = new ColumnDefinition(GridLength.Auto);
                ColumnDefinition cm2 = new ColumnDefinition(GridLength.Auto);
                
                gridEmptySolution.RowDefinitions.Add(rw);
                gridEmptySolution.RowDefinitions.Add(rw2);
                gridEmptySolution.RowDefinitions.Add(rw3);
                gridEmptySolution.RowDefinitions.Add(rw4);
                gridEmptySolution.RowDefinitions.Add(rw5);
                gridEmptySolution.ColumnDefinitions.Add(cm);
                gridEmptySolution.ColumnDefinitions.Add(cm2);
                
                Label labelCaption = new Label();
                labelCaption.FontStyle = FontStyle.Italic;
                labelCaption.Content = "Empty solution";

                Label labelSolutionName = new Label();
                labelSolutionName.Content = "Solution name:";
                Grid.SetRow(labelSolutionName, 1);

                Label labelSolutionDirectory = new Label();
                labelSolutionDirectory.Content = "Solution directory:";
                Grid.SetRow(labelSolutionDirectory, 2);
                
                TBSolutionName.GetObservable(TextBox.TextProperty).Subscribe(OnNext);
                Grid.SetRow(TBSolutionName, 1);
                Grid.SetColumn(TBSolutionName, 1);

                TextBox tbSolutionDirectory = new TextBox();
                tbSolutionDirectory.Text = ((App) Application.Current).Settings.DirectoryPath;
                Grid.SetRow(tbSolutionDirectory, 2);
                Grid.SetColumn(tbSolutionDirectory, 1);

                CheckBox chbCreateDirectory = new CheckBox();
                chbCreateDirectory.Content = "Create directory for the solution";
                Grid.SetRow(chbCreateDirectory, 3);
                Grid.SetColumn(chbCreateDirectory, 1);

                DockPanel dPanel = new DockPanel();
                
                Button bCreate = new Button();
                bCreate.Content = "Create";
                bCreate.Click += BCreateOnClick;

                Button bCancel = new Button();
                bCancel.Content = "Cancel";
                bCancel.Click += BCancelOnClick;
                
                StackPanel sPanel = new StackPanel();
                Grid.SetRow(sPanel, 4);
                Grid.SetColumnSpan(sPanel, 2);
                sPanel.Orientation = Orientation.Horizontal;
                sPanel.HorizontalAlignment = HorizontalAlignment.Right;
                sPanel.Children.Add(bCreate);
                sPanel.Children.Add(bCancel);

                gridEmptySolution.Children.Add(labelCaption);
                gridEmptySolution.Children.Add(labelSolutionName);
                gridEmptySolution.Children.Add(TBSolutionName);
                gridEmptySolution.Children.Add(labelSolutionDirectory);
                gridEmptySolution.Children.Add(tbSolutionDirectory);
                gridEmptySolution.Children.Add(chbCreateDirectory);
                gridEmptySolution.Children.Add(sPanel);
            }
        }

        private void OnNext(string s)
        {
            if (s != null)
            {
                if (s.Length > 50)
                {
                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        IsShowToolTip = true;
                        return TBSolutionName.Text = ValidSolutionName;
                    });
                }
                else
                {
                    if (!IsShowToolTip)
                    {
                        
                    }
                    IsShowToolTip = false;
                    
                    ValidSolutionName = s;
                }
            }
        }

        private void BCancelOnClick(object? sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BCreateOnClick(object? sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}