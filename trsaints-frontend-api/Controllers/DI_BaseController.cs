using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using trsaints_frontend_api.Data.Context;
using trsaints_frontend_api.Data.Entities;

namespace trsaints_frontend_api.Controllers;

public class DI_BaseController : ControllerBase
{
    protected AppDbContext Context { get; }
    protected IAuthorizationService AuthorizationService { get; }
    protected UserManager<ApplicationUser> UserManager { get; }

    public DI_BaseController(AppDbContext context,
                             IAuthorizationService authorizationService,
                             UserManager<ApplicationUser> userManager)
    {
        Context = context;
        UserManager = userManager;
        AuthorizationService = authorizationService;
    }
}
