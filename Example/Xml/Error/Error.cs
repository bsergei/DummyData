using System.Xml.Serialization;

namespace Example.Xml.Error
{
    public class Error
    {
        [XmlAttribute]
        public string Type { get; set; }
    }
}