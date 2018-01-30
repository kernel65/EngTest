using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Web;

namespace EngTestWordsUI
{
    class PixabayImages
    {

        private Form2 form2;
        
        //Поле, содержащее uri ссылки к изображениям
        private List<String> links = new List<string>();
        public List<String> Links
        {
            get { return this.links; }        
        }

        //Поле, которое позволяет получить доступ к api pixabay
        private String pixabayKey = "5756798-f08e699c5ad3614c060f872b4";
        public String PixabayKey
        {
            get { return this.pixabayKey; }
        }


        //Строка, содержащая результат запроса
        private String jsonInput = null;
        public String JsonInput
        {
            get { return this.jsonInput; }
        }

        public PixabayImages(Form2 form)
        {
            form2 = form;
        }


        public async Task<String> getResponseFromSite(String req)
        {
            
            

            List<String> lnks = new List<String>();
            WebRequest request = getRequest(req);
            
            try
            {
                Task<WebResponse> wResponseAsync = request.GetResponseAsync();

                form2.pictureBox.Load(Directory.GetCurrentDirectory() + @"\ProgramsFiles\wait.gif");
                //Ожидаем завершения асинхронной задачи.
                WebResponse wResponse = await wResponseAsync;              

                using (StreamReader sr = new StreamReader(wResponse.GetResponseStream()))
                {
                    jsonInput = sr.ReadToEnd();
                    sr.Close();
                    wResponse.Close();
                }


            }
            catch (WebException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Data);
                this.form2.exceptionLabel.Text = "Проблема с соединением!";
            }
            catch (Exception e)
            {

            }

            return jsonInput;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="jsonParam">
        ///   Параметры pixabay
        ///     pageURL
        ///     previewURL
        ///     webformatURL
        ///     userImageURL
        /// </param>
        public async Task getLinks(String request, String jsonParam)
        {
            List<String> links = new List<String>();
            Task<String> jsonTask = getResponseFromSite(request);
            String test =  await jsonTask;

            if (test != null)
            {
                getAllLinks(test, jsonParam, links);
                this.links = links;
            }
            else
               throw new Exception("Exception in getResponseFromSite");

        }

        public void saveJsonToFile(String fileName, String jsonString, String folder)
        {
            try
            {
                int index = fileName.IndexOf(".");
                String subName = fileName.Substring(index, fileName.Length - index - 1);

                if (!(subName == "json"))
                    fileName = fileName + ".json";

                using (StreamWriter swr = new StreamWriter(folder + @"\" + fileName))
                {
                    swr.Write(jsonString);
                    swr.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Невозможно записать файл!");
            }
        }

        public void saveLinksToFile(String fileName, List<String> links, String folder)
        {
            int index = fileName.IndexOf(".");
            String subName = fileName.Substring(index, fileName.Length - index - 1);

            if (!(subName == "txt"))
                fileName = fileName + ".txt";

            using (StreamWriter swr = new StreamWriter(folder + @"\" + fileName))
            {
                foreach (String lnk in links)
                {
                    swr.WriteLine(lnk);                   
                }

                swr.Close();
            }
        }

        private WebRequest getRequest(String request)
        {
            WebRequest wr = WebRequest.Create("https://pixabay.com/api/?key=" + pixabayKey +
                                                              "&q=" + request + "&image_type=photo");
            wr.Timeout = 2000;
            return wr;
        }      
     
        private void getAllLinks(String str, String param, List<String> links)
        {
            int index = 0;

            while ((index = str.IndexOf(param, index)) > -1)
            {
                String temp1 = str.Substring(index, 10);
                int tmpIndex = str.IndexOf("\"", index + param.Length + 3);
                String lnk = str.Substring(index + param.Length + 3, tmpIndex - (index + param.Length + 3));
                index = tmpIndex + 1;
                links.Add(lnk);

            }
        }


    }
}
