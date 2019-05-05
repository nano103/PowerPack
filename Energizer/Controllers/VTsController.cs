using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Energizer.Models;

namespace Energizer.Controllers
{
    public class VTsController : Controller
    {
        static EnergizerContext my_context = EnergizerContext.getInstance();
        public async Task<IActionResult> Index()
        {
            ICollection<VoltageTransformer> ElectricMeters = await my_context.GetOutdatedAsync<VoltageTransformer>("vts/0");
            return View(ElectricMeters);
        }

        public async Task<IActionResult> Outdated(int id)
        {
            ICollection<VoltageTransformer> ElectricMeters = await my_context.GetOutdatedAsync<VoltageTransformer>("vts/" + id);
            return View("Index", ElectricMeters);
        }
    }
}