using System;
using DataGenerator;
using DataGenerator.Core;
using Example.Xml.Report;

namespace Example.DataGen
{
    static class Report2
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
                                    Root = GetRoot(asOfDate, String.Empty)
                                },
                            new AccountDelta()
                                {
                                    Delta = "Delta",
                                    Root = GetRoot(asOfDate, "_Delta"),

                                },
                            new AccountBenchmark()
                                {
                                    Benchmark = "BenchMark",
                                    Root = GetRoot(asOfDate, "_BenchMark"),
                                },
                        }
            };
        }

        private static Root GetRoot(string asOfDate, string tag)
        {
            var randomSeed = (asOfDate + tag + "report2").ToInt32();

            var rootCredit =
                new RootCredit()
                    .Generate(
                        root =>
                        root
                            .Property(_ => _.Name).WithStaticValue("serviceTree")
                            .Property(_ => _.Counterparty).WithGeneratedValue(_ => new RandomListValueGenerator<string>(randomSeed, DataLists.Country))
                            .Property(_ => _.Level)
                                .WithNewArray(
                                    InstanceFactory.Create<LevelSector>(),
                                    level1 =>
                                    level1
                                        .Property(_ => _.Name).WithGeneratedValue(_ => new ListValueGenerator<string>(DataLists.Markets))
                                        .Property(_ => _.Leg_Name).WithGeneratedValue(
                                            context =>
                                            {
                                                var regexGenerator = new RegexValueGenerator(randomSeed, @"\w{3}\d*");
                                                return new LambdaValueGenerator<string>(() => Option.Any(context.CurrentInstance.Name + regexGenerator.Next().Value));
                                            })
                                        .Property(_ => _.MV_Pct).WithGeneratedValue(_ => (double)_, _ => new PercentageValueGenerator(randomSeed, DataLists.Markets.Length))
                            )
                    );

            return rootCredit;
        }
    }
}
