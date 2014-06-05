using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace FileNameGovener
{
    public partial class Form1 : Form
    {
        string path = "";
        List<string> GFiles = new List<string>();
        public Form1()
        {
            InitializeComponent();
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path = folderBrowserDialog.SelectedPath;
                LoadDir();
            }
            CalculateDesinatedList();
        }
        private void LoadDir()
        {
            GFiles.Clear();
            if (path != "" && Directory.Exists(path))
            {
                string[] lFiles = Directory.GetFiles(path);
                AlphanumComparator ns = new AlphanumComparator();
                string temp = "";
                for (int write = 0; write < lFiles.Length; write++)
                {
                    for (int sort = 0; sort < lFiles.Length - 1; sort++)
                    {
                        if (ns.Compare(lFiles[sort], lFiles[sort + 1]) > 0)
                        {
                            temp = lFiles[sort + 1];
                            lFiles[sort + 1] = lFiles[sort];
                            lFiles[sort] = temp;
                        }
                    }
                }
                GFiles.AddRange(lFiles);
                UpdateList();
            }
        }
        private void UpdateList()
        {
            lstFiles.Items.Clear();
            lstFiles.Items.AddRange(GFiles.ToArray());
        }
        private void chkReverse_CheckedChanged(object sender, EventArgs e)
        {
            if (GFiles.Count > 0)
            {
                GFiles.Reverse();
                UpdateList();
            }
            else
            {
                chkReverse.Checked = false;
            }
            CalculateDesinatedList();
        }
        private static Random random = new Random((int)DateTime.Now.Ticks);//thanks to McAden
        private string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        private void btnRenameFiles_Click(object sender, EventArgs e)
        {
            if (GFiles.Count > 0)
            {
                string lNewFileName = RandomString(10);
                string lNewAbsolutePath = Path.Combine(path, lNewFileName);
                int lBaseNum = int.Parse(numericUpDown1.Value.ToString());
                Directory.CreateDirectory(lNewAbsolutePath);
                string lExtension = "";
                foreach (string str in GFiles)
                {
                    lExtension = Path.GetExtension(str);
                    File.Copy(str, Path.Combine(lNewAbsolutePath, txtPrefix.Text + (lBaseNum++) + lExtension));
                    if (chkStepTwo.Checked) {
                        lBaseNum++;
                    }
                }
                MessageBox.Show("Done!");
                System.Diagnostics.Process.Start("explorer.exe",lNewAbsolutePath);
            }
            else {
                MessageBox.Show("No file to process.");
            }
        }
        private void CalculateDesinatedList() {
            if (GFiles.Count > 0)
            {
                lstDesignated.Items.Clear();
                int lBaseNum = int.Parse(numericUpDown1.Value.ToString());
                string lExtension = "";
                foreach (string str in GFiles)
                {
                    lExtension = Path.GetExtension(str);
                    lstDesignated.Items.Add(txtPrefix.Text + (lBaseNum++) + lExtension);
                    if (chkStepTwo.Checked)
                    {
                        lBaseNum++;
                    }
                }
            }
        }

        private void chkStepTwo_CheckedChanged(object sender, EventArgs e)
        {
            CalculateDesinatedList();
        }

        private void txtPrefix_TextChanged(object sender, EventArgs e)
        {
            CalculateDesinatedList();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            CalculateDesinatedList();
        }
    }
}