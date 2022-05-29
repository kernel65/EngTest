using System;
using System.IO;


namespace EngTestWordsUI
{
    class Factory
    {
        public static Random rand = new Random();
        public static String prevEngWord = "";
        public static String prevRuWord = "";
        private Form2 form2;
        private String fileNameLinks = "LinksFileName.txt";
        private PixabayImages pImages;
        private ImageFactory iFactory;
        private EngTest engTest;


        //Текущий каталог
        private String defaultFolder = Directory.GetCurrentDirectory();
        public String DefaultFolder
        {
            get { return this.defaultFolder; }
            set { }
        }
        //Путь к стандартному файлу
        private String wordsFileNameFolder = Directory.GetCurrentDirectory() + @"\ProgramsFiles\words.txt";
        public String WordsFileNameFolder
        {
            get { return this.wordsFileNameFolder; }
            set { }
        }

        public Factory(Form2 f2)
        {
            form2 = f2;
            pImages = new PixabayImages(f2);
            iFactory = new ImageFactory(f2, DefaultFolder);
            engTest = new EngTest(f2, rand);
        }

        public void Form2Ini()
        {
            var words = engTest.GetRandomWord();
            prevEngWord = engTest.GetEngWord(words);
            prevRuWord = engTest.GetRuWord(words);

            StartSearchImage(prevEngWord, this.defaultFolder);
            form2.label1.Text = prevEngWord;
            form2.userCharLabel.Text = "'" + WordsRepository.EngUserChar.ToUpper() + "'";
        }

        public void StartTest()
        {
            engTest.EqualsWords(prevEngWord, prevRuWord, form2.requestField.Text.ToLower());
            var words = engTest.GetRandomWord();
            prevEngWord = engTest.GetEngWord(words);
            prevRuWord = engTest.GetRuWord(words);

            StartSearchImage(prevEngWord, this.defaultFolder);
            form2.label1.Text = prevEngWord;
            form2.requestField.Text = "";
        }

        public async void StartSearchImage(String word, String defFolder)
        {
            //Проверяем наличие файла
            if (iFactory.CheckImageInFolder(defFolder + @"\Images\", word + ".jpg"))
            {
                SetImageOnPictureBox(word + ".jpg");
                return;
            }

            await pImages.GetLinks(word, "webformatURL");
            await iFactory.SaveImageFromUri(word, pImages.Links, "test");
        }

        private void SetImageOnPictureBox(String fileName)
        {
            try
            {
                String tempPath = this.defaultFolder + @"\Images\" + fileName;
                form2.pictureBox.Load(tempPath);
            }
            catch (Exception e)
            {

            }
        }
    }
}
