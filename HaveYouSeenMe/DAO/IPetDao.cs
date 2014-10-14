using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaveYouSeenMe.Models;

namespace HaveYouSeenMe.DAO
{
    public interface IPetDao : IDao<Pet>
    {
        Pet GetPetByName(string name);
        IEnumerable<Pet> GetPets();
        IEnumerable<Pet> GetPetsWithStatus(Status status);
    }
}
