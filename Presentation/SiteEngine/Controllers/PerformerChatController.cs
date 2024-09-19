using ApplicationDbContext;
using Microsoft.AspNetCore.Mvc;

namespace SiteEngine.Controllers
{
    public class PerformerChatController : BaseController
    {
        private readonly AppDbContext context;

        public PerformerChatController(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> IndexAsync(int? id)
        {
            if(id.HasValue)
            {

            }

            return View();
        }
    }
}
