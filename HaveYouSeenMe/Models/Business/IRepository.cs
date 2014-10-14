using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaveYouSeenMe.Models.Business
{
    public interface IRepository
    {
        Pet GetPetByName(string name);
    }
}
