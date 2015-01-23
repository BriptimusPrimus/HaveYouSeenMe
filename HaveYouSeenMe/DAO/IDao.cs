using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaveYouSeenMe.DAO
{
    public interface IDao<TEntity>
    {
        TEntity Save(TEntity entity);
        TEntity Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
