namespace DemoCurrency.Entities
{
    public class RateEntitties
    {
        public double Amount { get; set; }
        public string Base { get; set; }
        public string Date { get; set; }
        public Dictionary<string, double> Rates { get; set; }

    }
}
