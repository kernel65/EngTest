using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Net;
using System.IO;
using System.ComponentModel;
using System.Collections.Specialized;


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


        public bool saveImageFromUri(String userRequest, List<String> links, String fileName)
        {
            WebClient wClient = new WebClient();
            String picName = userRequest;         

            try
            {

                if (links.Count != 0)
                {           
                    String tempUri = links[links.Count > 5 ? Factory.rand.Next(3) : 0];
                    String fullPath = defaultFolder + ImagesFolder + picName + ".jpg";

                    wClient.DownloadFileCompleted += new AsyncCompletedEventHandler(insert_ImageBox);
                    wClient.DownloadFileAsync(new Uri(tempUri), fullPath, fullPath);                  
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
                return false;
            }

            return true;
        }
        
        private void insert_ImageBox(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                String fullPath = (String)e.UserState;
                form2.pictureBox.Load(fullPath);                
            }
            catch(FileNotFoundException ex)
            {
 
            }
            
        }

        public bool checkImageInFolder(String folder,String fileName)
        {
            string[] files = Directory.GetFiles(folder);

            foreach(string fName in files)
            {
                int index = fName.LastIndexOf(@"\");
                String tempStr = fName.Substring(index + 1, fName.Length - index - 1);

                //Оператор == не работает, вероятно из-за кодировки
                if (tempStr.Equals(fileName))
                    return true;
            }

            return false;
        }

    }
}
