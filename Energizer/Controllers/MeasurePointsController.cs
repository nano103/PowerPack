using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ACDC;
using Energizer.Models;

namespace Energizer.Controllers
{
    public class MeasurePointsController : Controller
    {
        private readonly EnergizerContext _context;

        public MeasurePointsController(EnergizerContext context)
        {
            _context = context;
        }

        // GET: MeasurePoints
        public async Task<IActionResult> Index()
        {
            var energizerContext = _context.MeasurePoint.Include(m => m.Consumer).Include(m => m.CurrentTransformer).Include(m => m.VoltageTransformer);
            return View(await energizerContext.ToListAsync());
        }

        // GET: MeasurePoints/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var measurePoint = await _context.MeasurePoint
                .Include(m => m.Consumer)
                .Include(m => m.CurrentTransformer)
                .Include(m => m.VoltageTransformer)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (measurePoint == null)
            {
                return NotFound();
            }

            return View(measurePoint);
        }

        // GET: MeasurePoints/Create
        public IActionResult Create()
        {
            ViewData["ConsumerID"] = new SelectList(_context.Set<Consumer>(), "ID", "ID");
            ViewData["CurrentTransformerID"] = new SelectList(_context.Set<CurrentTransformer>(), "ID", "ID");
            ViewData["VoltageTransformerID"] = new SelectList(_context.Set<VoltageTransformer>(), "ID", "ID");
            return View();
        }

        // POST: MeasurePoints/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConsumerID,ElectricMeterID,VoltageTransformerID,CurrentTransformerID,Title,ID")] MeasurePoint measurePoint)
        {
            if (ModelState.IsValid)
            {
                _context.Add(measurePoint);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ConsumerID"] = new SelectList(_context.Set<Consumer>(), "ID", "ID", measurePoint.ConsumerID);
            ViewData["CurrentTransformerID"] = new SelectList(_context.Set<CurrentTransformer>(), "ID", "ID", measurePoint.CurrentTransformerID);
            ViewData["VoltageTransformerID"] = new SelectList(_context.Set<VoltageTransformer>(), "ID", "ID", measurePoint.VoltageTransformerID);
            return View(measurePoint);
        }

        // GET: MeasurePoints/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var measurePoint = await _context.MeasurePoint.FindAsync(id);
            if (measurePoint == null)
            {
                return NotFound();
            }
            ViewData["ConsumerID"] = new SelectList(_context.Set<Consumer>(), "ID", "ID", measurePoint.ConsumerID);
            ViewData["CurrentTransformerID"] = new SelectList(_context.Set<CurrentTransformer>(), "ID", "ID", measurePoint.CurrentTransformerID);
            ViewData["VoltageTransformerID"] = new SelectList(_context.Set<VoltageTransformer>(), "ID", "ID", measurePoint.VoltageTransformerID);
            return View(measurePoint);
        }

        // POST: MeasurePoints/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ConsumerID,ElectricMeterID,VoltageTransformerID,CurrentTransformerID,Title,ID")] MeasurePoint measurePoint)
        {
            if (id != measurePoint.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(measurePoint);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MeasurePointExists(measurePoint.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ConsumerID"] = new SelectList(_context.Set<Consumer>(), "ID", "ID", measurePoint.ConsumerID);
            ViewData["CurrentTransformerID"] = new SelectList(_context.Set<CurrentTransformer>(), "ID", "ID", measurePoint.CurrentTransformerID);
            ViewData["VoltageTransformerID"] = new SelectList(_context.Set<VoltageTransformer>(), "ID", "ID", measurePoint.VoltageTransformerID);
            return View(measurePoint);
        }

        // GET: MeasurePoints/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var measurePoint = await _context.MeasurePoint
                .Include(m => m.Consumer)
                .Include(m => m.CurrentTransformer)
                .Include(m => m.VoltageTransformer)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (measurePoint == null)
            {
                return NotFound();
            }

            return View(measurePoint);
        }

        // POST: MeasurePoints/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var measurePoint = await _context.MeasurePoint.FindAsync(id);
            _context.MeasurePoint.Remove(measurePoint);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MeasurePointExists(int id)
        {
            return _context.MeasurePoint.Any(e => e.ID == id);
        }
    }
}
