using DataGenerator;
using DataGenerator.Core;
using Example.Xml.Report;

namespace Example.DataGen
{
    static class Report1
    {
        public static Tree Create(string asOfDate)
        {
            return new Tree
            {
                Date = asOfDate,
                AccountsArray = new Account[]
                        {
                            new AccountBasic()
                                {
                                    AcctName = "Account",
                                    Root = GetRoot(asOfDate, "Acct")
                                },
                            new AccountDelta()
                                {
                                    Delta = "Report2_Delta",
                                    Root = GetRoot(asOfDate, "_Delta"),

                                },
                            new AccountBenchmark()
                                {
                                    Benchmark = "BenchMark1",
                                    Root = GetRoot(asOfDate, "_BenchMark"),
                                },
                        }
            };
        }

        private static Root GetRoot(string asOfDate, string tag)
        {
            int randomSeed = (asOfDate + tag + "report1").ToInt32();

            return
                new RootOas()
                    .Generate(
                        root =>
                        root
                            .Property(_ => _.Name).WithStaticValue("serviceTree")
                            .Property(_ => _.Counterparty).WithGeneratedValue(_ => new RandomListValueGenerator<string>(randomSeed, DataLists.Country))
                            .Property(_ => _.Level)
                                .WithNewArray(
                                    InstanceFactory.Create<LevelStrategy>(),
                                    level1 =>
                                    level1
                                        .Property(_ => _.Name)
                                            .WithGeneratedValue(_ => new ListValueGenerator<string>(DataLists.Markets))
                                        .Property(_ => _.CreditRating)
                                            .WithGeneratedValue(_ => new RandomListValueGenerator<string>(randomSeed, DataLists.CreditRatingsAll, DataLists.CreditRatingsAllWeights))
                                        .Property(_ => _.Period)
                                            .WithGeneratedValue(_ => new RandomListValueGenerator<string>(randomSeed, DataLists.Periods))
                                        .Property(_ => _.MV_Pct)
                                            .WithGeneratedValue(_ => (double)_, _ => new PercentageValueGenerator(randomSeed, DataLists.Markets.Length))
                                )
                    );
        }
    }
}
