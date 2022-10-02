using System;
using System.Collections.Generic;
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
public partial class MainWindow : Wpf.Ui.Controls.UiWindow
{
    private FileHandling _fileHandling;
    private FIFA23.Scripts.Scripts _scripts;
    private bool IsFileLoaded;

    public MainWindow()
    {
        InitializeComponent();
        IsFileLoaded = false;
        statusTextBox.IsEnabled = false;

    }

    private void ApplyButtonOverAllScript_Click(object sender, RoutedEventArgs e)
    {
        if (IsFileLoaded)
        {
            _scripts.UserTeamSingleStatScript("potential");
            statusTextBox.Text = "Script Pot_to99 executed.";
        }
        else
        {
            LoadFileError();

        }
    }

    private void ApplyButtonAgeScript_Click(object sender, RoutedEventArgs e)
    {
        if (IsFileLoaded)
        {
            _scripts.UserTeamSingleStatScript("birthdate");
            statusTextBox.Text = "Script MyTeamPlayerAgeTo15 executed.";
        }
        else
        {
            LoadFileError();

        }

    }

    private void LoadFileError()
    {

        statusTextBox.Text = "Error: Load Career File First";

    }
    private void ExitButton_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        statusTextBox.Text = "Saving File ...";
        if (IsFileLoaded)
        {
            
            _fileHandling.Save();
            statusTextBox.Text = "Save Complete";
        }
        else
        {
            statusTextBox.Text = "File Does Not Exist";
        }

    }

    private void OpenButton_Click(object sender, RoutedEventArgs e)
    {
        _fileHandling = new FileHandling();
        statusTextBox.Text = "Loading File ...";
        int ret = _fileHandling.Load();
        if (ret != 0) statusTextBox.Text = "Error: Incompatiable/No Chosen File";
        else
        {
            statusTextBox.Text = "Loading Complete";
            _scripts = new Scripts(_fileHandling);
            this.IsFileLoaded = true;
        }
    }
}
