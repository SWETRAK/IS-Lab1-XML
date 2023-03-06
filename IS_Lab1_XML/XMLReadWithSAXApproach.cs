using System.Xml;
using IS_Lab1_XML.Models;

namespace IS_Lab1_XML;

public static class XMLReadWithSAXApproach
{

    public static void Read(string filepath)
    {
        var settings = new XmlReaderSettings
        {
            IgnoreComments = true,
            IgnoreWhitespace = true,
            IgnoreProcessingInstructions = true
        };

        var reader = XmlReader.Create(filepath, settings);

        var count = 0;
        reader.MoveToContent();

        while (reader.Read())
        {
            if (reader is { NodeType: XmlNodeType.Element, Name: "produktLeczniczy" })
            {
                var postac = reader.GetAttribute("postac");
                var sc = reader.GetAttribute("nazwaPowszechnieStosowana");
                if (postac == "Krem" && sc == "Mometasoni furoas")
                    count++;
            }
        }

        Console.WriteLine(
            "Liczba produktów leczniczych w postaci kremu, których jedyną substancją czynną jest Mometasoni furoas {0} ",
            count);
    }

    public static void ReadAll(string filepath)
    {
        var settings = new XmlReaderSettings
        {
            IgnoreComments = true,
            IgnoreWhitespace = true,
            IgnoreProcessingInstructions = true
        };
        var reader = XmlReader.Create(filepath, settings);
        reader.MoveToContent();

        var specyfiki = new List<Specyfik>();

        while (reader.Read())
        {
            if (reader is { NodeType: XmlNodeType.Element, Name: "produktLeczniczy" })
            {
                var postac = reader.GetAttribute("postac");
                var sc = reader.GetAttribute("nazwaPowszechnieStosowana");
                if (postac is not null && sc is not null)
                {
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
        }

        Console.WriteLine("Specyfiki w które wsystępują w różnych formach: {0}", specyfiki.Where(x => x.Postaci.Count > 1).ToList().Count);
    }

    public static void ReadTabletkiKrem(string filepath)
    {
        var settings = new XmlReaderSettings
        {
            IgnoreComments = true,
            IgnoreWhitespace = true,
            IgnoreProcessingInstructions = true
        };

        var reader = XmlReader.Create(filepath, settings);
        reader.MoveToContent();

        var podmioty = new List<Producent>();

        while (reader.Read())
        {
            if (reader is { NodeType: XmlNodeType.Element, Name: "produktLeczniczy" })
            {
                var podmiot = reader.GetAttribute("podmiotOdpowiedzialny");
                var postac = reader.GetAttribute("postac");

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
        }

        podmioty.Sort((x1, x2) => -SortingFunction.Sort(x1.Tabletka, x2.Tabletka));
        var tabletka = podmioty[0];
        podmioty.Sort((x1, x2) => -SortingFunction.Sort(x1.Krem, x2.Krem));
        var krem = podmioty[0];

        Console.WriteLine(
            $"Firma robiaca najwiecej leków w tabletkach to {tabletka.Nazwa} : {tabletka.Tabletka}. \nFirma robiaca najwiecej leków w kremie to {krem.Nazwa} : {krem.Krem}");
    }

    public static void ReadMostKremy(string filepath)
    {
        var settings = new XmlReaderSettings
        {
            IgnoreComments = true,
            IgnoreProcessingInstructions = true,
            IgnoreWhitespace = true,
        };

        var reader = XmlReader.Create(filepath, settings);

        var statPodmiotu = new Dictionary<string, int>();

        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element && reader.Name
                == "produktLeczniczy")
            {
                var postac = reader.GetAttribute("postac");
                var podmiot = reader.GetAttribute("podmiotOdpowiedzialny");
                if (!statPodmiotu.ContainsKey(podmiot)) statPodmiotu[podmiot] = 0;
                if (postac == "Krem") statPodmiotu[podmiot]++;
            }
        }

        var maxKremow = statPodmiotu.OrderByDescending(x => x.Value);

        Console.WriteLine("3 największych sprzedawców kremów: ");
        for (var i = 0; i < 3; i++)
        {
            Console.WriteLine(i + ". " + maxKremow.ElementAt(i).Key + " " + maxKremow.ElementAt(i).Value);
        }
    }
}
