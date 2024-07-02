using ApplicationDbContext;
using ApplicationDbContext.Interfaces;
using SiteEngine.Models.Amplua;
using Microsoft.AspNetCore.Authorization;
using ModelsEntity;
using Microsoft.AspNetCore.Mvc;

namespace SiteEngine.Controllers
{
    public class CustomerBoardController : BaseController
    {
        private readonly AppDbContext context;
        private readonly IProfileCustomerRepositories profileCustomerRepositories;

        public CustomerBoardController(AppDbContext context, IProfileCustomerRepositories profileCustomerRepositories)
        {
            this.context = context;
            this.profileCustomerRepositories = profileCustomerRepositories;
        }
    }
}
