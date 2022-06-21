using Bank.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SkysDemo3_1.Infrastructure.Paging;
using static Bank.Pages.Accounts.AccountModel;
using static Bank.Pages.Accounts.DepositModel;
using static Bank.Pages.Accounts.TransferModel;
using static Bank.Pages.Accounts.WithdrawalModel;

namespace Bank.Services
{
    public class AccountService : IAccountService
    {
        private readonly JAFU20BankContext _context;

        public AccountService(JAFU20BankContext context)
        {
            _context = context;
        }

        // Get all accounts for start page
        public IEnumerable<Account> GetAccounts()
        {
            var accounts = _context.Accounts;
            return accounts;
        }

        // Get all accounts for table page
        public PagedResult<Account> GetAccounts(string sortColumn, string sortOrder, string searchText, int page)
        {
            var query = _context.Accounts.AsQueryable();

            if (sortColumn == "AccountId" || string.IsNullOrEmpty(sortColumn))
                if (sortOrder == "asc")
                    query = query.OrderBy(a => a.AccountId);
                else
                    query = query.OrderByDescending(a => a.AccountId);

            if (sortColumn == "Frequency" || string.IsNullOrEmpty(sortColumn))
                if (sortOrder == "asc")
                    query = query.OrderBy(a => a.Frequency);
                else
                    query = query.OrderByDescending(a => a.Frequency);

            else if (sortColumn == "Created" || string.IsNullOrEmpty(sortColumn))
                if (sortOrder == "desc")
                    query = query.OrderByDescending(a => a.Created);
                else
                    query = query.OrderBy(a => a.Created);


            else if (sortColumn == "Balance" || string.IsNullOrEmpty(sortColumn))
                if (sortOrder == "asc")
                    query = query.OrderBy(a => a.Balance);
                else
                    query = query.OrderByDescending(a => a.Balance);


            //Om det finns någon söksträng...
            if (searchText != null)
            {
                query = query
                .Where(a =>
                a.AccountId.ToString() == searchText ||
                a.Frequency.ToLower().Contains(searchText.ToLower()) ||
                a.Created.ToString() == searchText ||
                a.Balance.ToString() == searchText
                );
            }

            return query.GetPaged(page, 50);
        }


        // Get all accounts for a single customer for Customer Profile page
        public IEnumerable<Account> GetCustomerAccounts(int customerId)
        {
            var accounts = _context.Dispositions
                .Where(d => d.CustomerId == customerId)
                .Select(d => d.Account);

            return accounts;
        }

        // Get 1 account for a single customer using accountId
        public Account GetCustomerAccount(int accountId)
        {
            var account = _context.Accounts
                .FirstOrDefault(a => a.AccountId == accountId);

            return account;
        }


        // Get all transactions for a single account
        public IEnumerable<Transaction> GetTransactions(int accountId)
        {
            var account = _context.Accounts
                .Include(a => a.Transactions)
                .First(a => a.AccountId == accountId);


            var transactions = account.Transactions
                .ToList();

            return transactions;
        }

        // Create a fresh account
        public Account GetNewAccount()
        {
            var account = new Account();

            return account;
        }

        // Create a fresh disposition
        public Disposition GetNewDisposition()
        {
            var disposition = new Disposition();

            return disposition;
        }

        // Create a fresh deposit View model
        public DepositViewModel GetNewDepositViewModel()
        {
            var deposit = new DepositViewModel();

            return deposit;
        }

        // Create a fresh withdrawal View model
        public WithdrawalViewModel GetNewWithdrawalViewModel()
        {
            var withdrawal = new WithdrawalViewModel();

            return withdrawal;
        }

        // Create a fresh transfer View model
        public TransferViewModel GetNewTransferViewModel()
        {
            var transfer = new TransferViewModel();

            return transfer;
        }


        public void MakeDeposit(Account account, decimal amount)
        {
            account.Balance += amount;
            account.Transactions.Add(new Transaction
            {
                AccountId = account.AccountId,
                Date = DateTime.Now,
                Type = "Debit",
                Operation = "Withdrawal in Cash",
                Amount = amount,
                Balance = account.Balance,

                // These are Nullable... I haven't filled these out
                //Symbol = ,
                //Bank = ,
                //Account = ,
            });

            _context.SaveChanges();
        }

        public void MakeWithdrawal(Account account, decimal amount)
        {
            account.Balance -= amount;
            account.Transactions.Add(new Transaction
            {
                AccountId = account.AccountId,
                Date = DateTime.Now,
                Type = "Debit",
                Operation = "Withdrawal in Cash",
                Amount = amount * -1, // I want to display a negative number in html
                Balance = account.Balance,

                // These are Nullable... I haven't filled these out
                //Symbol = ,
                //Bank = ,
                //Account = ,
            });

            _context.SaveChanges();
        }

        public void MakeTransfer(Account account, Account accountTo, decimal amount)
        {

            // Lets handle the account been tranferred FROM first...
            account.Balance -= amount;
            account.Transactions.Add(new Transaction
            {
                AccountId = account.AccountId,
                Date = DateTime.Now,
                Type = "Debit",
                Operation = "Remittance to Another Bank",
                Amount = amount * -1, // I want to display a negative number in html
                Balance = account.Balance,

                // These are Nullable... I haven't filled these out
                //Symbol = ,
                //Bank = ,
                //Account = ,
            });


            // Now the account been tranferred TO...
            accountTo.Balance += amount;
            accountTo.Transactions.Add(new Transaction
            {
                AccountId = accountTo.AccountId,
                Date = DateTime.Now,
                Type = "Credit",
                Operation = "Collection from Another Bank",
                Amount = amount,
                Balance = accountTo.Balance,

                // These are Nullable... I haven't filled these out
                //Symbol = ,
                //Bank = ,
                //Account = ,
            });

            _context.SaveChanges();
        }



        public void CreateAccount(int customerId, string frequency, decimal balance)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == customerId);
            var account = this.GetNewAccount();
            var disposition = this.GetNewDisposition();

            // Tie diposition to correct customer
            disposition.CustomerId = customer.CustomerId;
            disposition.Account = account;
            disposition.AccountId = account.AccountId;
            disposition.Type = "OWNER";

            account.Frequency = frequency;

            // Create a transaction when creating a new account
            this.MakeDeposit(account, balance);

            // Set todays date as account creation
            DateTime today = DateTime.Now;
            account.Created = today;

            customer.Dispositions.Add(disposition);

            _context.SaveChanges();
        }

        public List<SelectListItem> FillFrequencyDropDown()
        {
            List<SelectListItem> frequencies = new List<SelectListItem>();
            frequencies = _context.Frequencies.Select(f => new SelectListItem
            {
                Text = f.FreqeuncyLabel,
                Value = f.FreqeuncyLabel.Replace(" ", ""),
            })
                .ToList();

            frequencies.Insert(0, new SelectListItem
            {
                Text = "Choose one...",
                Value = ""
            });

            return frequencies;
        }

       
        // Couldnt get this to work!!!
        public void SetCheckedInConsole(Transaction transaction)
        {
            var transactionToUpdate = _context.Transactions.Find(transaction.TransactionId);

            transactionToUpdate.LaunderingChecked = true;
            _context.SaveChanges();
        }
    }
}
