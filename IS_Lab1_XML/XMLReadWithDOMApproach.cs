using System.Xml;
using IS_Lab1_XML.Models;

namespace IS_Lab1_XML;

public static class XMLReadWithDOMApproach
{
    
    public static void Read(string filepath)
    {
        // odczyt zawartości dokumentu
        XmlDocument doc = new XmlDocument();
        doc.Load(filepath);
        string postac;
        string sc;
        int count = 0;
        var drugs = doc.GetElementsByTagName("produktLeczniczy");
        Dictionary<string, List<string>> iloscPostaci = new Dictionary<string, List<string>>();
        foreach (XmlNode d in drugs)
        {
            postac = d.Attributes.GetNamedItem("postac").Value;
            sc = d.Attributes.GetNamedItem("nazwaPowszechnieStosowana").Value;
            if (postac == "Krem" && sc == "Mometasoni furoas")
                count++;
            if (iloscPostaci.ContainsKey(sc) && !iloscPostaci[sc].Contains(postac)) iloscPostaci[sc].Add(postac);
            else
            {
                iloscPostaci[sc] = new List<string>();
                iloscPostaci[sc].Add(postac);
            }
        }

        int kilkaPostaci = 0;
        foreach(var i in iloscPostaci)
        {
            if(i.Value.Count > 1) kilkaPostaci++;
        }
        Console.WriteLine("Liczba produktów leczniczych w postaci kremu, których jedyną substancją czynną jest Mometasoni furoas {0}", count);
        Console.WriteLine("Preparatow o tej samej nazwie pod różną postacią jest: " + kilkaPostaci);
    }
    public static void Read1(string filepath)
    {
        var doc = new XmlDocument();
        doc.Load(filepath);
        string postac;
        string sc;
        var count = 0;
        var drugs = doc.GetElementsByTagName("produktLeczniczy");
        foreach (XmlNode d in drugs)
        {
            postac = d.Attributes.GetNamedItem("postac").Value;
            sc =
                d.Attributes.GetNamedItem("nazwaPowszechnieStosowana").Value;
            if (postac == "Krem" && sc == "Mometasoni furoas")
                count++;
        }

        Console.WriteLine(
            "Liczba produktów leczniczych w postaci kremu, których jedyną substancją czynną jest Mometasoni furoas {0}",
            count);

    }

    public static void ReadAll(string filepath)
    {
        var doc = new XmlDocument();
        doc.Load(filepath);
        var specyfiki = new List<Specyfik>();
        var drugs = doc.GetElementsByTagName("produktLeczniczy");
        foreach (XmlNode d in drugs)
        {
            var value = d.Attributes.GetNamedItem("postac").Value;
            var sc = d.Attributes.GetNamedItem("nazwaPowszechnieStosowana")?.Value;
            if (value != null && sc is not null)
            {
                var postac = value.ToLower();
                var sclow = sc.ToLower();

                var element = specyfiki.FirstOrDefault(x => x.Name == sc);

                if (element is null)
                {
                    specyfiki.Add(new Specyfik
                    {
                        Name = sc,
                        Postaci = new List<Postac>()
                        {
                            new()
                            {
                                Nazwa = postac,
                                Ilosc = 1,
                            }
                        }
                    });
                }
                else
                {
                    var postaci = element.Postaci.FirstOrDefault(x => x.Nazwa == postac);
                    if (postaci is null)
                    {
                        element.Postaci.Add(new Postac
                        {
                            Nazwa = postac,
                            Ilosc = 1
                        });
                    }
                    else
                    {
                        postaci.Ilosc++;
                    }
                }
            }
        }

        Console.WriteLine(specyfiki.Count);
        Console.WriteLine(specyfiki.Where(x => x.Postaci.Count > 1).ToList().Count);
    }

    public static void ReadTabletkiKrem(string filepath)
    {
        var doc = new XmlDocument();
        doc.Load(filepath);
        var drugs = doc.GetElementsByTagName("produktLeczniczy");
        var podmioty = new List<Producent>();

        foreach (XmlNode d in drugs)
        {

            var podmiot = d.Attributes.GetNamedItem("podmiotOdpowiedzialny").Value;
            var postac = d.Attributes.GetNamedItem("postac").Value;

            var element = podmioty.FirstOrDefault(x => x.Nazwa == podmiot);

            if (element is null)
            {
                if (postac == "Krem")
                {
                    podmioty.Add(new Producent
                    {
                        Nazwa = podmiot,
                        Krem = 1,
                        Tabletka = 0,
                    });
                }
                else if (postac == "Tabletki")
                {
                    podmioty.Add(new Producent
                    {
                        Nazwa = podmiot,
                        Krem = 0,
                        Tabletka = 1
                    });
                }
            }
            else
            {
                if (postac == "Krem")
                {
                    element.Krem++;
                }
                else if (postac == "Tabletki")
                {
                    element.Tabletka++;
                }
            }
        }

        podmioty.Sort((x1, x2) => -SortingFunction.Sort(x1.Tabletka, x2.Tabletka));
        var tabletka = podmioty[0];
        podmioty.Sort((x1, x2) => -SortingFunction.Sort(x1.Krem, x2.Krem));
        var krem = podmioty[0];

        Console.WriteLine(
            $"Firma robiaca najwiecej leków w tabletkach to {tabletka.Nazwa} : {tabletka.Tabletka}. \nFirma robiaca najwiecej leków w kremie to {krem.Nazwa} : {krem.Krem}");
    }
}

