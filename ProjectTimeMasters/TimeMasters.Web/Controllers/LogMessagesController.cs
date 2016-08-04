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
    public class LogMessagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LogMessagesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: LogMessages
        public async Task<IActionResult> Index()
        {
            return View(await _context.LogMessage.ToListAsync());
        }

        // GET: LogMessages/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logMessage = await _context.LogMessage.SingleOrDefaultAsync(m => m.ID == id);
            if (logMessage == null)
            {
                return NotFound();
            }

            return View(logMessage);
        }

        // GET: LogMessages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LogMessages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Message")] LogMessage logMessage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(logMessage);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(logMessage);
        }

        // GET: LogMessages/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logMessage = await _context.LogMessage.SingleOrDefaultAsync(m => m.ID == id);
            if (logMessage == null)
            {
                return NotFound();
            }
            return View(logMessage);
        }

        // POST: LogMessages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,Message")] LogMessage logMessage)
        {
            if (id != logMessage.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(logMessage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LogMessageExists(logMessage.ID))
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
            return View(logMessage);
        }

        // GET: LogMessages/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logMessage = await _context.LogMessage.SingleOrDefaultAsync(m => m.ID == id);
            if (logMessage == null)
            {
                return NotFound();
            }

            return View(logMessage);
        }

        // POST: LogMessages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var logMessage = await _context.LogMessage.SingleOrDefaultAsync(m => m.ID == id);
            _context.LogMessage.Remove(logMessage);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool LogMessageExists(string id)
        {
            return _context.LogMessage.Any(e => e.ID == id);
        }
    }
}
