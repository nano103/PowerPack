using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Energizer.Models;

namespace Energizer.Controllers
{
    public class CTsController : Controller
    {
        static EnergizerContext my_context = EnergizerContext.getInstance();
        public async Task<IActionResult> Index()
        {
            ICollection<CurrentTransformer> ElectricMeters = await my_context.GetOutdatedAsync<CurrentTransformer>("cts/0");
            return View(ElectricMeters);
        }

        public async Task<IActionResult> Outdated(int id)
        {
            ICollection<CurrentTransformer> ElectricMeters = await my_context.GetOutdatedAsync<CurrentTransformer>("cts/" + id);
            return View("Index", ElectricMeters);
        }
    }
}