using System.Xml.Serialization;

namespace Example.Xml.Report
{
    [XmlInclude(typeof(RootCredit))]
    [XmlInclude(typeof(RootOas))]
    [XmlInclude(typeof(RootBasic))]
    public abstract class Root
    {
        public string Name { get; set; }
        public double MVPct { get; set; }
        public string Counterparty { get; set; }
        
        [XmlElement]
        public Level[] Level { get; set; }
    }

    public class RootBasic : Root
    {
    }

    public class RootCredit : Root
    {
        public string CreditRatingHigh { get; set; }
        public string CreditRatingLow { get; set; }
    }

    public class RootOas : Root
    {
        public string OAS { get; set; }
    }
}