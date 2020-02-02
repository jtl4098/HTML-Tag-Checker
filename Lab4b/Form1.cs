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

/// <summary>
/// Program : Form1.cs
/// @author : Taekyung Kil
/// Date : 14/Nov/2019
/// 
/// Purpose : To set the Form
/// </summary>
namespace Lab4b
{

    public partial class Form1 : Form
    {
        //set global variables
        string fileName = "";
        string fileText = "";
        
        public Form1()
        {
            InitializeComponent();
            
        }

        /// <summary>
        /// this button is to load a html file using file dialog
        /// loading file should be a html file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox.Clear();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "html files (*.html)|*.html";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                
                fileName = openFileDialog.FileName;
                statusLabel.Text = $"Loaded : {fileName}";
                StreamReader reader = new StreamReader(fileName);
                fileText = reader.ReadToEnd();
            }
            checkTagsToolStripMenuItem.Enabled = true; //To enable to use check tags button after loading a file
        }
        /// <summary>
        /// this button is to exit the application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// It is used for checking html tags from html file.
        /// users are able to check opening tags, closing tags and non-container tags.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckTagsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            List<string> tagList = new List<string>();
            Stack<string> htmltag = new Stack<string>();
            string tag = "";
            // Store html tag into the tagList such as <html> , <title> and </html>.
            for (int i = 0; i < fileText.Length; i++)
            {
                if (fileText[i] == '<')
                {
 
                    tag += fileText[i];
                    for (int j = i + 1; j < fileText.Length; j++)
                    {

                        tag += fileText[j];
                        if (fileText[j] == '>')
                        {
                            tagList.Add(tag);
                            tag = "";
                            break;
                        }
                    }
                }
            }


            string[] nonContainerTags = { "<br>" }; // set non-container Tags list. it is created a list to be easy to add non-container tags
            int indentationcount = 0; //To get indentation based on the stack list
            // To check which tags are opening or closing or non-contatiner.
            // if a tag contains '/', it is a closing tag and remove it in the stack list using the pop method
            // if a tag contains "<hr>" or "<img>" or "<br>", it is a closing tag and remove it in the stack list using the pop method
            // else a tag does not contain, the tag is a opening tag and add it in the stack list using the push method
            for (int i = 0; i < tagList.Count; i++)
            {
                if (tagList[i].IndexOf('/') == 1)
                {
                    indentationcount--;
                    htmltag.Pop();
                    textBox.Text += new string(' ', indentationcount) + $"Found closing tag: {tagList[i]} \r\n";

                }           
                else if (tagList[i].Contains("<hr>"))
                {
                    textBox.Text += $"Found non-container tag: {tagList[i]} \r\n";
                }              
                else if (tagList[i].Contains("img"))
                {
                    
                    tagList[i] = "<img>";                   
                    textBox.Text += $"Found non-container tag: {tagList[i]} \r\n";
                }
                else if (tagList[i].Contains("href"))
                {
                    
                   
                    tagList[i] = "<a>";
                    htmltag.Push(tagList[i]);
                    textBox.Text += new string(' ', indentationcount) + $"Found opening tag: {tagList[i]} \r\n";
                    indentationcount++;
                }
                else if (tagList[i].Contains("table"))
                {
                    
                   
                    tagList[i] = "<table>";
                    htmltag.Push(tagList[i]);
                    textBox.Text += new string(' ', indentationcount) + $"Found opning tag: {tagList[i]} \r\n";
                    indentationcount++;

                }
                else if (tagList[i].Contains("<td"))
                {
                    
                   
                    tagList[i] = "<td>";
                    htmltag.Push(tagList[i]);
                    textBox.Text += new string(' ', indentationcount) + $"Found opning tag: {tagList[i]} \r\n";
                    indentationcount++;

                }
                else if (nonContainerTags.Contains<string>(tagList[i]))
                {                   
                    textBox.Text += $"Found non-container tag: {tagList[i]} \r\n";

                }
                else
                {
                    textBox.Text += new string(' ', indentationcount) + $"Found opening tag: {tagList[i]} \r\n";
                    indentationcount++;
                    htmltag.Push(tagList[i]);
                    
                }

              
            }
            // To display the html tags on text box 
            foreach (string t in htmltag)
            {
                textBox.Text += t;
            }

            if (htmltag.Count > 0) // To check that the html file has balanced tags or not
            {
                statusLabel.Text = $"{fileName} dose not have balanced tags";
                textBox.Text += "\r\n Need more Closing tag";
            }
            else
            {
                statusLabel.Text = $"{fileName} has balanced tags";
            }
         
        }
    } 
}
