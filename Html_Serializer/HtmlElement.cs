using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Html_Serializer
{
    internal class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Childrens { get; set; }

        public HtmlElement(string name, string atribute, HtmlElement parent)
        {
            Name = name;
            Attributes = new List<string>();
            if (atribute != null)
            {
                var list = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(atribute).ToList();
                foreach (var item in list)
                {
                    name = item.Value.Substring(0, item.Value.IndexOf('='));
                    var at = item.Value.Substring( item.Value.IndexOf('\"')+1);
                    at = at.Substring(0, at.LastIndexOf("\""));
                    if (name == "id")
                        Id = item.Value.Substring(item.Value.LastIndexOf('=') + 1);
                    else
                    {
                        if (name == "class")
                            Classes = at.Split(' ').ToList();
                        else
                        {
                            Attributes.Add(item.Value);
                        }
                    }
                }
            }
            Parent = parent;
            Childrens = new List<HtmlElement>();
        }
        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            queue.Enqueue(this);
            while (queue.Count > 0)
            {
                HtmlElement element = queue.Dequeue();
                yield return element;
                List<HtmlElement> children = element.Childrens;
                foreach (HtmlElement child in children)
                {
                    queue.Enqueue(child);
                }

            }

        }
        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement parent = this.Parent;
            while (parent != null)
            {
                yield return parent;
                parent = parent.Parent;
            }

        }

        //לפונקצית הרחבה- השואה בין סלקטור לאלמנט
        public override bool Equals(object? obj)
        {
            if (obj is Selector)
            {
                Selector other = (Selector)obj;
                if ((other.Id == null || Id == other.Id) && (other.TagName == null || other.TagName == Name))
                {
                    if ((this.Classes != null && other.Classes != null) || (this.Classes == null && other.Classes == null))
                    {
                        if(other.Classes != null)
                        foreach (var item in other.Classes)
                        {
                            if (this.Classes != null)
                                if (this.Classes.Contains(item) == false)
                                    return false;

                        }
                        return true;
                    }
                }
            }
            
            return false;
        }

        public HashSet<HtmlElement> SelectionQuery(Selector selector)
        {
            HashSet<HtmlElement> result = new HashSet<HtmlElement>();
            result = this.ExpansionFanc(selector);
            return result;
        }
        public override string ToString()
        {
            string s = $"name: {Name}, ";
            if (Id != null)
            {
                s += $"Id :{Id},";
            }
            if (this.Classes != null)
            {
                s += "Class :";
                foreach (var item in Classes)
                {
                    s += $"{item},";
                }
            }

            if (this.Attributes != null)
            {
                s += "Attributes :";
                foreach (var item in Attributes)
                {
                    s += $"{item},";
                }
            }
            return s;
        }
    }
}
