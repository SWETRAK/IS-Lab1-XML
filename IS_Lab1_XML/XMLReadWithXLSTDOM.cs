using System.Xml;
using System.Xml.XPath;

namespace IS_Lab1_XML;

public static class XMLReadWithXLSTDOM
{
    public static void Read(string filepath)
    {
        var document = new XPathDocument(filepath);
        var navigator = document.CreateNavigator();
        if (navigator == null) return;
        var manager = new XmlNamespaceManager(navigator.NameTable);
        manager.AddNamespace("x", "http://rejestrymedyczne.ezdrowie.gov.pl/rpl/eksport-danych-v1.0");
        var query =
            navigator.Compile("/x:produktyLecznicze/x:produktLeczniczy[@postac='Krem' and @nazwaPowszechnieStosowana='Mometasoni furoas']");
        query.SetContext(manager);
        var count = navigator.Select(query).Count;
        Console.WriteLine("Liczba produktów leczniczych w postacikremu, których jedyną substancją czynną jest Mometasoni furoas {0}", count );
    }

    public static void ReadAll(string filepath)
    {
        var document = new XPathDocument(filepath);
        var navigator = document.CreateNavigator();
        if (navigator == null) return;
        var manager = new XmlNamespaceManager(navigator.NameTable);
        manager.AddNamespace("x", "http://rejestrymedyczne.ezdrowie.gov.pl/rpl/eksport-danych-v1.0");
        var query =
            navigator.Compile("/x:produktyLecznicze/x:produktLeczniczy[@postac='Krem' and @nazwaPowszechnieStosowana='Mometasoni furoas']");
        query.SetContext(manager);
        var count = navigator.Select(query).Count;
        Console.WriteLine("Liczba produktów leczniczych w postacikremu, których jedyną substancją czynną jest Mometasoni furoas {0}", count );
    }


    public static void ReadTabletkiKrem(string filepath)
    {
        var document = new XPathDocument(filepath);
        var navigator = document.CreateNavigator();
        
        var manager = new XmlNamespaceManager(navigator.NameTable);
        manager.AddNamespace("x", "http://rejestrymedyczne.ezdrowie.gov.pl/rpl/eksport-danych-v1.0");
        
        var queryKremy =
            navigator.Compile("/x:produktyLecznicze/x:produktLeczniczy[@postac='Krem']/@podmiotOdpowiedzialny");
        queryKremy.SetContext(manager);
        
        var queryTabletki =
            navigator.Compile("/x:produktyLecznicze/x:produktLeczniczy[@postac='Tabletki']/@podmiotOdpowiedzialny");
        queryTabletki.SetContext(manager);
        
        var enumeratorKremow = navigator.Select(queryKremy).GetEnumerator();
        var enumeratorTabletek = navigator.Select(queryTabletki).GetEnumerator();
        
        var iloscKremow = new Dictionary<string, int>();
        var iloscTabletek = new Dictionary<string, int>();
        
        while (enumeratorKremow.MoveNext())
        {
            var podmiot = enumeratorKremow.Current.ToString();
            if (!iloscKremow.ContainsKey(podmiot)) iloscKremow[podmiot] = 1;
            else iloscKremow[podmiot]++;
        }
        while (enumeratorTabletek.MoveNext())
        {
            var podmiot = enumeratorTabletek.Current.ToString();
            if (!iloscTabletek.ContainsKey(podmiot)) iloscTabletek[podmiot] = 1;
            else iloscTabletek[podmiot]++;
        }

        var maxKremow= iloscKremow.OrderByDescending(x => x.Value).First();
        var maxTabletki= iloscTabletek.OrderByDescending(x => x.Value).First();
        
        Console.WriteLine($"Firma robiaca najwiecej leków w tabletkach to {maxTabletki.Key} : {maxTabletki.Value}");
        Console.WriteLine($"Firma robiaca najwiecej leków w kremie to {maxKremow.Key} : {maxKremow.Value}.");
    }
}