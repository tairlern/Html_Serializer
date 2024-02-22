using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Html_Serializer
{
    public class HtmlHelper
    {
        private readonly static HtmlHelper _innerHtml=new HtmlHelper();
        public static HtmlHelper Instance => _innerHtml;
        public string[] ListHtmlTags { get; set; }
        public string[] ListHtmlVoidTags { get; set; }
        
       
        private HtmlHelper()
        {
           var tags= File.ReadAllText("seed/HtmlTags.json");
            var voidTags = File.ReadAllText("seed/HtmlVoidTags.json");
            ListHtmlTags = JsonSerializer.Deserialize<string[]>(tags);
            ListHtmlVoidTags= JsonSerializer.Deserialize<string[]>(voidTags);

        }
    }
   
}
