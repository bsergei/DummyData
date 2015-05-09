using System.Xml.Serialization;

namespace Example.Xml.Report
{
    [XmlInclude(typeof(LevelSector))]
    [XmlInclude(typeof(LevelQuality))]
    [XmlInclude(typeof(LevelStrategy))]
    public abstract class Level
    {
        public string Leg_Name { get; set; }
        public string Name { get; set; }
        public string CreditRating { get; set; }
        public string Period { get; set; }
        public double Exposure { get; set; }
        public double MV_Pct { get; set; }
    }

    public class LevelQuality : Level
    {
        [XmlArray("annualPerformanceRisk")]
        [XmlArrayItem("annualPerformanceRiskData")]
        public PerformanceAndRisk[] Annual { get; set; }
    }

    public class LevelSector : Level
    {
        public string CreditRatingHigh { get; set; }
        public string CreditRatingLow { get; set; }
    }

    public class LevelStrategy : Level
    {
        public string OAS { get; set; }
    }
}