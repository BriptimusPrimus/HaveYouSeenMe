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
        Pet GetPetById(int id);
        Pet GetPetByName(string name);
        IEnumerable<Pet> GetPets();
        IEnumerable<Pet> GetPetsWithStatus(Status status);
        IEnumerable<Pet> GetPetsWithStatus(string status);
        IEnumerable<Pet> GetPetsFromOwner(string UserName);
        UserProfile GetUserByName(string UserName);
        Status GetStatus(string description);
        Status GetStatus(int id);
    }
}
