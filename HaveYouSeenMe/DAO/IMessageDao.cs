using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaveYouSeenMe.Models;

namespace HaveYouSeenMe.DAO
{
    public interface IMessageDao : IDao<Message>
    {
        Message GetMessageById(int id);
        IEnumerable<Message> GetMessages();
        IEnumerable<Message> GetUserMessages(int id);
        IEnumerable<Message> GetUserMessages(string UserName);
    }
}
