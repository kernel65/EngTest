using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.ComponentModel;


namespace EngTestWordsUI
{
    class ImageFactory
    {
        private Form2 form2;
        private String defaultFolder;


        //Папка, в которую загружаются изображения
        private String imagesFolder = @"\Images\";
        public String ImagesFolder
        {
            get
            {
                return imagesFolder;
            }
        }

        public ImageFactory(Form2 form, String folder)
        {
            form2 = form;
            defaultFolder = folder;
        }

        public async Task SaveImageFromUri(String userRequest, List<String> links, String fileName)
        {
            WebClient wClient = new WebClient();

            try
            {

                if (links.Count != 0)
                {
                    String tempUri = links[links.Count > 5 ? Factory.rand.Next(3) : 0];
                    String fullPath = defaultFolder + ImagesFolder + userRequest + ".jpg";

                    wClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Insert_ImageBox);
                    wClient.DownloadFileAsync(new Uri(tempUri), fullPath, userRequest);
                }
                else
                {
                    throw new Exception();
                }

            }
            catch (Exception e)
            {
                try
                {
                    String tempPath = defaultFolder + @"\ProgramsFiles\NoImageFound.jpg";
                    form2.pictureBox.Load(tempPath);
                }
                catch (Exception ex)
                {
                    form2.exceptionLabel.Text = "Невозможно вставить изображение";
                }
            }
        }

        private void Insert_ImageBox(object sender, AsyncCompletedEventArgs e)
        {

            String request = (String)e.UserState;
            String fullPath = defaultFolder + ImagesFolder + request + ".jpg";

            try
            {
                if (request.Equals(Factory.prevEngWord))
                {
                    form2.pictureBox.Load(fullPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Не удалось найти изображение по пути " + fullPath);
            }

        }

        public bool CheckImageInFolder(String folder, String fileName)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            
            string[] files = Directory.GetFiles(folder);
            foreach (string fName in files)
            {
                int index = fName.LastIndexOf(@"\");
                String tempStr = fName.Substring(index + 1, fName.Length - index - 1);

                if (tempStr.Equals(fileName))
                    return true;
            }

            return false;
        }

    }
}
