﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaveYouSeenMe.Models;

namespace HaveYouSeenMe.DAO
{
    public interface IMessageDao : IDao<Message>
    {
        IEnumerable<Message> GetMessages();
    }
}
