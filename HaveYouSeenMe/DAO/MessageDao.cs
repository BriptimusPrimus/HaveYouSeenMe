using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveYouSeenMe.Models;

namespace HaveYouSeenMe.DAO
{
    public class MessageDao : EntityDao<Message>, IMessageDao
    {
        public override Message Save(Message entity)
        {        
            Context.Messages.Add(entity);
            Context.SaveChanges();
            return entity;
        }

        public override Message Update(Message entity)
        {
            throw new NotImplementedException();
        }

        public override void Delete(Message entity)
        {
            Context.Messages.Remove(entity);
            Context.SaveChanges();
        }

        public Message GetMessageById(int id)
        {
            Message m = null;
            m = Context.Messages.SingleOrDefault(x => x.MessageID == id);
            return m;
        }

        public IEnumerable<Message> GetMessages()
        {
            IEnumerable<Message> result = null;
            result = Context.Messages;
            return result.ToArray();
        }

        public IEnumerable<Message> GetUserMessages(int id)
        {
            IEnumerable<Message> result = null;
            result = from message in Context.Messages
                     where message.UserId == id
                     select message;
            return result.ToArray();
        }

        public IEnumerable<Message> GetUserMessages(string UserName)
        {
            IEnumerable<Message> result = null;
            result = from message in Context.Messages
                     where message.UserProfile.UserName == UserName.ToLower()
                     select message;
            return result.ToArray();
        }
    }
}