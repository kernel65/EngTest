using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EngTestWordsUI
{
    public partial class Form2 : Form
    {
        private Factory factory;

        private Form1 form1 = null;
        public Form1 Form1
        {
            get { return form1; }
            set { form1 = value; }
        }

        public Form2()
        {
            InitializeComponent();
            factory = new Factory(this);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            iniComboBox();
            factory.Form2Ini();           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            factory.StartTest();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(Object sender, FormClosingEventArgs e)
        {
            Form1.Close();
        }

        private void wordChangeBox_SelectedIndexChanged(object sender,EventArgs args)
        {
            String selectedWord = ((ComboBox)sender).SelectedItem.ToString().ToLower();
            WordsRepository.EngUserChar = selectedWord;
            this.userCharLabel.Text = "'" + selectedWord.ToUpper() + "'";
        }

        private void iniComboBox()
        {

            for (int i = 0; i < WordsRepository.CurrentChars.Count; i++)
            {
                this.wordChangeBox.Items.Insert(i, WordsRepository.CurrentChars[i].ToUpper());
            }
            this.wordChangeBox.Items.Insert(wordChangeBox.Items.Count, "All");
            this.wordChangeBox.SelectedItem = form1.comboBox1.SelectedItem;
           
        }
    }
}
