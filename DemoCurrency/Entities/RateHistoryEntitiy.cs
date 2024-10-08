﻿using Microsoft.AspNetCore.Http;

namespace DemoCurrency.Entities
{
    public class RateHistoryEntitiy
    {
     
        public double Amount { get; set; }
        public string Base { get; set; }
        public string Start_Date { get; set; }
        public string End_Date { get; set; } 
        public Dictionary<string, Dictionary<string, double>> Rates { get; set; }

        public int ErrorCode { get; set; }
        public int ErrorMessage { get; set; }


    }
}
