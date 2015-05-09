using System.Xml.Serialization;

namespace Example.Xml.Report
{
    public class Tree
    {
        public string Date { get; set; }

        [XmlElement("Account")]
        public Account[] AccountsArray { get; set; }
    }
}