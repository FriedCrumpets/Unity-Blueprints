namespace Http.Models
{
    public class CoinPriceDetails
    {
        public string Symbol { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public bool IsPositive { get; set; }

        public double PercentChange1H { get; set; }

        public double PercentChange24H { get; set; }

        public double PercentChange7D { get; set; }

        public double PercentChange30D { get; set; }
    }
}
