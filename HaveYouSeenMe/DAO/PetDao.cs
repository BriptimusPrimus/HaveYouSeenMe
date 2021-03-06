﻿using System;
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

        public override void Delete(Pet entity)
        {
            Context.Pets.Remove(entity);
            Context.SaveChanges();            
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
    }
}