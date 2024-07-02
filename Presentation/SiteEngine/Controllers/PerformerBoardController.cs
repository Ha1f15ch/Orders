using ApplicationDbContext;
using ApplicationDbContext.Interfaces;
using SiteEngine.Models.Amplua;
using Microsoft.AspNetCore.Authorization;
using ModelsEntity;
using Microsoft.AspNetCore.Mvc;

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
    }
}
