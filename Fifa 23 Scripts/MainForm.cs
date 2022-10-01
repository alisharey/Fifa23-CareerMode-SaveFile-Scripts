using Fifa_Career_Script;
namespace Fifa_23_Scripts
{
    public partial class MainForm : Form
    {
        private FileHandling _fileHandling;
        private Scripts _scripts;
        private bool IsFileLoaded;
        public MainForm()
        {
            InitializeComponent();
            IsFileLoaded = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {

            _fileHandling = new FileHandling();
            statusTextBox.Text = "Loading File ...";
            int ret = _fileHandling.Load();
            if (ret != 0) statusTextBox.Text = "Error while loading/No File";
            else
            {
                statusTextBox.Text = "Loading Complete";
                _scripts = new Scripts(_fileHandling);
                this.IsFileLoaded = true;
            }







            //Console.WriteLine("File Saved.");
            //Console.Read();
        }

        private void ApplyScript1Button_Click(object sender, EventArgs e)
        {
            if (IsFileLoaded)
            {
                _scripts.MyTeamPlayersto99();
                statusTextBox.Text = "Script MyTeamPlayersto99 executed.";
            }
            else
            {
                LoadFileError();

            }

        }
        private void ApplyScriptAgeButton_Click(object sender, EventArgs e)
        {
            if (IsFileLoaded)
            {
                _scripts.MyTeamPlayerAgeTo15();
                statusTextBox.Text = "Script MyTeamPlayerAgeTo15 executed.";
            }
            else
            {
                LoadFileError();

            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsFileLoaded)
            {
                _fileHandling.Save();
                statusTextBox.Text = "Save Complete";
            }


        }
        private void LoadFileError()
        {

            statusTextBox.Text = "Error: Load Career File First";

        }


    }
}