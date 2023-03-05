using IS_Lab1_XML;

string xmlpath = "/Users/kamilpietrak/Documents/Dotnet Projects/IS_Lab1_XML/IS_Lab1_XML/Assets/data.xml";
// odczyt danych z wykorzystaniem DOM

Console.WriteLine("XML loaded by DOM Approach");
XMLReadWithDOMApproach.Read(xmlpath);
XMLReadWithDOMApproach.ReadAll(xmlpath);
XMLReadWithDOMApproach.ReadTabletkiKrem(xmlpath);

// odczyt danych z wykorzystaniem SAX
Console.WriteLine("XML loaded by SAX Approach");
XMLReadWithSAXApproach.Read(xmlpath);
XMLReadWithSAXApproach.ReadAll(xmlpath);
XMLReadWithSAXApproach.ReadTabletkiKrem(xmlpath);

// odczyt danych z wykorzystaniem XPath i DOM
Console.WriteLine("XML loaded with XPath");
XMLReadWithXLSTDOM.Read(xmlpath);
XMLReadWithXLSTDOM.ReadTabletkiKrem(xmlpath);

Console.ReadLine();