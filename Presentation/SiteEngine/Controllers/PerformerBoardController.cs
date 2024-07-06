using ApplicationDbContext;
using ApplicationDbContext.Interfaces;
using SiteEngine.Models.Amplua;
using Microsoft.AspNetCore.Authorization;
using ModelsEntity;
using Microsoft.AspNetCore.Mvc;
using ApplicationDbContext.ContextRepositories;

namespace SiteEngine.Controllers
{
    public class PerformerBoardController : BaseController
    {
        private readonly AppDbContext context;
        private readonly IProfilePerformerRepositories profilePerformerRepositories;
        private readonly IPerformerServiceMappingRepositories performerServiceMappingRepositories;
        private readonly IServiceRepository serviceRepository;

        public PerformerBoardController(AppDbContext context, 
                                        IProfilePerformerRepositories profilePerformerRepositories, 
                                        IPerformerServiceMappingRepositories performerServiceMappingRepositories, 
                                        IServiceRepository serviceRepository)
        {
            this.context = context;
            this.profilePerformerRepositories = profilePerformerRepositories;
            this.performerServiceMappingRepositories = performerServiceMappingRepositories;
            this.serviceRepository = serviceRepository;
        }

        private int GetUserIdFromCookie()
        {
            if (Request.Cookies.TryGetValue("userID", out string userIDString))
            {
                if (int.TryParse(userIDString, out int userID))
                {
                    return userID;
                }
            }
            return 0;
        }


        [Authorize, HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var myProfilePerformerModel = await profilePerformerRepositories.GetProfilePerformer(GetUserIdFromCookie());

            return View(myProfilePerformerModel);
        }
    }
}
