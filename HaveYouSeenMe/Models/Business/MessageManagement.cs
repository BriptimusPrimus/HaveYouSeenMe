using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveYouSeenMe.DAO;

namespace HaveYouSeenMe.Models.Business
{
    public class MessageManagement
    {
        private IMessageDao Dao;

        public MessageManagement()
        {
            Dao = new MessageDao();
        }

        public MessageManagement(IMessageDao messageDao)
        {
            Dao = messageDao;
        }

        public Message GetById(int id)
        {
            Message m = Dao.GetMessageById(id);
            return m;
        }

        //send new message = create new message and persist in DB
        public Message SendNew(Message message)
        {
            Message result = null;

            //
            // Business rules validations
            //

            if (string.IsNullOrEmpty(message.Message1))
            {
                throw new ApplicationException("Can not send an empty message");
            }

            //
            // Business rules data values
            //

            //date must be current
            message.MessageDate = DateTime.Now;

            //save the new data
            try
            {
                result = Dao.Save(message);
            }
            catch (ApplicationException Ex)
            {
                //pass the ball
                throw Ex;
            }
            catch (Exception Ex)
            {
                //log exception e

                //throw exception to indicate fail
                throw new ApplicationException("There was a problem trying to send the message, please try later");              
            }

            //succeed
            return result;
        }

        public bool Delete(int MessageID)
        { 
            //delet data from database
            try 
            { 
                //make sure message exists
                Message message = Dao.GetMessageById(MessageID);
                if (message == null)
                {
                    throw new ApplicationException("Inexistent record");
                }
                Dao.Delete(message);
            }
            catch (ApplicationException Ex)
            {
                //pass the ball
                throw Ex;
            }
            catch (Exception Ex)
            {
                //log exception e

                //throw exception to indicate fail
                throw new ApplicationException("Data base error");
            }

            //succeed
            return true;
        }

        public IEnumerable<Message> GetUserMessages(int UserID)
        {
            IEnumerable<Message> list = null;
            list = Dao.GetUserMessages(UserID);
            if (list == null)
            {
                return new Message[0];
            }
            return list;
        }

        public IEnumerable<Message> GetUserMessages(string UserName)
        {
            IEnumerable<Message> list = null;
            list = Dao.GetUserMessages(UserName);
            if (list == null)
            {
                return new Message[0];
            }
            return list;
        }
    }
}