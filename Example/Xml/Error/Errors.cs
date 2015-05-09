using System.Xml.Serialization;

namespace Example.Xml.Error
{
    [XmlRoot("ERRORS")]
    public class Errors
    {
        [XmlElement("ERROR")]
        public Error[] ErrorsArray { get; set; }
    }
}