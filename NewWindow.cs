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
using Avalonia.Media;
using Avalonia.Threading;

namespace PrimSCADA
{
    public class NewWindow : Window
    {
        private Popup PopupMessage;
        private Label LMessage;
        private Grid GridMain;
        private ListBox LBSolution;
        private TextBox TBSolutionName;
        private TextBox TBSolutionDirectory;
        private bool IsShowPopupSolutionName;
        private int SolutionNameLength;
        private char[] InvalidChars;
        private string ValidSolutionName;
        private List<string> CollectionLBSolution;
        
        private new void NewWindowOnOpened(object? sender, EventArgs e)
        {
            Screens screens = new Window().Screens;
            PixelRect pr = screens.Primary.Bounds;
            PixelPoint pp = new PixelPoint(pr.BottomRight.X / 5, pr.BottomRight.Y / 5) ;
            Position = pp;
            
            GridMain = this.FindControl<Grid>("GridMain");
            LBSolution = this.FindControl<ListBox>("LBSolution");
            
            ColumnDefinition column2 =  GridMain.ColumnDefinitions[0];
            column2.MaxWidth = 600;
            column2.MinWidth = 200;
            
            InvalidChars = new char[] {'"', '/', '\\', '<', '>', '?', '*', '|', ':'};
            
            SolutionNameLength = 70;
            
            CollectionLBSolution = new List<string>();
            TBSolutionName = new TextBox();
            
            LMessage = new Label();

            Border border = new Border();
            border.Background = Brushes.Red;
            border.Child = LMessage;

            PopupMessage = new Popup();
            PopupMessage.IsLightDismissEnabled = true;
            PopupMessage.PlacementAnchor = PopupAnchor.Bottom;
            PopupMessage.PlacementTarget = TBSolutionName;
            PopupMessage.Child = border;
            
            GridMain.Children.Add(PopupMessage);

            CollectionLBSolution.Add("Empty solution");

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

                ColumnDefinition cm = new ColumnDefinition(GridLength.Auto);
                ColumnDefinition cm2 = new ColumnDefinition(GridLength.Auto);
                
                gridEmptySolution.RowDefinitions.Add(rw);
                gridEmptySolution.RowDefinitions.Add(rw2);
                gridEmptySolution.RowDefinitions.Add(rw3);
                gridEmptySolution.RowDefinitions.Add(rw4);
                gridEmptySolution.ColumnDefinitions.Add(cm);
                gridEmptySolution.ColumnDefinitions.Add(cm2);

                TextBlock textCaption = new TextBlock();
                textCaption.Text = "Empty solution";
                textCaption.FontWeight = FontWeight.Bold;

                Label labelSolutionName = new Label();
                labelSolutionName.Content = "Solution name:";
                Grid.SetRow(labelSolutionName, 1);

                Label labelSolutionDirectory = new Label();
                labelSolutionDirectory.Content = "Solution directory:";
                Grid.SetRow(labelSolutionDirectory, 2);
                
                TBSolutionName.GetObservable(TextBox.TextProperty).Subscribe(SolutionTextChange);
                TBSolutionName.Text = "";
                Grid.SetRow(TBSolutionName, 1);
                Grid.SetColumn(TBSolutionName, 1);

                Button bBrowse = new Button();
                bBrowse.Click += BBrowseOnClick;
                bBrowse.Content = "Browse";
                
                TBSolutionDirectory = new TextBox();
                TBSolutionDirectory.Text = ((App) Application.Current).Settings.DirectoryPath;

                StackPanel stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Horizontal;
                stackPanel.Children.Add(TBSolutionDirectory);
                stackPanel.Children.Add(bBrowse);
                Grid.SetRow(stackPanel, 2);
                Grid.SetColumn(stackPanel, 1);

                DockPanel dPanel = new DockPanel();
                
                Button bCreate = new Button();
                bCreate.Content = "Create";
                bCreate.Click += BCreateOnClick;

                Button bCancel = new Button();
                bCancel.Content = "Cancel";
                bCancel.Click += BCancelOnClick;

                StackPanel sPanel = new StackPanel();
                sPanel.Margin = new Thickness(0, 100, 0, 0);
                Grid.SetRow(sPanel, 3);
                Grid.SetColumnSpan(sPanel, 2);
                sPanel.Orientation = Orientation.Horizontal;
                sPanel.HorizontalAlignment = HorizontalAlignment.Right;
                sPanel.Children.Add(bCreate);
                sPanel.Children.Add(bCancel);

                gridEmptySolution.Children.Add(textCaption);
                gridEmptySolution.Children.Add(labelSolutionName);
                gridEmptySolution.Children.Add(TBSolutionName);
                gridEmptySolution.Children.Add(labelSolutionDirectory);
                gridEmptySolution.Children.Add(stackPanel);
                gridEmptySolution.Children.Add(sPanel);
            }
        }
        private async Task<string> GetPath()
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
        private void SolutionTextChange(string s)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (s != null)
            {
                if (s.Length > SolutionNameLength)
                {
                    LMessage.Content = "Solution length must not exceed " + SolutionNameLength + " characters.";
                    
                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        IsShowPopupSolutionName = true;
                        return TBSolutionName.Text = ValidSolutionName;
                    });
                    
                    PopupMessage.IsOpen = true;
                }
                else if (s.IndexOfAny(InvalidChars) != -1)
                {
                    LMessage.Content = "solution name must not contain characters: < > | \" / \\ * : ?";
                    
                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        IsShowPopupSolutionName = true;
                        return TBSolutionName.Text = ValidSolutionName;
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
        private async Task<bool> MessageBox()
        {
            MessageBoxWindow mb = new MessageBoxWindow("File with that name already exists, overwrite it?");
            mb.IsVisibleOk = true;
            mb.TextButtonOk = "Yes";
            await mb.ShowDialog(this);
            return mb.IsBOk;
        }
        private async void BCreateOnClick(object? sender, RoutedEventArgs e)
        {
            try
            {
                if (((App)Application.Current).Settings.CreateDirectory)
                {
                    string s = TBSolutionDirectory.Text + "\\" + TBSolutionName.Text;
                    
                    if (File.Exists(s + "\\" + TBSolutionName.Text + ".ps"))
                    {
                        if (await MessageBox())
                        {
                            
                        }
                    }
                    
                    Directory.CreateDirectory(s);
                    File.Create(s + "\\" + TBSolutionName.Text + ".ps");
                }
                else
                {
                    File.Create(TBSolutionDirectory.Text + "\\" + TBSolutionName.Text + ".ps");
                }
            }
            catch (Exception exception)
            {
                MessageBoxWindow errorWindow = new MessageBoxWindow(exception.Message);
                errorWindow.ShowDialog(this);
                return;
            }
            
            Close();
        }
    }
}