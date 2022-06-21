using Bank.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bank.Pages
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ICountryDataService _countryDataService;

        public class IndexViewModel
        {
            public int NumberOfClientsSE { get; set; }
            public int NumberOfAccountsSE { get; set; }
            public decimal TotalAccountValueSE { get; set; }


            public int NumberOfClientsNO { get; set; }
            public int NumberOfAccountsNO { get; set; }
            public decimal TotalAccountValueNO { get; set; }


            public int NumberOfClientsFI { get; set; }
            public int NumberOfAccountsFI { get; set; }
            public decimal TotalAccountValueFI { get; set; }


            public int NumberOfClientsDK { get; set; }
            public int NumberOfAccountsDK { get; set; }
            public decimal TotalAccountValueDK { get; set; }

        }

        public IndexViewModel ViewModelSE { get; set; } = new IndexViewModel();
        public IndexViewModel ViewModelNO { get; set; } = new IndexViewModel();
        public IndexViewModel ViewModelFI { get; set; } = new IndexViewModel();
        public IndexViewModel ViewModelDK { get; set; } = new IndexViewModel();


        public IndexModel(
            ILogger<IndexModel> logger,
            ICountryDataService countryDataService)
        {
            _logger = logger;
            _countryDataService = countryDataService;
        }


        public void OnGet()
        {
            // SWEDEN /////////////////////////////////////////////////////////////////
            ViewModelSE.NumberOfClientsSE = _countryDataService.GetCountryCustomersCount("SE");
            ViewModelSE.NumberOfAccountsSE = _countryDataService.GetCountryAccountsCount("SE");
            ViewModelSE.TotalAccountValueSE = _countryDataService.GetCountryBalance("SE");


            // FINLAND /////////////////////////////////////////////////////////////////
            ViewModelFI.NumberOfClientsFI = _countryDataService.GetCountryCustomersCount("FI");
            ViewModelFI.NumberOfAccountsFI = _countryDataService.GetCountryAccountsCount("FI");
            ViewModelFI.TotalAccountValueFI = _countryDataService.GetCountryBalance("FI");


            // DENMARK /////////////////////////////////////////////////////////////////
            ViewModelDK.NumberOfClientsDK = _countryDataService.GetCountryCustomersCount("DK");
            ViewModelDK.NumberOfAccountsDK = _countryDataService.GetCountryAccountsCount("DK");
            ViewModelDK.TotalAccountValueDK = _countryDataService.GetCountryBalance("DK");


            // NORWAY /////////////////////////////////////////////////////////////////
            ViewModelNO.NumberOfClientsNO = _countryDataService.GetCountryCustomersCount("NO");
            ViewModelNO.NumberOfAccountsNO = _countryDataService.GetCountryAccountsCount("NO");
            ViewModelNO.TotalAccountValueNO = _countryDataService.GetCountryBalance("NO");
        }
    }
}