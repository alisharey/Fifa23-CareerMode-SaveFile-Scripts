using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FIFA23.Scripts;

namespace FIFA23.Scripts.UI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private FileHandling _fileHandling;
    private FIFA23.Scripts.Scripts _scripts;
    private CareerInfo careerInfo;
    private bool IsFileLoaded;
    private bool IsLinkLoaded = false;


    public MainWindow()
    {
        InitializeComponent();
        IsFileLoaded = false;
        //statusTextBox.IsEnabled = false;
        //ImportCareerInfo.Foreground = Brushes.White;

    }
    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);

        // Begin dragging the window
        this.DragMove();
    }

    private void ImportCareerInfoButton_Click(object sender, RoutedEventArgs e)
    {
        if (IsFileLoaded && _fileHandling.Type == FileType.Squad)
        {
            if (careerInfo != null)
            {
                _scripts.ImportCareerInfo(careerInfo);
                //statusTextBox.Text = $"Imported careerinfo with teamid{careerInfo.MyTeamID}";
            }
            else
            {
                //statusTextBox.Text = "No Exported Career Info Found.";
            }

        }
        else; //statusTextBox.Text = "Load Squad File First.";
    }
    private void ExportCareerInfo_Click(object sender, RoutedEventArgs e)
    {

        if (IsFileLoaded && _fileHandling.Type == FileType.Career)
        {
            careerInfo = _scripts.ExportCareerInfo();
            //statusTextBox.Text = $"Exported career with teamid{careerInfo.MyTeamID}";
        }
        else LoadCareerFileError();
    }
    private void ApplyButtonOverAllScript_Click(object sender, RoutedEventArgs e)
    {
        if (IsFileLoaded && _fileHandling.Type == FileType.Career)
        {
            //_scripts.TempScriptForAllStats();
            var ret = _scripts.UserTeamSingleStatScript("potential");

            if (ret != -1) ; //statusTextBox.Text = "Script Pot_to99 executed.";
            else
            {
                MessageBox.Show($"Error ret = {ret}");
            }
        }
        else LoadCareerFileError();




    }

    private void ApplyButtonAgeScript_Click(object sender, RoutedEventArgs e)
    {
        if (IsFileLoaded && _fileHandling.Type == FileType.Career)
        {
            var ret = _scripts.UserTeamSingleStatScript("birthdate");
            if (ret != -1) ; //statusTextBox.Text = "Script MyTeamPlayerAgeTo15 executed.";
            else
            {
                MessageBox.Show($"Error ret = {ret}");
            }
        }
        else
        {
            LoadCareerFileError();

        }

    }

    private void LoadCareerFileError()
    {

        //statusTextBox.Text = "Error: Load Career File First";

    }
    private void ShutDownButton_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }




    private void TabablzControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

        if (MainTab.SelectedItem == GitHubTab )
        {
            MainTab.SelectedItem = HomeTab;
            foreach (TabItem tab in MainTab.Items)
            {
                if (tab == HomeTab) tab.IsSelected = true;
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
    private void LoadButton_Click(object sender, RoutedEventArgs e)
    {
        _fileHandling = new FileHandling();
        //statusTextBox.Text = "Loading File ...";
        int ret = _fileHandling.Load();
        if (ret != 0) ; //statusTextBox.Text = "Error: Incompatiable/No Chosen File";
        else
        {
            //statusTextBox.Text = "Loading Complete";
            _scripts = new Scripts(_fileHandling);
            this.IsFileLoaded = true;
        }
    }
    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        //statusTextBox.Text = "Saving File ...";
        if (IsFileLoaded)
        {

            _fileHandling.Save();
            //statusTextBox.Text = "Save Complete";
        }
        else
        {
            //statusTextBox.Text = "File Does Not Exist";
        }

    }

}