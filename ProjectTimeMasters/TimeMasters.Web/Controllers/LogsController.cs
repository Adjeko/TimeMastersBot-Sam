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
            for(int i = tmp.Count() - 6; i < tmp.Count(); i++)
            {
                Log l = tmp.ElementAt(i);
                l.Environment = await _context.Environment.SingleOrDefaultAsync(m => m.LogID == l.ID);
                l.Environment.MetroLogVersion = await _context.MetroLogVersion.SingleOrDefaultAsync(m => m.EnvironmentID == l.Environment.ID);

                l.Events = await _context.Events.SingleOrDefaultAsync(m => m.LogID == l.ID);
                l.Events.Exception = await _context.Exception.SingleOrDefaultAsync(m => m.EventsID == l.Events.ID);
                l.Events.ExceptionWrapper = await _context.ExceptionWrapper.SingleOrDefaultAsync(m => m.EventsID == l.Events.ID);
            }
            tmp = tmp.Reverse();
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

        [HttpPost]
        public async Task<IActionResult> AddLog([FromBody] LogReceiveMessage root)
        {
            Log tmp = new Log();

            MetroLogVersion mlv = new MetroLogVersion
            {
                Build = root.Environment.MetroLogVersion.Build,
                Major = root.Environment.MetroLogVersion.Major,
                MajorRevision = root.Environment.MetroLogVersion.MajorRevision,
                Minor = root.Environment.MetroLogVersion.Minor,
                MinorRevision = root.Environment.MetroLogVersion.MinorRevision,
                Revision = root.Environment.MetroLogVersion.Revision
            };

            Models.Logging.Environment env = new Models.Logging.Environment
            {
                MachineName = root.Environment.MachineName,
                FxProfile = root.Environment.FxProfile,
                IsDebugging = root.Environment.IsDebugging,
                SessionId = root.Environment.SessionId,
                MetroLogVersion = mlv,
            };

            tmp.Environment = env;

            Models.Logging.Exception ex = new Models.Logging.Exception
            {
                Message = root.Events.Exception.Message,
                Source = root.Events.Exception.Source,
                StackTrace = root.Events.Exception.StackTrace,
                HelpLink = root.Events.Exception.HelpLink,
                HResult = root.Events.Exception.HResult,
            };

            Models.Logging.ExceptionWrapper exw = new Models.Logging.ExceptionWrapper
            {
                AsString = root.Events.ExceptionWrapper.AsString,
                Hresult = root.Events.ExceptionWrapper.Hresult,
                TypeName = root.Events.ExceptionWrapper.TypeName
            };

            Events ev = new Events
            {
                Exception = ex,
                ExceptionWrapper = exw,
                Message = root.Events.Message,
                Level = root.Events.Level,
                Logger = root.Events.Logger,
                SequenceID = root.Events.SequenceID,
                TimeStamp = root.Events.TimeStamp,
            };

            tmp.Events = ev;

            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Log> ent = _context.Log.Add(tmp);
            await _context.SaveChangesAsync();

            Log returnLog = new Log
            {
                Environment = ent.Entity.Environment,
                Events = ent.Entity.Events
            };

            return new ObjectResult(returnLog);
        }

        #region RECEIVING OBJECT
        public class LogReceiveMessage
        {
            public Environment Environment { get; set; }
            public Event Events { get; set; }
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
            public Exception Exception { get; set; }
            public ExceptionWrapper ExceptionWrapper { get; set; }
        }

        public class Exception
        {
            //public string Data { get; set; }
            public string HelpLink { get; set; }
            public int HResult { get; set; }
            //public string InnerException { get; set; }
            public string Message { get; set; }
            public string Source { get; set; }
            public string StackTrace { get; set; }
            //public string TargetSite { get; set; }
        }

        public class ExceptionWrapper
        {
            public string AsString { get; set; }
            public int Hresult { get; set; }
            public string TypeName { get; set; }
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
