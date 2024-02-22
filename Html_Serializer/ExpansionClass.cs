using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Html_Serializer
{
    static class ExpansionClass
    {
        public static void RecExpansionFanc(this HtmlElement htmlElement,Selector selector, HashSet<HtmlElement> collec)
        {
            foreach (var item in htmlElement.Descendants()) 
            {
                if (item.Equals(selector))
                {
                    if (selector.Child == null)
                    {
                        collec.Add(item);
                    }
                    else
                    {
                        item.RecExpansionFanc(selector.Child, collec);
                    }
                }
            }
           
        }

        public static HashSet<HtmlElement> ExpansionFanc(this HtmlElement htmlElement, Selector selector)
        {
           HashSet<HtmlElement> collec = new HashSet<HtmlElement>();
            if(htmlElement.Equals(selector))
                htmlElement.RecExpansionFanc(selector.Child, collec);
            htmlElement.RecExpansionFanc(selector, collec);
            return collec;  
        }
    }
}
