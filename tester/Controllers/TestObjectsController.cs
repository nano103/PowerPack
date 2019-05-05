using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using tester.Models;

namespace tester.Controllers
{
    public class TestObjectsController : Controller
    {
        private readonly testerContext _context;

        public TestObjectsController(testerContext context)
        {
            _context = context;
        }

        // GET: TestObjects
        public async Task<IActionResult> Index()
        {
            return View(await _context.TestObject.ToListAsync());
        }

        // GET: TestObjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testObject = await _context.TestObject
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testObject == null)
            {
                return NotFound();
            }

            return View(testObject);
        }

        // GET: TestObjects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TestObjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,name,about")] TestObject testObject)
        {
            if (ModelState.IsValid)
            {
                _context.Add(testObject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(testObject);
        }

        // GET: TestObjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testObject = await _context.TestObject.FindAsync(id);
            if (testObject == null)
            {
                return NotFound();
            }
            return View(testObject);
        }

        // POST: TestObjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,name,about")] TestObject testObject)
        {
            if (id != testObject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testObject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestObjectExists(testObject.Id))
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
            return View(testObject);
        }

        // GET: TestObjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testObject = await _context.TestObject
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testObject == null)
            {
                return NotFound();
            }

            return View(testObject);
        }

        // POST: TestObjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var testObject = await _context.TestObject.FindAsync(id);
            _context.TestObject.Remove(testObject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestObjectExists(int id)
        {
            return _context.TestObject.Any(e => e.Id == id);
        }
    }
}
