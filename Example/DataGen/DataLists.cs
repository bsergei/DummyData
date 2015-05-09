namespace Example.DataGen
{
    internal static class DataLists
    {
        public static readonly string[] CreditRatings =
        {
            "AAA", "AA", "A", "BBB", "Below BBB"
        };

        public static readonly decimal[] CreditRatingsWeights =
        {
            0.5m, 0.1m, 0.2m, 0.15m, 0.05m
        };

        public static readonly string[] CreditRatingsAll =
        {
            "AAA", "AA", "A", "BBB", "BB", "B", "CCC", "CC", "C"
        };

        public static readonly decimal[] CreditRatingsAllWeights =
        {
            1m, 1m, 1m, 1m, 1m, 1m, 1m, 1m, 1m
        };

        public static readonly string[] Markets =
        {
            "Corp",
            "Securities",
            "MBS",
            "Capital",
        };

        public static readonly string[] AnnualPeriods =
        {
            "2015", "2014", "2013", "2012", "2011"
        };

        public static readonly string[] Country =
        {
            "Blank",
            "Australia",
            "Austria",
            "Belgium",
            "Brazil",
            "Canada",
            "Chile",
            "China",
            "Czech Republic",
            "Denmark",
            "France",
            "Germany",
            "Hong Kong SAR",
            "Japan",
            "Korea, Republic of",
            "Luxembourg",
            "Malaysia",
            "Mexico",
            "Netherlands",
            "Norway",
            "Poland",
            "Singapore",
            "Slovak Republic",
            "South Africa",
            "Sweden",
            "Switzerland",
            "United Kingdom",
            "United States"
        };

        public static readonly string[] Periods =
        {
            "0d", "1d", "5d", "1m", "3m", "6m", "9m", "1y", "18m", "2y", "30m", 
            "3y", "4y", "5y", "6y", "7y", "8y", "9y", "10y", "15y", "20y", "30y"
        };
    }
}