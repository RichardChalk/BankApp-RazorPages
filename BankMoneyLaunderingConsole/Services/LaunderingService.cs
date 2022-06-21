using AutoMapper;
using Bank.Models;
using Bank.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BankMoneyLaunderingConsole.Application;

namespace BankMoneyLaunderingConsole.Services
{
    public class LaunderingService : ILaunderingService
    {
        private readonly ICustomerService _customerService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;


        public LaunderingService(IAccountService accountService, ICustomerService customerService, IMapper mapper)
        {
            _accountService = accountService;
            _customerService = customerService;
            _mapper = mapper;
        }

        public class CustomerConsoleViewModel
        {
            public int Id { get; set; }
            public string Givenname { get; set; } = null!;
            public string Surname { get; set; } = null!;
            public string Name { get; set; } = null!;
            public List<Account> Accounts { get; set; }
            public List<Transaction> Transactions { get; set; }
        }

        public List<CustomerConsoleViewModel> FindTransactions(string country, DateTime checkedTo)
        {
            // Find all customers incl transactions
            var customers = _customerService.GetCustomers()
                    .Where(c => c.CountryCode == country).ToList();

            // Add accounts
            List<CustomerConsoleViewModel> customersInclAccounts = new List<CustomerConsoleViewModel>();

            foreach (var customer in customers)
            {
                var customerModel = new CustomerConsoleViewModel();

                // Data binding.... boring stuff
                _mapper.Map(customer, customerModel);

                // Collect customer accounts
                customerModel.Accounts = _accountService.GetCustomerAccounts(customer.CustomerId).ToList();

                foreach (var account in customerModel.Accounts)
                {
                    customerModel.Transactions = _accountService.GetTransactions(account.AccountId)
                        // if datetime > datetime in file!!! (file i saved on comp)
                        .Where(t=>t.Date.Date > checkedTo)
                        .OrderByDescending(t => t.Date)
                        .ToList();
                }

                // Add all customers incl Accounts & Transactions to list
                customersInclAccounts.Add(customerModel);
            }
            return customersInclAccounts;
        }

        public List<LaunderingResults> CheckTransactions(List<CustomerConsoleViewModel> customersToCheck)
        {
            var results = new List<LaunderingResults>();

            foreach (var customer in customersToCheck)
            {
                foreach (var transaction in customer.Transactions)
                {
                    // Rule 1: En enskild transaktion större än 15000 kr, 
                    if (transaction.Amount > 15000)
                    {
                        results.Add(new LaunderingResults
                        {
                            CustomerId = customer.Id,
                            AccountId = transaction.AccountId,
                            TransactionId = transaction.TransactionId,
                            Amount = transaction.Amount,
                            TransactionDate = transaction.Date,
                            Reason = "Single transaction above 15000kr"
                        });
                    }


                    // Rule 2: Om totala transaktioner de senaste tre dygnen (72h)
                    // från aktuellt tidpunkt större än 23000kr

                    // Create a list of all transactions in the last 3 days (72h)
                    var transactionsLastThreeDays = customer.Transactions
                        .Where(t => 
                            t.Date == transaction.Date || 
                            t.Date >= transaction.Date.AddHours(-72));

                    // If last 3 days transactions exceed 23000kr
                    if (transactionsLastThreeDays.Sum(t => t.Amount) > 23000)
                    {
                        var transactionChecked = new LaunderingResults()
                        {
                            CustomerId = customer.Id,
                            AccountId = transaction.AccountId,
                            TransactionId = transaction.TransactionId,
                            Amount = transaction.Amount,
                            TransactionDate = transaction.Date,
                            Reason = "Last 3 days transactions above 23000kr"
                        };

                            results.Add(transactionChecked);
                    }

                    // Couldnt get this to work!
                    // _accountService.SetCheckedInConsole(transaction);

                }
            }
            return results;
        }

        public void PrintReport(List<LaunderingResults> checkedTransactions, string country)
        {
            DateTime created = DateTime.UtcNow;
            var newDate = created.ToShortDateString();

            using (StreamWriter myStream = new StreamWriter($"../../../Reports/Report-{country}-{newDate}.txt", append: false))
            {
                foreach (var trans in checkedTransactions)
                {
                    myStream.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                    myStream.WriteLine($"AccountId: {trans.AccountId}");
                    myStream.WriteLine($"TransactionId: {trans.TransactionId}");
                    myStream.WriteLine($"CustomerId: {trans.CustomerId}");
                    myStream.WriteLine($"Amount (kr): {trans.Amount}");
                    myStream.WriteLine($"Transaction Date: {trans.TransactionDate.ToShortDateString()}");
                    myStream.WriteLine($"Reason: {trans.Reason}");
                }
            }
        }
    }
}
