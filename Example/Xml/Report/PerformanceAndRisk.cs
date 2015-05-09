using System.Xml.Serialization;

namespace Example.Xml.Report
{
    public class PerformanceAndRisk
    {
        [XmlElement("period")]
        public string Period { get; set; }

        [XmlElement("acctPct")]
        public double AccountPct { get; set; }

        [XmlElement("bmPct")]
        public double BenchmarkPct { get; set; }
    }
}