// See https://aka.ms/new-console-template for more information

using Html_Serializer;
using System.Text.RegularExpressions;
using System.Xml;

async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}
HtmlElement Serialize(string clinHtml)
{
    var htmlLins = new Regex("<(.*?)>").Split(clinHtml).Where(s => (s.Length > 0 && s[0] != ' ')).ToList();
    var name = htmlLins[1].Split(' ')[0];
    //האוביקט הראשון 
    HtmlElement root = new HtmlElement(name, null, null);
    htmlLins = htmlLins.Skip(2).ToList();
    Stack<HtmlElement> stack = new Stack<HtmlElement>();
    stack.Push(root);
    foreach (var item in htmlLins)
    {
        name = item.Split(' ')[0];

        if (name == "/html")
        {
            break;
        }
        //כדי לבדוק תגית סוגרת
        string closeName = '/' + stack.Peek().Name;
        if (name != closeName)
        {
           HtmlElement temp = stack.Pop();
            //השם לא מופיע כתגית כלומר, תוכן של אלמנט
            if (!HtmlHelper.Instance.ListHtmlTags.Contains(name) && !HtmlHelper.Instance.ListHtmlVoidTags.Contains(name))
            {
                temp.InnerHtml = item;
                stack.Push(temp);

            }
            //מתחיל ב סלש ונימצא ברשימת האלמנטים ללא סגירה
            else if (HtmlHelper.Instance.ListHtmlVoidTags.Contains(name))
            {
                temp.Childrens.Add(new HtmlElement(name, item, temp));
                stack.Push(temp);
               
            } 
            //תגית חדשה 
            else
            {
                stack.Push(temp);
                stack.Push(new HtmlElement(name, item, temp));
            }

        }
        else
        {
            HtmlElement child = stack.Pop();
            HtmlElement parent = stack.Pop();
            parent.Childrens.Add(child);
            stack.Push(parent);
        }

    }
return root;
}



var html = await Load("https://learn.malkabruk.co.il/");
var clinHtml=new Regex("(\\s)").Replace(html," ");
HtmlElement tree=Serialize(clinHtml);


Selector s= Selector.ConvertStringToSelect("link");
var res=tree.SelectionQuery(s);
Console.WriteLine(res.Count());
res.ToList().ForEach(e => Console.WriteLine(e.ToString()) );


/*
 * a.button
 * div.home-hero div.home-hero1 div.home-container1
 * div.home-hero div.home-container1
 * 
*/






