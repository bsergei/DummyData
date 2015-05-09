using System.Xml.Serialization;

namespace Example.Xml.Report
{
    [XmlInclude(typeof(AccountBasic))]
    [XmlInclude(typeof(AccountDelta))]
    [XmlInclude(typeof(AccountBenchmark))]
    public abstract class Account
    {
        public string AcctName { get; set; }

        public Root Root { get; set; }
    }

    public class AccountBasic : Account
    {
    }

    public class AccountDelta : Account
    {
        public string Delta { get; set; }
    }

    public class AccountBenchmark : Account
    {
        public string Benchmark { get; set; }
    }
}