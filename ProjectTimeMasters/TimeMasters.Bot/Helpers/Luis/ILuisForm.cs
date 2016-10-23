using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeMasters.Bot.Helpers.Luis
{
    public interface ILuisForm
    {
        void TryResolveMissingInformation();
    }
}
