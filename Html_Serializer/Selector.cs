using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Html_Serializer
{
    public class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; }
        public Selector Parent { get; set; }
        public Selector Child { get; set; }

        public Selector() { }
        public Selector(string name, string data, Selector parent)
        {
            TagName = name;
            if (data.Contains("#"))
            {
                Id = data.Substring(data.IndexOf('#') + 1, data.IndexOf('.'));
            }
            if (data.Contains("."))
            {
                Classes = data.Substring(data.IndexOf('.') + 1).Split('.').ToList();
            }
            Parent = parent;
        }
        public static Selector ConvertStringToSelect(string str)
        {
            string name;
            var listSelect = str.Split(' ').ToList();
            //עבודה על הראשון-שורש
            if (listSelect[0].Contains("#"))
            { 
                name = listSelect[0].Substring(0, listSelect[0].IndexOf('#'));
            }
            else
            {
                if (listSelect[0].Contains("."))
                { 
                    name = listSelect[0].Substring(0, listSelect[0].IndexOf('.'));
                }
                else { name = listSelect[0].Substring(0); }
            }
            Selector root = new Selector();
            Selector temp = new Selector();
            if (HtmlHelper.Instance.ListHtmlTags.Contains(name) || HtmlHelper.Instance.ListHtmlTags.Contains(name))
            {
                root = new Selector(name, listSelect[0], null);
            }
         
            listSelect = listSelect.Skip(1).ToList();
            Selector tempP = root;
            foreach (var item in listSelect)
            {

                if (item.Contains("#"))
                { 
                    name = item.Substring(0, item.IndexOf('#'));
                }
                else
                {
                    if (item.Contains("."))
                    {
                        name = item.Substring(0, item.IndexOf('.'));
                    }
                    else { 
                        name = item.Substring(0);
                    }
                }
                if (HtmlHelper.Instance.ListHtmlTags.Contains(name) || HtmlHelper.Instance.ListHtmlTags.Contains(name))
                {
                    temp = new Selector(name, item, tempP);
                    tempP.Child = temp;
                    tempP = temp;
                }

            }

            return root;
        }
    }
}
