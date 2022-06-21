using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BankMoneyLaunderingConsole.Application;
using static BankMoneyLaunderingConsole.Services.LaunderingService;

namespace BankMoneyLaunderingConsole.Services
{
    public interface ILaunderingService
    {
        List<CustomerConsoleViewModel> FindTransactions(string country, DateTime checkedTo);
        List<LaunderingResults> CheckTransactions(List<CustomerConsoleViewModel> customerToCheck);
        void PrintReport(List<LaunderingResults> checkedTransactionsSE, string country);
    }
}
