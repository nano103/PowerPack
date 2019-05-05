using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Energizer.Models;

namespace Energizer.Controllers
{
    public class ElectricMetersController : Controller
    {
        static EnergizerContext my_context = EnergizerContext.getInstance();
        public async Task<IActionResult> Index()
        {
            ICollection<ElectricMeter> ElectricMeters = await my_context.GetOutdatedAsync<ElectricMeter>("meters/0");
            return View(ElectricMeters);
        }
        
        public async Task<IActionResult> Outdated(int id)
        {
            ICollection<ElectricMeter> ElectricMeters = await my_context.GetOutdatedAsync<ElectricMeter>("meters/"+id);
            return View("Index",ElectricMeters);
        }
    }
}