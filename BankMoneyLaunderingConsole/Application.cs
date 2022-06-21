using AutoMapper;
using Bank.Models;
using Bank.Services;
using BankMoneyLaunderingConsole.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BankAPI.Controllers.CustomerProfileController;
using static BankMoneyLaunderingConsole.Services.LaunderingService;

namespace BankMoneyLaunderingConsole
{
    public class Application
    {
        private readonly ILaunderingService _launderingService;

        public Application(ILaunderingService launderingService)        
        {
            _launderingService = launderingService;
        }

        public class LaunderingResults
        {
            public int CustomerId { get; set; }
            public int AccountId { get; set; }
            public int TransactionId { get; set; }
            public decimal Amount { get; set; }
            public DateTime TransactionDate { get; set; }
            public string Reason { get; set; }
        }

        public void Run()
        {
            // Read date from file!!!
            // Checking last date checked!
            string LastDateCheckedStr;
            using (StreamReader myReadStream = new StreamReader(@"../../../Reports/Report-CheckedTo.txt"))
            {
                LastDateCheckedStr = myReadStream.ReadLine();
            }

            var LastDateCheckedParsed = DateTime.Parse(LastDateCheckedStr);


            // Lets check ou transactions
            string[] countries = { "SE", "DK", "FI", "NO" };

            foreach (string country in countries)
            {
                var customersToCheck = _launderingService.FindTransactions(country, LastDateCheckedParsed);
                var checkedTransactions = _launderingService.CheckTransactions(customersToCheck);
                _launderingService.PrintReport(checkedTransactions, country);
            }

            // Save todays date to a file!!!
            var todayToFile = DateTime.Now.Date;

            using (StreamWriter myStream = new StreamWriter(@"../../../Reports/Report-CheckedTo.txt", append: false))
            {
                myStream.WriteLine(todayToFile);
            }











        }
    }
}
