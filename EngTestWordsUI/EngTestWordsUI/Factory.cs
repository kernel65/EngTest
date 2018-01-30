using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;

namespace EngTestWordsUI
{
    class Factory
    {
        public static Random rand = new Random();
        public String prevEngWord = "";
        public String prevRuWord = "";
        private Form2 form2;
        private String fileNameJson = "JsonFilePixabay.json";
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

        public void form2Ini()
        {
            Dictionary<String, String> words = engTest.getRandomWord();
            prevEngWord = engTest.getEngWord(words);
            prevRuWord = engTest.getRuWord(words);

            startSearchImage(prevEngWord, this.defaultFolder);
            form2.label1.Text = prevEngWord;
            form2.userCharLabel.Text = "'"  + WordsRepository.EngUserChar.ToUpper() + "'";
        }

        public void startTest()
        {
            engTest.equalsWords(prevEngWord, prevRuWord, form2.requestField.Text.ToLower());
            Dictionary<String, String> words = engTest.getRandomWord();
            prevEngWord = engTest.getEngWord(words);
            prevRuWord = engTest.getRuWord(words);
            
            startSearchImage(prevEngWord, this.defaultFolder);
            form2.label1.Text = prevEngWord;
            form2.requestField.Text = "";
        }

        public async Task startSearchImage(String word, String defFolder)
        {
            //Проверяем наличие файла
            if(iFactory.checkImageInFolder(defFolder + @"\Images\", word + ".jpg"))
            {
                setImageOnPictureBox(word + ".jpg");
                return;
            }
            
            //Ожидаем завершения, иначе выполнится следующий код раньше.
            if (CheckForInternetConnection())
            {
                await pImages.getLinks(word, "webformatURL");
                pImages.saveLinksToFile(fileNameLinks, pImages.Links, DefaultFolder);
            }
            
            iFactory.saveImageFromUri(word, pImages.Links, "test");
        }

        private void setImageOnPictureBox(String fileName)
        {
            try
            {
                String tempPath = this.defaultFolder + @"\Images\" + fileName;
                form2.pictureBox.Load(tempPath);
            }
            catch(Exception e)
            {

            }
        }

        private bool CheckForInternetConnection()
        {
            try
            {
                Ping myPing = new Ping();
                String host = "google.com";
                byte[] buffer = new byte[32];
                int timeout = 500;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                return (reply.Status == IPStatus.Success);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
