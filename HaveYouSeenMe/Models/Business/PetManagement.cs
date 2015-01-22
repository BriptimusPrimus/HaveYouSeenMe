using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;
using HaveYouSeenMe.DAO;

namespace HaveYouSeenMe.Models.Business
{
    public class PetManagement
    {
        private IPetDao Dao;

        public PetManagement()
        {
            Dao = new PetDao();
        }

        public PetManagement(IPetDao petDao)
        {
            Dao = petDao;
        }

        public Pet GetById(int id)
        {
            Pet p = Dao.GetPetById(id);
            return p;
        }

        public Pet GetByName(string name)
        {
            Pet p = Dao.GetPetByName(name);
            return p;
        }

        public Pet CreateNew(Pet pet, string userName)
        {
            Pet result = null;

            //
            // Business rules validations
            //
            
            if (string.IsNullOrEmpty(pet.PetName))
            {
                throw new ApplicationException("Pet name is required");
            }


            //
            // Business rules data values
            //

            //Initial status of a new pet always "lost"
            pet.StatusID = 1;

            //Add pet to currently logged in user           
            UserProfile user = Dao.GetUserByName(userName);
            if (user == null)
            {
                throw new ApplicationException("User not logged in");
            }
            pet.UserId = user.UserId;


            //save the new data
            try
            {
                result = Dao.Save(pet);
            }
            catch (Exception Ex) //pokemon exception handling
            { 
                //log exception e

                //throw exception to indicate fail
                throw new ApplicationException("Data base error");                
            }

            //succeed
            return result;
        }

        public static void CreateThumbnail(string fileName, string filePath,
                                                   int thumbWi, int thumbHi,
                                                   bool maintainAspect)
        {
            // do nothing if the original is smaller than the designated
            // thumbnail dimensions
            var originalFile = Path.Combine(filePath, fileName);
            var source = Image.FromFile(originalFile);
            if (source.Width <= thumbWi && source.Height <= thumbHi) return;

            Bitmap thumbnail;
            try
            {
                int wi = thumbWi;
                int hi = thumbHi;

                if (maintainAspect)
                {
                    // maintain the aspect ratio despite the thumbnail size parameters
                    if (source.Width > source.Height)
                    {
                        wi = thumbWi;
                        hi = (int)(source.Height * ((decimal)thumbWi / source.Width));
                    }
                    else
                    {
                        hi = thumbHi;
                        wi = (int)(source.Width * ((decimal)thumbHi / source.Height));
                    }
                }

                thumbnail = new Bitmap(wi, hi);
                using (Graphics g = Graphics.FromImage(thumbnail))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.FillRectangle(Brushes.Transparent, 0, 0, wi, hi);
                    g.DrawImage(source, 0, 0, wi, hi);
                }

                var thumbnailName = Path.Combine(filePath, "thumbnail_" + fileName);
                thumbnail.Save(thumbnailName);
            }
            catch
            {

            }

        }

        public IEnumerable<Pet> GetMissing() 
        {
            IEnumerable<Pet> list = null;
            list = Dao.GetPets();
            return list;
        }

        public IEnumerable<Pet> GetPetsFromOwner(string UserName)
        {
            IEnumerable<Pet> list = null;
            list = Dao.GetPetsFromOwner(UserName);
            if (list == null)
            {
                return new Pet[0];
            }
            return list;
        }

    }
}