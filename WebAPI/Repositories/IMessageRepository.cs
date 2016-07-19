//-------------------------------------------------------------------------------------
// Author:      mbr
// Created:     7/19/2016 10:09:59 AM
// Copyright (c) white duck Gesellschaft für Softwareentwicklung mbH
//-------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace WebAPI.Repositories
{
    public interface IMessageRepository
    {
        string GetMessageById(Guid aId);
        IDictionary<Guid, string> GetAll();
        void InsertMesage(Guid aId, string aMessage);
        bool DeleteMessage(Guid aId);
        void UpdateMessage(Guid aId, string aMessage);
    }
}
