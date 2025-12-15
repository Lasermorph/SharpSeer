using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using SharpSeer.Interfaces;
using SharpSeer.Models;
using SharpSeer.Services;

namespace SharpSeer.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    SharpSeerDbContext Seer;
    IService<Cohort> CohortService;

    public IndexModel(ILogger<IndexModel> logger, SharpSeerDbContext seer, IService<Cohort> cohortService)
    {
        _logger = logger;
        Seer = seer;
        CohortService = cohortService;
    }

    public void OnGet()
    {
        //Cohort cohort1 = new Cohort("Alice2023", "Datamatiker", 2023);
        //CohortService.Delete(cohort1);
        

    }
}
