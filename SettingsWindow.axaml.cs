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
using ProtoBuf;

namespace PrimSCADA
{
    public class SettingsWindow : Window
    {
        private Popup PopupMessage;
        private Label LMessage;
        private ListBox LBSettings;
        private Grid GridMain;
        private TextBox TBSolutionDirectory;
        private CheckBox ChbCreateDirectory;
        private bool IsShowPopupSolutionName;
        private int SolutionNameLength;
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

            SolutionNameLength = 70;
            
            CollectionLBSolution = new List<string>();

            LMessage = new Label();

            Border border = new Border();
            border.Background = Brushes.Red;
            border.Child = LMessage;

            PopupMessage = new Popup();
            PopupMessage.IsLightDismissEnabled = true;
            PopupMessage.PlacementAnchor = PopupAnchor.Bottom;
            PopupMessage.PlacementTarget = TBSolutionDirectory;
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

                TextBlock textCaption = new TextBlock();
                textCaption.Text = "General";
                textCaption.FontWeight = FontWeight.Bold;
  
                Label labelSolutionDirectory = new Label();
                labelSolutionDirectory.Content = "Solution directory:";
                Grid.SetRow(labelSolutionDirectory, 1);

                Button bBrowse = new Button();
                bBrowse.Content = "Browse";
                bBrowse.Click += BBrowseOnClick;

                TBSolutionDirectory = new TextBox();
                TBSolutionDirectory.Text = ((App) Application.Current).Settings.DirectoryPath;

                StackPanel stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Horizontal;
                stackPanel.Children.Add(TBSolutionDirectory);
                stackPanel.Children.Add(bBrowse);
                Grid.SetRow(stackPanel, 1);
                Grid.SetColumn(stackPanel, 1);

                ChbCreateDirectory = new CheckBox();
                ChbCreateDirectory.IsChecked = ((App) Application.Current).Settings.CreateDirectory;
                ChbCreateDirectory.Content = "Create directory for the solution";
                Grid.SetRow(ChbCreateDirectory, 2);
                Grid.SetColumn(ChbCreateDirectory, 1);

                Button bSaveAndClose = new Button();
                bSaveAndClose.Content = "Save and close";
                bSaveAndClose.Click += BSaveAndCloseOnClick;
                
                Button bSave = new Button();
                bSave.Content = "Save";
                bSave.Click += BSaveOnClick;

                Button bCancel = new Button();
                bCancel.Content = "Cancel";
                bCancel.Click += BCancelOnClick;

                StackPanel sPanel = new StackPanel();
                Grid.SetRow(sPanel, 3);
                Grid.SetColumnSpan(sPanel, 2);
                sPanel.Orientation = Orientation.Horizontal;
                sPanel.HorizontalAlignment = HorizontalAlignment.Right;
                sPanel.Children.Add(bSaveAndClose);
                sPanel.Children.Add(bSave);
                sPanel.Children.Add(bCancel);

                gridEmptySolution.Children.Add(textCaption);
                gridEmptySolution.Children.Add(labelSolutionDirectory);
                gridEmptySolution.Children.Add(stackPanel);
                gridEmptySolution.Children.Add(ChbCreateDirectory);
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

        private void BCancelOnClick(object? sender, RoutedEventArgs e)
        {
            Close();
        }
        
        private void BSaveAndCloseOnClick(object? sender, RoutedEventArgs e)
        {
            Save();
            Close();
        }

        private void BSaveOnClick(object? sender, RoutedEventArgs e)
        {
            Save();
        }
        
        void Save()
        {
            string s = Directory.GetCurrentDirectory() + @"\Settings.bin";
            Settings settings = ((App)Application.Current).Settings;
            settings.CreateDirectory = (bool)ChbCreateDirectory.IsChecked;
            settings.DirectoryPath = TBSolutionDirectory.Text;
            using (var file = File.Create(s))
            {
                Serializer.Serialize(file, settings);
            }
        }
    }
}