using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace EngTestWordsUI
{
    public partial class Form1 : Form
    {
        private Factory factory;
        private String WordsFileNameFolder;


        public Form1()
        {
            InitializeComponent();
            WordsFileNameFolder = Directory.GetCurrentDirectory() + @"\ProgramsFiles\words.txt";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            iniComboBox();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void apply_Click(object sender, EventArgs e)
        {
            initialization();

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog opFileDialog = new OpenFileDialog();
            opFileDialog.Filter = "Text Files|*.txt";
            opFileDialog.Title = "Выберите текстовый файл со словами";
            opFileDialog.InitialDirectory = Directory.GetCurrentDirectory() + @"\ProgramsFiles\";


            if (opFileDialog.ShowDialog() == DialogResult.OK)
                if (opFileDialog.FileName != String.Empty)
                {
                    WordsFileNameFolder = opFileDialog.FileName;
                    editFolderTbox.Text = WordsFileNameFolder;
                }
                    
        }

        //Инициализируем, прежде чем открыть 2-ю форму
        private void initialization()
        {
            List<String> lines = WordsRepository.ReadWordsFromTxtFile(WordsFileNameFolder);

            try
            {
                if (lines != null)
                {
                    
                    WordsRepository.ParseLines(lines);
                    if(!(comboBox1.SelectedItem == null))
                        WordsRepository.EngUserChar = comboBox1.SelectedItem.ToString().ToLower();
                    Form2 f2 = new Form2();
                    f2.Form1 = this;
                    f2.Show();
                    this.Hide();
                }
                else
                    throw new Exception();
            }
            catch(Exception e)
            {
                MessageBox.Show("Неверный файл, выберите другой!" +
                                "\nФормат файла - английские слова, должны быть разделены дефисом");
            }



        }

        private void iniComboBox()
        {
            String tempAll = "All";

            for (int i = 0; i<WordsRepository.chars.Length; i++)
            {
                 this.comboBox1.Items.Insert(i, WordsRepository.chars[i].ToUpper());
            }
            this.comboBox1.Items.Insert(comboBox1.Items.Count, tempAll);
            this.comboBox1.SelectedItem = tempAll;
        }
    }
}
