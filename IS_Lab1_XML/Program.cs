using IS_Lab1_XML;

var xmlpath = Path.Combine("Assets", "data.xml");

// odczyt danych z wykorzystaniem DOM
Console.WriteLine("XML loaded by DOM Approach");
XMLReadWithDOMApproach.Read(xmlpath);
XMLReadWithDOMApproach.ReadAll(xmlpath);
XMLReadWithDOMApproach.ReadTabletkiKrem(xmlpath);
XMLReadWithDOMApproach.ReadMostKremy(xmlpath);

// odczyt danych z wykorzystaniem SAX
Console.WriteLine("XML loaded by SAX Approach");
XMLReadWithSAXApproach.Read(xmlpath);
XMLReadWithSAXApproach.ReadAll(xmlpath);
XMLReadWithSAXApproach.ReadTabletkiKrem(xmlpath);
XMLReadWithSAXApproach.ReadMostKremy(xmlpath);

// odczyt danych z wykorzystaniem XPath i DOM
Console.WriteLine("XML loaded with XPath");
XMLReadWithXLSTDOM.Read(xmlpath);
XMLReadWithXLSTDOM.ReadAll(xmlpath);
XMLReadWithXLSTDOM.ReadTabletkiKrem(xmlpath);
XMLReadWithXLSTDOM.ReadMostKremy(xmlpath);

Console.ReadLine();