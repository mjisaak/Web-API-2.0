using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace WebAPI.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private static readonly IDictionary<Guid, string> mMessages = new ConcurrentDictionary<Guid, string>();

        #region Implementation of IMessageRepository

        public string GetMessageById(Guid aId)
        {
            return mMessages[aId];
        }

        public IDictionary<Guid, string> GetAll()
        {
            return mMessages;
        }

        public void InsertMesage(Guid aId, string aMessage)
        {
            mMessages.Add(aId, aMessage);
        }

        public bool DeleteMessage(Guid aId)
        {
           return mMessages.Remove(aId);
        }

        public void UpdateMessage(Guid aId, string aMessage)
        {
            mMessages[aId] = aMessage;
        }

        #endregion
    }
}