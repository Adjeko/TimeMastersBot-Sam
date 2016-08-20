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
            foreach (Log l in tmp)
            {
                l.Environment = await _context.Environment.SingleOrDefaultAsync(m => m.LogID == l.ID);
                l.Environment.MetroLogVersion = await _context.MetroLogVersion.SingleOrDefaultAsync(m => m.EnvironmentID == l.Environment.ID);

                List<Events> tmpEvents;
                if (l.Events != null)
                {
                    tmpEvents = l.Events.ToList();
                    tmpEvents[0] = await _context.Events.SingleOrDefaultAsync(m => m.LogID == l.ID);
                    tmpEvents[0].Exception =
                        await _context.Exception.SingleOrDefaultAsync(m => m.EventsID == tmpEvents[0].ID);
                    tmpEvents[0].ExceptionWrapper =
                        await _context.ExceptionWrapper.SingleOrDefaultAsync(m => m.EventsID == tmpEvents[0].ID);

                    l.Events = tmpEvents;
                }   
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


            List<Events> tmpEvents;
            if (log.Events != null)
            {
                tmpEvents = log.Events.ToList();
                tmpEvents[0] = await _context.Events.SingleOrDefaultAsync(m => m.LogID == id);
                tmpEvents[0].Exception =
                    await _context.Exception.SingleOrDefaultAsync(m => m.EventsID == tmpEvents[0].ID);
                tmpEvents[0].ExceptionWrapper =
                    await _context.ExceptionWrapper.SingleOrDefaultAsync(m => m.EventsID == tmpEvents[0].ID);

                log.Events = tmpEvents;
            }

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

        [HttpPost]
        public async Task<IActionResult> AddLog([FromBody] Log root)
        {
            //Log tmp = new Log();

            //Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Log> ent = _context.Log.Add(tmp);
            //await _context.SaveChangesAsync();

            //Environment env = new Environment
            //{
            //    MachineName = root.Environment.MachineName,

            //};

            return new ObjectResult(root);
        }

        #region RECEIVING OBJECT
        public class Rootobject
        {
            public Environment Environment { get; set; }
            public Event[] Events { get; set; }
        }

        public class Environment
        {
            public string MachineName { get; set; }
            public string SessionId { get; set; }
            public string FxProfile { get; set; }
            public bool IsDebugging { get; set; }
            public Metrologversion MetroLogVersion { get; set; }
        }

        public class Metrologversion
        {
            public int Major { get; set; }
            public int Minor { get; set; }
            public int Build { get; set; }
            public int Revision { get; set; }
            public int MajorRevision { get; set; }
            public int MinorRevision { get; set; }
        }

        public class Event
        {
            public int SequenceID { get; set; }
            public string Level { get; set; }
            public string Logger { get; set; }
            public string Message { get; set; }
            public DateTime TimeStamp { get; set; }
            public object Exception { get; set; }
            public object ExceptionWrapper { get; set; }
        }
        #endregion


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
