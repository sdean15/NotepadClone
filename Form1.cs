using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NotepadClone
{
    public partial class notepadCloneForm : Form
    {
        public notepadCloneForm()
        {
            InitializeComponent();
            textBox1.SelectionStart = 1;
            textBox1.SelectionLength = 1;
            this.FormClosing += notepadCloneForm_FormClosing;
        }


        //Global vars
        float defaultFontSizePercent;
        string filename = "Unititled";
        string filepath;
        bool isDirty = false;

        


        /*Form closing action
         * check if file saved, if not, prompt user before exiting
         */
        private void notepadCloneForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(isDirty)
            {
                ConfirmSaveForm confirmSaveForm = new ConfirmSaveForm(filename, filepath);
                DialogResult res = confirmSaveForm.ShowDialog();

                //if pressed cancel, stop from closing
                if(res == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }

            }
            else
            {
                Application.ExitThread();
            }
        }



        /*Exit item
         * exits when clicked
        */
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if(!(File.Exists(filepath)))
            {
                ConfirmSaveForm confirmSaveForm = new ConfirmSaveForm(filename, filepath);
                confirmSaveForm.ShowDialog();

            }
            else
            {
                Application.ExitThread();
            }
            
        }



        /*New item
         * sets text and title to default null values
         */
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox1.Font = new Font(textBox1.Font.FontFamily, 12f);
            defaultFontSizePercent = 100.0f;

        }

        /*Event after child form is closed
         * 
         */
        private void childForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Form.ActiveForm.Close();
            //Application.ExitThread();
            //this.Close();
        }



        /*New Notepad Clone instance
         * Creates and runs a new instance of Notepad Clone
         */
        private void OpenNewApp()
        {
            Application.Run(new notepadCloneForm());
        }


        /*New window item
         * opens a new window instance of the form
         */
        private void newWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //create a new NotepadClone instance and show
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(this.OpenNewApp));
            t.Start();

        }


        /*On form load
         * sets font size percent to 100
         */
        private void NotepadClone_Load(object sender, EventArgs e)
        {
            defaultFontSizePercent = 100.0f;
        }



        /*About toolstrip item
         * opens a about dialog form
         */
        private void aboutNotepadCloneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutWindow abtWin = new AboutWindow();
            abtWin.ShowDialog();
        }




        /*Zoom in toolstrip item
         * Zooms in the text by increasing font size
         */
        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(defaultFontSizePercent < 500.0f)
            {
                defaultFontSizePercent += 10;
                textBox1.Font = new Font(textBox1.Font.FontFamily, ((defaultFontSizePercent) / 8.33f));
                toolStripStatusLabel2.Text = defaultFontSizePercent.ToString() + "%";
            }
            
        }




        /*Zoom out toolstrip item
         * Zooms out the text by decreasing font size
         */
        private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(defaultFontSizePercent > 50.0f)
            {
                defaultFontSizePercent -= 10;
                textBox1.Font = new Font(textBox1.Font.FontFamily, ((defaultFontSizePercent) / 8.33f));
                toolStripStatusLabel2.Text = defaultFontSizePercent.ToString() + "%";
            }
                
        }



        /*Restore deault toolstrip item
         * Puts the zoom back to 100%
         */
        private void restoreDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            defaultFontSizePercent = 100.0f;
            textBox1.Font = new Font(textBox1.Font.FontFamily, 12f);
            toolStripStatusLabel2.Text = defaultFontSizePercent.ToString() + "%";
        }




        /*Word wrap toolstrip item
         * enable/disables word wrap for text
         */
        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.WordWrap = !(textBox1.WordWrap);
        }




        /*Font toolstrip item
         * Opens font options dialog to change font settings
         */
        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();

            fd.Font = textBox1.Font;

            
            if(fd.ShowDialog() != DialogResult.Cancel)
            {
                textBox1.Font = fd.Font;
            }
            
        }



        /*Open toolstrip item
         * opens a file dialog to open a existing file to display
         */
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            
            if(fileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileContent = System.IO.File.ReadAllText(fileDialog.FileName);
                textBox1.Text = fileContent;
            }
        }



        /*Selection changed
         * ?
         */
        private void textBox1_SelectionChanged(object sender, EventArgs e)
        {

        }


        /*Text changed
         * called when the text in textbox1 changes
         */
        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            int line = (1 + textBox1.GetLineFromCharIndex(textBox1.SelectionStart));
            int col = (1 + textBox1.SelectionStart - (textBox1.GetFirstCharIndexFromLine(1 + textBox1.GetLineFromCharIndex(textBox1.SelectionStart) - 1)));

            toolStripStatusLabel1.Text = "Ln" + " " + line + " " + "Col" + " " + col;

            notepadCloneForm.ActiveForm.Text = "*" + filename + "  -Notepad";
            isDirty = true;

        }


        /*Save As
         * method for when user wants to save when the file does not currently exists
         */
        public void saveAs()
        {
            SaveFileDialog saveFile = new SaveFileDialog();


            saveFile.FileName = ".txt";
            saveFile.Filter = "Text file | *.txt";


            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(saveFile.OpenFile());
                FileInfo fileInfo = new FileInfo(saveFile.FileName);
                filename = fileInfo.Name;
                filepath = saveFile.FileName;

                writer.Write(textBox1.Text);


                notepadCloneForm.ActiveForm.Text = filename + "  -Notepad";

                writer.Dispose();

                writer.Close();

            }
        }


        /*Save File
         * method for when the user wants to save the file that currently exists
         */
        public void saveFile()
        {
            if (File.Exists(filepath))
            {
                StreamWriter writer = new StreamWriter(filepath);
                writer.Write(textBox1.Text);
                writer.Dispose();
                writer.Close();

                notepadCloneForm.ActiveForm.Text = filename + "  -Notepad";
            }
            else
            {
                saveAs();
            }

        }


        /*Save As toolstrip menu item
         * 
         */
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveAs();          
        }


        /*Save toolstrip menu item
         * 
         */
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFile();   
        }



        /*print document on print page
         * method for drawing the txt to the printing page
         */
        private void PrintDocumentOnPrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(this.textBox1.Text, this.textBox1.Font, Brushes.Black, 10, 25);
        }



        /*Print toolstrip menu item
         * 
         */
        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            PrintDocument doc = new PrintDocument();
            doc.PrintPage += PrintDocumentOnPrintPage;
            printDialog.Document = doc;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                doc.Print();
            }
        }


        /*Page Setup toolstrip menu item
         * 
         */
        private void pageSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PageSetupDialog pageSetupDialog = new PageSetupDialog();
            pageSetupDialog.PageSettings = new System.Drawing.Printing.PageSettings();
            pageSetupDialog.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
            pageSetupDialog.ShowNetwork = false;

            DialogResult result = pageSetupDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                object[] results = new object[]{
                pageSetupDialog.PageSettings.Margins,
                pageSetupDialog.PageSettings.PaperSize,
                pageSetupDialog.PageSettings.Landscape,
                pageSetupDialog.PrinterSettings.PrinterName,
                pageSetupDialog.PrinterSettings.PrintRange};
            }
        }


        /*Undo toolstrip menu item
         * 
         */
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Determine if last operation can be undone in text box.   
            if (textBox1.CanUndo == true)
            {
                // Undo the last operation.
                textBox1.Undo();
                // Clear the undo buffer to prevent last action from being redone.
                textBox1.ClearUndo();
            }
        }


        /*Cut toolstrip menu item
         * 
         */
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Ensure that text is currently selected in the text box.   
            if (textBox1.SelectedText != "")
                // Cut the selected text in the control and paste it into the Clipboard.
                textBox1.Cut();
        }


        /*Copy toolstrip menu item
         * 
         */
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Ensure that text is selected in the text box.   
            if (textBox1.SelectionLength > 0)
                // Copy the selected text to the Clipboard.
                textBox1.Copy();
        }


        /*Paste toolstrip menu item
         * 
         */
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Determine if there is any text in the Clipboard to paste into the text box.
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) == true)
            {
                // Determine if any text is selected in the text box.
                if (textBox1.SelectionLength > 0)
                {
                    // Ask user if they want to paste over currently selected text.
                    if (MessageBox.Show("Do you want to paste over current selection?", "Cut Example", MessageBoxButtons.YesNo) == DialogResult.No)
                        // Move selection to the point after the current selection and paste.
                        textBox1.SelectionStart = textBox1.SelectionStart + textBox1.SelectionLength;
                }
                // Paste current text in Clipboard into text box.
                textBox1.Paste();
            }

        }


        /*Delete toolstrip menu item
         * 
         */
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(textBox1.SelectionLength > 0)
            {
                int a = textBox1.SelectionLength;
                textBox1.Text = textBox1.Text.Remove(textBox1.SelectionStart, a);
            }
        }


        /*Select All toolstrip menu item
         * selects all text in textbox
         */
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                textBox1.HideSelection = false;
                textBox1.Focus();
                textBox1.SelectAll();  
            }
                
        }


        /*Time/Date toolstrip menu item
         * returns the current time and date
         */
        private void timeDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.AppendText(DateTime.Now.ToString());
        }
    }
}
