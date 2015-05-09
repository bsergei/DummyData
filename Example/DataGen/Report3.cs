using System;
using DataGenerator;
using DataGenerator.Core;
using Example.Xml.Report;
using Account = Example.Xml.Report.Account;

namespace Example.DataGen
{
    class Report3
    {
        public static Tree Create(string asOfDate)
        {
            return new Tree
            {
                Date = asOfDate,
                AccountsArray = new Account[]
                        {
                            new AccountBasic
                                {
                                    AcctName = "Account",
                                    Root = GetRoot(asOfDate, String.Empty)
                                },
                            new AccountDelta
                                {
                                    Delta = "Delta",
                                    Root = GetRoot(asOfDate, "_Delta"),
                                },
                            new AccountBenchmark
                                {
                                    Benchmark = "BenchMark",
                                    Root = GetRoot(asOfDate, "_BenchMark"),
                                },
                        }
            };
        }

        private static Root GetRoot(string asOfDate, string tag)
        {
            int randomSeed = (asOfDate + tag + "report3").ToInt32();
            var rootBasic = new RootBasic()
                .Generate(
                    root =>
                    root
                        .Property(_ => _.Name).WithStaticValue("serviceTree")
                        .Property(_ => _.MVPct).WithStaticValue(100.0)
                        .Property(_ => _.Counterparty).WithGeneratedValue(_ => new RandomListValueGenerator<string>(randomSeed, DataLists.Country))
                        .Property(_ => _.Level)
                            .WithNewArray(
                                InstanceFactory.Create<LevelQuality>(),
                                level =>
                                level
                                  .Property(_ => _.CreditRating).WithGeneratedValue(_ => new ListValueGenerator<string>(DataLists.CreditRatings))
                                  .Property(_ => _.MV_Pct).WithGeneratedValue(_ => (double)_, _ => new PercentageValueGenerator(randomSeed, DataLists.CreditRatings.Length))
                                  .Property(_ => _.Annual)
                             .WithNewArray(InstanceFactory.Create<PerformanceAndRisk>(),
                                           t =>
                                           t
                                               .Property(_ => _.Period).WithGeneratedValue(_ => new ListValueGenerator<string>(DataLists.AnnualPeriods))
                                               .Property(_ => _.AccountPct).WithGeneratedValue(_ => new FunctionGenerator(DataLists.AnnualPeriods.Length, GetRandomFunc((asOfDate + "AnnualAcctPct" + _.Parent.CurrentInstanceIndex).ToInt32(), AnnualFunc)))
                                               .Property(_ => _.BenchmarkPct).WithGeneratedValue(_ => new FunctionGenerator(DataLists.AnnualPeriods.Length, GetRandomFunc((asOfDate + "AnnualBmPct" + _.Parent.CurrentInstanceIndex).ToInt32(), AnnualFunc)))
                             ))
                        );

            return rootBasic;
        }

        private static Func<int, int, double> GetRandomFunc(int randomSeed, Func<int, int, double> stableFunc)
        {
            Random random = new Random(randomSeed);
            return (x, count) =>
            {
                double y = stableFunc(x, count);
                double r = random.NextDouble();
                double value = y / 2.0 + (y / 2.0 * r);
                double rounded = Math.Round(value, Settings.DoubleDigits);
                return rounded;
            };
        }

        private static double AnnualFunc(int x, int count)
        {
            return (x + 1) * (x + 1) + 5.0;
        }
    }
}
