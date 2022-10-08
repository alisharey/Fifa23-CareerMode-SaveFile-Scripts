using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Win32;
using System.Collections.Generic;
using Path = System.IO.Path;
using MessageBox = System.Windows.MessageBox;
using Application = System.Windows.Application;
using System.ComponentModel;

namespace FIFA23.Scripts.UI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    const string CareerFileError = "Error: Load Career File First";

    private FileHandling _fileHandling;
    private FIFA23.Scripts.Scripts _scripts;
    private CareerInfo careerInfo;
    private bool IsFileLoaded;
    private bool IsLinkLoaded;

    private bool IsEntireTeam;
    private FileType _fileType;
    private string _fileName;

    private Dictionary<string, string> MyTeamPlayers { get; set; }


    public MainWindow()
    {
        InitializeComponent();
        IsFileLoaded = false;
        IsLinkLoaded = false;
        MyTeamPlayers = new Dictionary<string, string>();
        this.StatsComboBox.ItemsSource = Scripts.PlayerStats;
        
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);
        this.DragMove();
    }

    private bool OpenFile(string title)
    {

        #region File Dialog and type 
        OpenFileDialog openDialog = new();
        string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        openDialog.InitialDirectory = Path.Combine(userPath, @"OneDrive\Documents\FIFA 23\settings");
        openDialog.Title = title;
        var result = openDialog.ShowDialog();
        if (result == true)
        {

            _fileName = openDialog.FileName;
        }
        if (string.IsNullOrEmpty(_fileName)) return false;

        //check the file type 
        bool ret;
        var fileName = Path.GetFileName(_fileName);
        if (fileName.StartsWith("Squad"))

        {
            _fileType = FileType.Squad;
            ret = true;
        }
        else if (fileName.StartsWith("Career"))
        {
            _fileType = FileType.Career;
            ret = true;
        }
        else
        {

            MessageBox.Show("Select a file which starts with Squads or Career and try again");
            ret = false;




        }
        #endregion
        return ret;


    }
    private async Task LoadFile(string title = "Open a Squad or Career File")
    {

        if (!OpenFile(title)) return;


        PopUpMessage("Loading..");
        await Task.Run(() =>
        {
            _fileHandling = new FileHandling(this._fileType);
            _fileHandling.Load(_fileName);
            _fileHandling.LoadDb();
            _scripts = new Scripts(_fileHandling);
            this.IsFileLoaded = true;

        });


        if (IsFileLoaded) PopUpMessage("Loading Complete");
    }

    private async void LoadButton_Click(object sender, RoutedEventArgs e)
    {
        await LoadFile();
        if (_fileType == FileType.Career) SaveCareerInfo();

    }
    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        var message = string.Empty;
        try
        {

            if (IsFileLoaded)
            {
                PopUpMessage("saving..");
                await Task.Run(() =>
                {
                    _fileHandling.Save();
                });

                message = "Save Complete";
            }
            else message = "No file is loaded.";


        }
        catch (Exception ex)
        {
            message = ex.Message;
        }

        PopUpMessage(message);


    }
    private async void ExportButton_Click(object sender, RoutedEventArgs e)
    {
        var filename = "";
        if (IsFileLoaded)
        {

            await Task.Run(() => { filename = _fileHandling.ExportToXL(); });

        }
        else
        {
            PopUpMessage("Load a file first.");
            return;
        }

        PopUpMessage($"File: {filename}.xlsx is saved in your current app directory as the first database.");
    }
    private async void ImportCareerInfoButton_Click(object sender, RoutedEventArgs e)
    {
        await LoadFile("Open a Career Mode file to get the data from");
        if (!SaveCareerInfo())
        {
            PopUpMessage("Error: Try again by Loading a career file then a squad file in order.");
            return;
        }

        await LoadFile("Open the targeted squad file");
        ImportCareerInfoTask();
    }
    private void ImportCareerInfoTask()
    {
        string message;
        if (IsFileLoaded && _fileType == FileType.Squad && careerInfo != null)
        {

            _scripts.ImportCareerInfo(careerInfo);
            message = $"Imported career info with Team ID {careerInfo.MyTeamID}";


        }
        else
        {
            message = "Error: Try again by Loading a career file then a squad file in order.";

        }

        PopUpMessage(message);
    }
    private bool SaveCareerInfo()
    {

        if (IsFileLoaded && _fileHandling.Type == FileType.Career)
        {
            careerInfo = _scripts.ExportCareerInfo();

            playerComboBox.ItemsSource = careerInfo.MyTeamPlayerNamesDict;
            playerComboBox.SelectedValuePath = "Key";
            playerComboBox.DisplayMemberPath = "Value";

            return true;

            //PopUpMessage("Career info saved, load a squad file to import the info.");
        }
        else
        {
            return false;
            //PopUpMessage(CareerFileError);
        }

    }



    private void CareerScript1Button_Click(object sender, RoutedEventArgs e)
    {
       




    }
    private void CareerScript2Button_Click(object sender, RoutedEventArgs e)
    {      


    }
    private void SquadScriptOverallButton_Click(object sender, RoutedEventArgs e) //merge this
    {
      
    }
    private void SquadSingleStatButton_Click(object sender, RoutedEventArgs e)
    {
        
    }



    private void PopUpMessage(string message, string ActionContent = "OK")
    {
        if (!CheckAccess())
        {
            Dispatcher.BeginInvoke(() => PopUpMessage(message, ActionContent));
        }
        else
        {
            StatusSnackBar.MessageQueue.Enqueue(message,
               ActionContent,
               param => Trace.WriteLine("Actioned: " + param),
               message);
        }


    }





    private void TabablzControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

        if (MainTab.SelectedItem == GitHubTab)
        {
            MainTab.SelectedItem = SquadTab;
            foreach (TabItem tab in MainTab.Items)
            {
                if (tab == SquadTab) tab.IsSelected = true;
                else tab.IsSelected = false;

            }

            if (!IsLinkLoaded)
            {
                var destinationurl = "https://github.com/alisharey/Fifa23-CareerMode-SaveFile-Scripts/";
                var sInfo = new System.Diagnostics.ProcessStartInfo(destinationurl)
                {
                    UseShellExecute = true,
                };
                System.Diagnostics.Process.Start(sInfo);
                IsLinkLoaded = true;
            }
            else IsLinkLoaded = false;




        }



    }
    private void ShutDownButton_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }



    private void NumericOnly(System.Object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        e.Handled = IsTextNumeric(e.Text);

    }


    private static bool IsTextNumeric(string str)
    {
        System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^0-9]");
        return reg.IsMatch(str);

    }

    private void ValueTextBox_KeyDown(object sender, KeyEventArgs e)
    {
        var value = ValueTextBox.Text;
         


        try
        {

            if (e.Key == Key.Enter && !string.IsNullOrWhiteSpace(value) && StatsComboBox.SelectedValue != null
           && (playerComboBox.SelectedValue != null || IsEntireTeam))
            {

                var intValue = int.Parse(value);
                var stat = (string)StatsComboBox.SelectedValue;


                if (intValue >= 1 && intValue <= 99)
                {
                    if (!IsEntireTeam)
                    {
                        string? playerID = playerComboBox.SelectedValue as string;
                        var playerName = playerComboBox.Text;
                        _scripts.SetPlayerStat(playerID, stat, intValue);
                        PopUpMessage($"{playerName} {stat} is set to {intValue}");

                    }
                    else // entire team
                    {
                        var playerIDs = new List<string>();
                        foreach (KeyValuePair<string, string> kvp in playerComboBox.Items)
                        {
                         
                            playerIDs.Add(kvp.Key);
                           
                            
                        }

                        _scripts.SetPlayerStat(playerIDs, stat, intValue);
                        if (stat == "birthdate") intValue = 17;
                        PopUpMessage($"Entire Team {stat} is set to {intValue}");
                    }

                }

                else
                {
                    PopUpMessage($"{stat} value is Invalid");
                }



            }
        }
        catch (Exception ex)
        {
            PopUpMessage($"Error: {ex.Message}");
        }

    }

    private void entireTeamCheckBox_Click(object sender, RoutedEventArgs e)
    {
      if(entireTeamCheckBox.IsChecked != null)
        {
            playerComboBox.IsEnabled = (bool)(!entireTeamCheckBox.IsChecked);
            IsEntireTeam = !playerComboBox.IsEnabled;
        }
    }
            
}
