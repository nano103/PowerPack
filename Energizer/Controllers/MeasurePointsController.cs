using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Energizer.Models;

namespace Energizer.Controllers
{
    public class MeasurePointsController : Controller
    {
        static EnergizerContext my_context= EnergizerContext.getInstance();

        // GET: MeasurePoints
        public async Task<IActionResult> Index()
        {
            ICollection<MeasurePoint>  MeasurePoints = await my_context.GetMeasurePointsAsync();
            return View(MeasurePoints);
        }

        // POST: MeasurePoints/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MeasurePoint measurePoint)
        {
            if (ModelState.IsValid)
            {
                await my_context.CreateMeasurePointAsync(measurePoint);
                return RedirectToAction(nameof(Index));
            }
            return View(measurePoint);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            //var vm = new MeasurePoint();
            ICollection<Consumer> Consumers = await my_context.GetConsumersAsync();
            var ConsList = new SelectList(Consumers,"ID","Title");
            /*
            foreach(Consumer c in Consumers)
            {
                var SItem = new SelectListItem { Text = c.Title, Value = c.ID.ToString() };
                if (ConsList==null)
                    ConsList = new SelectList(SItem);
                else
                    ConsList.Append(SItem);
            }
            */
            ViewBag.Consumers = ConsList;
            return View();
        }

    }
}
