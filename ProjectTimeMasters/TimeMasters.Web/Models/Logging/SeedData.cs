using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeMasters.Web.Data;

namespace TimeMasters.Web.Models.Logging
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                if(context.Log.Any())
                {
                    return;
                }

                var logs = new List<Log>
                {
                    new Log {},
                    new Log {}
                };
                logs.ForEach(l => context.Log.Add(l));
                context.SaveChanges();

                var env = new List<Environment>
                {
                    new Environment {LogID=3, SessionId="7287ce77-59a0-4925-9c40-4dea30c7d0ab", FxProfile="Microsoft® .NET Framework - 4.6.1586.0", IsDebugging=false, MachineName="ADJEKO_DESKTOP"},
                    new Environment {LogID=2, SessionId="55555e77-59a0-4925-9c40-4dea30c7d0ab", FxProfile="Microsoft® .NET Framework - 4.6.1586.0", IsDebugging=false, MachineName="ADJEKO_Mobile"}

                };
                env.ForEach(e => context.Environment.Add(e));
                context.SaveChanges();

                var eve = new List<Events>
                {
                    new Events {LogID=3, SequenceID=1, Level="Info", Logger="TestProgramm", Message="Fucked Somethin up", TimeStamp=DateTime.Parse("04.12.1992") },
                    new Events {LogID=2, SequenceID=2, Level="Error", Logger="FuckedProgramm", Message="FATAL FUCK UP", TimeStamp=DateTime.Parse("12.04.2006") }
                };
                eve.ForEach(e => context.Events.Add(e));
                context.SaveChanges();

                var metro = new List<MetroLogVersion>
                {
                    new MetroLogVersion {EnvironmentID=1, Build=1, Major=1, MajorRevision=0, Minor=1, MinorRevision=0, Revision=0 },
                    new MetroLogVersion {EnvironmentID=2, Build=2, Major=2, MajorRevision=0, Minor=1, MinorRevision=0, Revision=0 },
                };
                metro.ForEach(m => context.MetroLogVersion.Add(m));
                context.SaveChanges();

                var ex = new List<TimeMasters.Web.Models.Logging.Exception>
                {
                    new Exception {EventsID=1, Data="", HelpLint="", HResult="", InnerException="", Message="fuckedSomethingUpException", Source="yo mama", StackTrace="trace to yo mama", TargetSite="" },
                    new Exception {EventsID=2, Data="", HelpLint="", HResult="", InnerException="", Message="FUCKEDException", Source="FUCK", StackTrace="trace to FUCK", TargetSite="" }
                };
                ex.ForEach(e => context.Exception.Add(e));
                context.SaveChanges();

                var exw = new List<ExceptionWrapper>
                {
                    new ExceptionWrapper {EventsID=1, AsString="fucked something up", Hresult=1, TypeName="" },
                    new ExceptionWrapper {EventsID=2, AsString="FUCKED", Hresult=2, TypeName="" },
                };
                exw.ForEach(e => context.ExceptionWrapper.Add(e));
                context.SaveChanges();
            }
        }
    }
}
