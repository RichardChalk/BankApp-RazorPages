using Bank.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using SkysDemo3_1.Infrastructure.Paging;
using static Bank.Pages.Accounts.AccountModel;
using static Bank.Pages.Accounts.DepositModel;
using static Bank.Pages.Accounts.TransferModel;
using static Bank.Pages.Accounts.WithdrawalModel;

namespace Bank.Services
{
    public interface IAccountService
    {
        IEnumerable<Account> GetAccounts();
        PagedResult<Account> GetAccounts(string sortColumn, string sortOrder, string searchText, int page);
        IEnumerable<Account> GetCustomerAccounts(int customerId);
        Account GetCustomerAccount(int accountId);
        IEnumerable<Transaction> GetTransactions(int accountId);
        Account GetNewAccount();
        Disposition GetNewDisposition();
        DepositViewModel GetNewDepositViewModel();
        WithdrawalViewModel GetNewWithdrawalViewModel();
        TransferViewModel GetNewTransferViewModel();
        void MakeDeposit(Account account, decimal amount);
        void MakeWithdrawal(Account account, decimal amount);
        void MakeTransfer(Account account, Account accountTo, decimal amount);
        void CreateAccount(int customerId, string frequency, decimal balance);
        List<SelectListItem> FillFrequencyDropDown();
        void SetCheckedInConsole(Transaction transaction);
    }
}
