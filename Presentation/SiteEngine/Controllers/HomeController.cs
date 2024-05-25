using ApplicationDbContext.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelsEntity;
using SiteEngine.Models;
using System.Diagnostics;

namespace SiteEngine.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IServiceRepository serviceRepository;

        public HomeController(IServiceRepository serviceRepository)
        {
            this.serviceRepository = serviceRepository;
        }

        public IActionResult Index()// show all items
        {
            var serviceModel = serviceRepository.GetAllService();
            return View(serviceModel);
        }

        public IActionResult Details(int id)//show item by id
        {
            var serviceModel = serviceRepository.GetServiceById(id);
            return View(serviceModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(Service service)
        {
            if(ModelState.IsValid)
            {
                serviceRepository.AddNewService(service);
                
                var serviceId = serviceRepository.GetServiceById(service.Id);

                return RedirectToAction("Details", serviceId);
            }
            else
            {
                return View(service);
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult Update(int id)
        {
            var itemForUpdate = serviceRepository.GetServiceById(id);

            return View(itemForUpdate);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Update(Service service)
        {
            var serviceModel = serviceRepository.GetServiceById(service.Id);

            if (serviceModel is not null && ModelState.IsValid)
            {
                serviceRepository.UpdateServiceByModel(service);

                var getIdUpdatedItem = serviceModel.Id;

                return RedirectToAction("Details", new {id = getIdUpdatedItem});
            }
            else
            {
                return View(serviceModel);
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var serviceModelForDelete = serviceRepository.GetServiceById(id);

            if(serviceModelForDelete is not null)
            {
                serviceRepository.DeleteServiceById(serviceModelForDelete.Id);
                return RedirectToAction("Index");
            }
            else
            {
                return View("Details", id);
            }
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [AllowAnonymous]
        [Route("/NotFound")]
        public IActionResult PageNotFound()
        {
            return View();
        }
    }
}
