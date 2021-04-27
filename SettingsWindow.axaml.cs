using System;
using System.Collections;
using System.Collections.Generic;
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

namespace PrimSCADA
{
    public class SettingsWindow : Window
    {
        private Popup PopupMessage;
        private Label LMessage;
        private ListBox LBSettings;
        private Grid GridMain;
        private TextBox TBSolutionDirectory;
        private bool IsShowPopupSolutionName;
        private int SolutionNameLength;
        private char[] InvalidChars;
        private string ValidSolutionName;
        private List<string> CollectionLBSolution;
        
        private new void SettingsWindowOnOpened(object? sender, EventArgs e)
        {
            Screens screens = new Window().Screens;
            PixelRect pr = screens.Primary.Bounds;
            PixelPoint pp = new PixelPoint(pr.BottomRight.X / 5, pr.BottomRight.Y / 5) ;
            Position = pp;
            
            GridMain = this.FindControl<Grid>("GridMain");
            LBSettings = this.FindControl<ListBox>("LBSettings");
            
            ColumnDefinition column2 =  GridMain.ColumnDefinitions[0];
            column2.MaxWidth = 600;
            column2.MinWidth = 200;
            
            InvalidChars = new char[] {'"', '/', '\\', '<', '>', '?', '*', '|', ':'};
            
            SolutionNameLength = 70;
            
            CollectionLBSolution = new List<string>();

            LMessage = new Label();

            Border border = new Border();
            border.Background = Brushes.Red;
            border.Child = LMessage;

            PopupMessage = new Popup();
            PopupMessage.IsLightDismissEnabled = true;
            PopupMessage.PlacementAnchor = PopupAnchor.Bottom;
           // PopupMessage.PlacementTarget = TBSolutionName;
            PopupMessage.Child = border;
            
            GridMain.Children.Add(PopupMessage);

            CollectionLBSolution.Add("General");

            Binding bLBSolution = new Binding();
            bLBSolution.Source = CollectionLBSolution;
            
            LBSettings.Bind(ListBox.ItemsProperty, bLBSolution);
            LBSettings.SelectedIndex = 0;
        }
        private void LBSettingsOnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            IList list = e.AddedItems;

            if (list[0] == "General")
            {
                Grid gridEmptySolution = new Grid();
                Grid.SetColumn(gridEmptySolution, 2);
                Grid.SetRow(gridEmptySolution, 1);
                GridMain.Children.Add(gridEmptySolution);

                RowDefinition rw = new RowDefinition(GridLength.Auto);
                RowDefinition rw2 = new RowDefinition(GridLength.Auto);
                RowDefinition rw3 = new RowDefinition(GridLength.Auto);
                RowDefinition rw4 = new RowDefinition(GridLength.Auto);

                ColumnDefinition cm = new ColumnDefinition(GridLength.Auto);
                ColumnDefinition cm2 = new ColumnDefinition(GridLength.Auto);
                
                gridEmptySolution.RowDefinitions.Add(rw);
                gridEmptySolution.RowDefinitions.Add(rw2);
                gridEmptySolution.RowDefinitions.Add(rw3);
                gridEmptySolution.RowDefinitions.Add(rw4);
                gridEmptySolution.ColumnDefinitions.Add(cm);
                gridEmptySolution.ColumnDefinitions.Add(cm2);
                
                Label labelCaption = new Label();
                labelCaption.FontStyle = FontStyle.Italic;
                labelCaption.Content = "General";

                Label labelSolutionDirectory = new Label();
                labelSolutionDirectory.Content = "Solution directory:";
                Grid.SetRow(labelSolutionDirectory, 1);

                Button bBrowse = new Button();
                bBrowse.Click += BBrowseOnClick;
                bBrowse.Content = "Browse";
                
                TBSolutionDirectory = new TextBox();
                TBSolutionDirectory.Text = ((App) Application.Current).Settings.DirectoryPath;

                StackPanel stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Horizontal;
                stackPanel.Children.Add(TBSolutionDirectory);
                stackPanel.Children.Add(bBrowse);
                Grid.SetRow(stackPanel, 1);
                Grid.SetColumn(stackPanel, 1);

                CheckBox chbCreateDirectory = new CheckBox();
                chbCreateDirectory.Content = "Create directory for the solution";
                Grid.SetRow(chbCreateDirectory, 2);
                Grid.SetColumn(chbCreateDirectory, 1);

                DockPanel dPanel = new DockPanel();
                
                Button bCreate = new Button();
                bCreate.Content = "Save";
                bCreate.Click += BCreateOnClick;

                Button bCancel = new Button();
                bCancel.Content = "Cancel";
                bCancel.Click += BCancelOnClick;

                StackPanel sPanel = new StackPanel();
                Grid.SetRow(sPanel, 3);
                Grid.SetColumnSpan(sPanel, 2);
                sPanel.Orientation = Orientation.Horizontal;
                sPanel.HorizontalAlignment = HorizontalAlignment.Right;
                sPanel.Children.Add(bCreate);
                sPanel.Children.Add(bCancel);

                gridEmptySolution.Children.Add(labelCaption);
                gridEmptySolution.Children.Add(labelSolutionDirectory);
                gridEmptySolution.Children.Add(stackPanel);
                gridEmptySolution.Children.Add(chbCreateDirectory);
                gridEmptySolution.Children.Add(sPanel);
            }
        }

        public async Task<string> GetPath()
        {
            OpenFolderDialog openFolderDialog = new OpenFolderDialog();
            openFolderDialog.Directory = TBSolutionDirectory.Text;
            string s = await openFolderDialog.ShowAsync(this);
            return s;
        }

        private async void BBrowseOnClick(object? sender, RoutedEventArgs e)
        {
            string s = await GetPath();
            if (s != "")
            {
                TBSolutionDirectory.Text = s;
            }
        }

        private void OnNext(string s)
        {
            if (s != null)
            {
                if (s.Length > SolutionNameLength)
                {
                    LMessage.Content = "Solution length must not exceed " + SolutionNameLength + " characters.";
                    
                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        IsShowPopupSolutionName = true;
                        //return TBSolutionName.Text = ValidSolutionName;
                    });
                    
                    PopupMessage.IsOpen = true;
                }
                else if (s.IndexOfAny(InvalidChars) != -1)
                {
                    LMessage.Content = "solution name must not contain characters: < > | \" / \\ * : ?";
                    
                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        IsShowPopupSolutionName = true;
                        //return TBSolutionName.Text = ValidSolutionName;
                    });
                    
                    PopupMessage.IsOpen = true;
                }
                else
                {
                    if (!IsShowPopupSolutionName)
                    {
                        PopupMessage.IsOpen = false;
                    }
                    IsShowPopupSolutionName = false;
                    
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