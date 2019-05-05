using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Energizer.Models;

namespace Energizer.Controllers
{
    public class ConsumersController : Controller
    {
        static EnergizerContext my_context = EnergizerContext.getInstance();
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            ICollection<Consumer> Consumers = await my_context.GetConsumersAsync();
            return View(Consumers);
        }
    }
}