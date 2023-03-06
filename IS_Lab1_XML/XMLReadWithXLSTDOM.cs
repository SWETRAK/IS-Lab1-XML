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
            navigator.Compile(
                "/x:produktyLecznicze/x:produktLeczniczy[@postac='Krem' and @nazwaPowszechnieStosowana='Mometasoni furoas']");
        query.SetContext(manager);
        var count = navigator.Select(query).Count;
        Console.WriteLine(
            "Liczba produktów leczniczych w postacikremu, których jedyną substancją czynną jest Mometasoni furoas {0}",
            count);
    }

    public static void ReadAll(string filepath)
    {
        var document = new XPathDocument(filepath);
        var navigator = document.CreateNavigator();

        var manager = new XmlNamespaceManager(navigator.NameTable);
        manager.AddNamespace("x", "http://rejestrymedyczne.ezdrowie.gov.pl/rpl/eksport-danych-v1.0");

        var queryNazwy =
            navigator.Compile("/x:produktyLecznicze/x:produktLeczniczy/@nazwaPowszechnieStosowana");

        queryNazwy.SetContext(manager);

        var enumerator = navigator.Select(queryNazwy).GetEnumerator();
        var kilkaPostaci = 0;

        var nazwy = new List<string>();

        while (enumerator.MoveNext())
        {
            var nazwa = enumerator.Current.ToString();

            if (nazwy.Contains(nazwa)) continue;

            nazwy.Add(nazwa);
            if (nazwa.Contains('\'')) nazwa = nazwa.Replace("'", "&#39;");

            var queryPostaci =
                navigator.Compile(
                    $"/x:produktyLecznicze/x:produktLeczniczy[@nazwaPowszechnieStosowana='{nazwa}']/@postac");
            queryPostaci.SetContext(manager);

            var e = navigator.Select(queryPostaci).GetEnumerator();
            var postaciPreparatu = new List<string>();
            while (e.MoveNext())
            {
                if (!postaciPreparatu.Contains(e.Current.ToString())) postaciPreparatu.Add(e.Current.ToString());
            }

            if (postaciPreparatu.Count >= 2) kilkaPostaci++;
        }

        Console.WriteLine("Specyfiki w które wsystępują w różnych formach: {0}", kilkaPostaci);
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

        var maxKremow = iloscKremow.OrderByDescending(x => x.Value).First();
        var maxTabletki = iloscTabletek.OrderByDescending(x => x.Value).First();

        Console.WriteLine($"Firma robiaca najwiecej leków w tabletkach to {maxTabletki.Key} : {maxTabletki.Value}");
        Console.WriteLine($"Firma robiaca najwiecej leków w kremie to {maxKremow.Key} : {maxKremow.Value}.");
    }

    public static void ReadMostKremy(string filepath)
    {
        var document = new XPathDocument(filepath);
        var navigator = document.CreateNavigator();

        var manager = new XmlNamespaceManager(navigator.NameTable);
        manager.AddNamespace("x", "http://rejestrymedyczne.ezdrowie.gov.pl/rpl/eksport-danych-v1.0");

        var queryKremy =
            navigator.Compile("/x:produktyLecznicze/x:produktLeczniczy[@postac='Krem']/@podmiotOdpowiedzialny");
        queryKremy.SetContext(manager);

        var enumeratorKremow = navigator.Select(queryKremy).GetEnumerator()
            ;
        var iloscKremow = new Dictionary<string, int>();

        while (enumeratorKremow.MoveNext())
        {
            string podmiot = enumeratorKremow.Current.ToString();
            if (!iloscKremow.ContainsKey(podmiot)) iloscKremow[podmiot] = 1;
            else iloscKremow[podmiot]++;
        }

        var maxKremow = iloscKremow.OrderByDescending(x => x.Value);

        Console.WriteLine("3 największych sprzedawców kremów: ");
        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine(i + ". " + maxKremow.ElementAt(i).Key + " " + maxKremow.ElementAt(i).Value);
        }
    }
}