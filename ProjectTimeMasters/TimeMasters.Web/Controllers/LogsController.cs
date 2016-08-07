using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TimeMasters.Web.Data;
using TimeMasters.Web.Models.Logging;

namespace TimeMasters.Web.Controllers
{
    public class LogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LogsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Logs
        public async Task<IActionResult> Index()
        {
            IEnumerable<Log> tmp = await _context.Log.ToListAsync();
            foreach(Log l in tmp)
            {
                l.Environment = await _context.Environment.SingleOrDefaultAsync(m => m.LogID == l.ID);
                l.Environment.MetroLogVersion = await _context.MetroLogVersion.SingleOrDefaultAsync(m => m.EnvironmentID == l.Environment.ID);
                l.Events = await _context.Events.SingleOrDefaultAsync(m => m.LogID == l.ID);
                l.Events.Exception = await _context.Exception.SingleOrDefaultAsync(m => m.EventsID == l.Events.ID);
                l.Events.ExceptionWrapper = await _context.ExceptionWrapper.SingleOrDefaultAsync(m => m.EventsID == l.Events.ID);
            }

            return View(tmp);
        }

        // GET: Logs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var log = await _context.Log.SingleOrDefaultAsync(m => m.ID == id);
            if (log == null)
            {
                return NotFound();
            }
            log.Environment = await _context.Environment.SingleOrDefaultAsync(m => m.LogID == id);
            log.Environment.MetroLogVersion = await _context.MetroLogVersion.SingleOrDefaultAsync(m => m.EnvironmentID == log.Environment.ID);
            log.Events = await _context.Events.SingleOrDefaultAsync(m => m.LogID == id);
            log.Events.Exception = await _context.Exception.SingleOrDefaultAsync(m => m.EventsID == log.Events.ID);
            log.Events.ExceptionWrapper = await _context.ExceptionWrapper.SingleOrDefaultAsync(m => m.EventsID == log.Events.ID);

            return View(log);
        }

        // GET: Logs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Logs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID")] Log log)
        {
            if (ModelState.IsValid)
            {
                _context.Add(log);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(log);
        }

        // GET: Logs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var log = await _context.Log.SingleOrDefaultAsync(m => m.ID == id);
            if (log == null)
            {
                return NotFound();
            }
            return View(log);
        }

        // POST: Logs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID")] Log log)
        {
            if (id != log.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(log);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LogExists(log.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(log);
        }

        // GET: Logs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var log = await _context.Log.SingleOrDefaultAsync(m => m.ID == id);
            if (log == null)
            {
                return NotFound();
            }

            return View(log);
        }

        // POST: Logs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var log = await _context.Log.SingleOrDefaultAsync(m => m.ID == id);
            _context.Log.Remove(log);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool LogExists(int id)
        {
            return _context.Log.Any(e => e.ID == id);
        }
    }
}
