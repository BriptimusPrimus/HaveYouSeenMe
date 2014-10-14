using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaveYouSeenMe.DAO
{
    public interface IDaoFactory
    {
        IMessageDao GetMessageDao();
        IPetDao GetPetDao();
        IPetType GetPetType();
        ISetting GetSetting();
        IStatus GetStatus();
    }
}
