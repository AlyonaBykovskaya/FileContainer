using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;


namespace FileContainer
{
    public partial class Form1 : Form
    {

        public OpenFileDialog openFiles;
        public List<FileEntry> container = new List<FileEntry>();
        public SaveFileDialog saveFile;
        public FolderBrowserDialog saveDialog;
        public string archive;


        public Form1()
        {
            InitializeComponent();
        }


        private void List_SelectedIndexChanged(object sender, EventArgs e)
        {
            List.Sorted = true;

        }

        private void Create_new_Click(object sender, EventArgs e)//for creating new container
        {

            try
            {
                openFiles = new OpenFileDialog();
                openFiles.Title = "Select the files that you want store in a container";
                openFiles.Filter = "All Files|*.*";
                openFiles.Multiselect = true;
                MessageBox.Show("Select the files which you want to put in a container");
                if (openFiles.ShowDialog() == DialogResult.OK)
                {
                    foreach (String fileName in openFiles.FileNames)
                    {
                        container.Add(new FileEntry()
                        {
                            Name = Path.GetFileName(fileName),
                            Content = File.ReadAllBytes(fileName),
                        });
                    }
                }

            }

            catch
                (Exception ex)
            {
                MessageBox.Show("Error while selecting the files, try again! " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            try
            {
                saveFile = new SaveFileDialog();
                saveFile.Title = "Saving the archive";
                saveFile.FileName = "Archive";
                openFiles.Filter = "File AL|*.al*";
                MessageBox.Show("Select the path to create a container");
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    archive = saveFile.FileName;
                    BinaryFormatter ser = new BinaryFormatter();
                    FileStream fs = new FileStream(archive, FileMode.Create);
                    ser.Serialize(fs, container);
                    List.Items.Clear();
                    foreach (var file in container)
                    {
                        List.Items.Add(file.Name);

                    }
                    fs.Close();
                    MessageBox.Show("Data saved successfully", "Info", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Not selected a location to save the archive, try again! " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }


        private void Open_Container_Click_1(object sender, EventArgs e)// for opening the container 
        {
            try
            {
                openFiles = new OpenFileDialog();
                openFiles.Title = "Select the container that you want to open";
                openFiles.Filter = "File AL|*.al";
                if (openFiles.ShowDialog() == DialogResult.OK)
                {
                    archive = openFiles.FileName;
                }
             
                if (new FileInfo(archive).Length > 0)
                {
                    FileStream fs = new FileStream(archive, FileMode.Open);
                    BinaryFormatter form = new BinaryFormatter();
                    container = form.Deserialize(fs) as List<FileEntry>;
                    List.Items.Clear();
                    foreach (var file in container)
                    {

                        List.Items.Add(file.Name);
                    }
                    fs.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error by selecting a container, try again! " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
     
           
        }

        private void Add_Click(object sender, EventArgs e) //for selecting files to add to the container
        {

            try
            {
                openFiles = new OpenFileDialog();
                openFiles.Title = "Select the files that you want store in a container";
                openFiles.Filter = "All Files|*.*";
                openFiles.Multiselect = true;

                if (openFiles.ShowDialog() == DialogResult.OK)
                {
                    foreach (String fileName in openFiles.FileNames)
                    {
                        container.Add(new FileEntry()
                        {
                            Name = Path.GetFileName(fileName),
                            Content = File.ReadAllBytes(fileName),
                        });
                    }
                }

            }

            catch
                (Exception ex)
            {
                MessageBox.Show("Error while selecting the files, try again! " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            try
            {
                BinaryFormatter ser = new BinaryFormatter();
                FileStream fs = new FileStream(archive, FileMode.Create);
                ser.Serialize(fs, container);
                List.Items.Clear();

                ser.Serialize(fs, container);
                List.Items.Clear();
                foreach (var file in container)
                {
                    List.Items.Add(file.Name);

                }

                MessageBox.Show("Data saved successfully");
                fs.Close();
            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


        }


        private void Extract_to_Click(object sender, EventArgs e) //for selecting files that you want to extract 
        {

            if (List.SelectedIndex > -1)
            {
                try
                {

                    saveDialog = new FolderBrowserDialog();
                    saveDialog.Description = "Select a folder for extracting the files";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        string path = saveDialog.SelectedPath; 
                        FileStream fs = new FileStream(archive, FileMode.Open); 
                        BinaryFormatter formatter = new BinaryFormatter();
                        container = formatter.Deserialize(fs) as List<FileEntry>;
                        List.Text = List.SelectedItem.ToString();
                        FileEntry item = container.FirstOrDefault(en => en.Name == List.Text);
                        File.WriteAllBytes(Path.Combine(path, item.Name), item.Content);
                        fs.Close();
                        MessageBox.Show("File was extracted to the specified folder", "Info", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }

                    else throw new Exception("There Wasn't identified place for extracting file from the container!");


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while selecting the files to be extracted from the container, try again! " + ex.Message,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }
            else
            {
                MessageBox.Show("Please select an Item at first!");
            }
        }
        private void ExtractAll_Click(object sender, EventArgs e)//to extracting all files
        {
            if (container != null && container.Count != 0)
            {
                try
                {
                    string path;

                    saveDialog = new FolderBrowserDialog();
                    saveDialog.Description = "Select a folder for extracting";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        path = saveDialog.SelectedPath;
                        FileStream fs = new FileStream(archive, FileMode.Open);
                        BinaryFormatter formatter = new BinaryFormatter();
                        container = formatter.Deserialize(fs) as List<FileEntry>;

                        if (container != null)
                        {
                            foreach (var file in container)
                            {
                                File.WriteAllBytes(Path.Combine(path, file.Name), file.Content);
                            }
                        }
                        fs.Close();
                        MessageBox.Show("All contents of the container extracted in the specified folder", "Info",
                            MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                 

                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("The container is empty!");
            }

        }

        }


    }

