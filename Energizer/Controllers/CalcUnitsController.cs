using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Energizer.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Energizer
{

    public class CalcUnitsController : Controller
    {
        static EnergizerContext my_context = EnergizerContext.getInstance();
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            ICollection<CalcUnit> CalcUnits = await my_context.GetCalcUnitsAsync("");
            return View(CalcUnits);
        }

        public async Task<IActionResult> Year(int id)
        {
            ICollection<CalcUnit> CalcUnits = await my_context.GetCalcUnitsAsync("year/"+id);
            return View("Index",CalcUnits);
        }
    }
}
