using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveYouSeenMe.Models;

namespace HaveYouSeenMe.DAO
{
    public class PetDao : EntityDao<Pet>, IPetDao
    {
        public override Pet Save(Pet entity)
        {
            Context.Pets.Add(entity);           
            Context.SaveChanges();
            return entity;
        }

        public override Pet Update(Pet entity)
        {
            //make sure pet exists
            Pet original = Context.Pets.Find(entity.PetID);
            if (original == null)
            {
                throw new ApplicationException("Inexistent record");
            }                

            //modify only updatable fields
            original.PetAgeYears = entity.PetAgeYears;
            original.PetAgeMonths = entity.PetAgeMonths;
            original.StatusID = entity.StatusID;
            original.LastSeenOn = entity.LastSeenOn;
            original.LastSeenWhere = entity.LastSeenWhere;
            original.Notes = entity.Notes;

            //save changes
            Context.SaveChanges();

            return original;
        }

        public override void Delete(Pet entity)
        {
            Context.Pets.Remove(entity);
            Context.SaveChanges();            
        }

        public Pet GetPetById(int id)
        {
            Pet p = null;
            p = Context.Pets.SingleOrDefault(x => x.PetID == id);
            return p;
        }

        public Pet GetPetByName(string name)
        {
            Pet p = null;
            p = Context.Pets.SingleOrDefault(x => x.PetName.Contains(name));            
            return p;            
        }

        public IEnumerable<Pet> GetPets()
        {
            IEnumerable<Pet> result = null;            
            result = Context.Pets;            
            return result.ToArray();
        }

        public IEnumerable<Pet> GetPetsWithStatus(Status status)
        {
            IEnumerable<Pet> result = null;
            result = from pet in Context.Pets
                        where pet.Status.StatusID == status.StatusID
                        select pet;                        
            return result.ToArray();
        }

        public IEnumerable<Pet> GetPetsWithStatus(string status)
        {
            IEnumerable<Pet> result = null;
            result = from pet in Context.Pets
                     where pet.Status.Description == status.ToLower()
                     select pet;
            return result.ToArray();
        }

        public IEnumerable<Pet> GetPetsFromOwner(string UserName)
        {
            IEnumerable<Pet> result = null;
            result = from pet in Context.Pets
                     where pet.UserProfile.UserName == UserName.ToLower()
                     select pet;                           
            return result;
        }

        public UserProfile GetUserByName(string UserName)
        {
            UserProfile result = null;

            try
            {
                result = Context.UserProfiles.SingleOrDefault
                    (x => x.UserName == UserName.ToLower());
            }
            catch (InvalidOperationException)
            {
                return null;
            }

            return result;
        }

        public Status GetStatus(string description)
        {
            Status s = null;
            s = Context.Status.SingleOrDefault(x => x.Description.Contains(description));
            return s;
        }

        public Status GetStatus(int id)
        {
            Status s = null;
            s = Context.Status.Find(id);
            return s;
        }
    }
}