using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SearchWord
{
    public partial class WebApi : Form
    {
        string dataParams = "source=ko&target=en&text=";
        string clientId = "kfCa8Gi65r40nhIdhIzj";
        string clientSecret = "BgZimo4vpz";

        int count;

        public WebApi()
        {
            InitializeComponent();
            KeyPreview = true;
        }

        private void changeLanguage_Click(object sender, EventArgs e)
        {
            if(count == 0)
            {
                dataParams = "source=en&target=ko&text=";
                Point temp = koreaText.Location;
                koreaText.Location = englishText.Location;
                englishText.Location = temp;
                count++;
                Console.WriteLine(count);
            }
            else
            {
                dataParams = "source=ko&target=en&text=";
                Point temp = englishText.Location;
                englishText.Location = koreaText.Location;
                koreaText.Location = temp;
                count = 0;
            }
        }

        private void translate_Click(object sender, EventArgs e)
        {
            Translate();
        }

        private void WebApi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Translate(); 
            }
        }

        public void Translate()
        {
            Boolean checkInput = string.IsNullOrEmpty(InputTarget.Text);
            if (checkInput)
            {
                return;
            }
            string url = "https://openapi.naver.com/v1/papago/n2mt";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("X-Naver-Client-Id", clientId);
            request.Headers.Add("X-Naver-Client-Secret", clientSecret);
            request.Method = "POST";
            string query = InputTarget.Text;
            byte[] byteDataParams = Encoding.UTF8.GetBytes(dataParams + query);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteDataParams.Length;
            Stream st = request.GetRequestStream();
            st.Write(byteDataParams, 0, byteDataParams.Length);
            st.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            string text = reader.ReadToEnd();
            stream.Close();
            response.Close();
            reader.Close();
            JObject jObject = JObject.Parse(text);
            InputResult.Text = jObject["message"]["result"]["translatedText"].ToString();
        }
    }
}
