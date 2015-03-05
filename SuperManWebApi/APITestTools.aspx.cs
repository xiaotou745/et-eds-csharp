using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.WebPages;
using Newtonsoft.Json;

namespace SuperManWebApi
{
    public partial class APITestTools : System.Web.UI.Page
    {
        public string Results = "";
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string url = TextBox1.Text;
           
            if (RadioButtonList1.SelectedValue=="1")
            {
                Results = new HttpClient().GetAsync(url).Result.Content.ReadAsStringAsync().Result;
            }
            else
            {
                string parsJson = TextBox2.Text;
                if (!parsJson.IsEmpty())
                {
                   var  pars = JsonConvert.DeserializeObject(parsJson);
                   Results = new HttpClient().PostAsJsonAsync(url, pars).Result.Content.ReadAsStringAsync().Result;
                }  
            }
           
        }
    }
}