using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NotepadClone
{
    public partial class ConfirmSaveForm : Form
    {
        string filepath;

        public ConfirmSaveForm(string fn, string fp)
        {
            InitializeComponent();
            textBox1.Text = "Do you want to save changes to " + fn + "?";
            filepath = fp;
        }

        private void ConfirmSaveForm_Load(object sender, EventArgs e) {   }


        /*Cancel button click
         * closes the saveFormDialog
         */
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        /*Dont save button click
         * Exits the current thread which closes the current form
         */
        private void dontSaveButton_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }


        /*Save button click
         * saves to the file
         * else opens a saveDialog to save file if file does not exits
         */
        private void saveButton_Click(object sender, EventArgs e)
        {
            notepadCloneForm ncf = new notepadCloneForm();

            if (File.Exists(filepath))
            {
                StreamWriter writer = new StreamWriter(filepath);
                writer.Write(ncf.Text);
                writer.Dispose();
                writer.Close();
            }
            else
            {
                ncf.saveAs();
            }

        }
    }
}
