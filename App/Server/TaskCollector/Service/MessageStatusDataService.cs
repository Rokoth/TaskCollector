﻿using System;
using System.Linq.Expressions;
using TaskCollector.Contract.Model;

namespace TaskCollector.Service
{
    public class MessageStatusDataService : DataService<
        Db.Model.MessageStatus,
        Contract.Model.MessageStatus,
        Contract.Model.MessageStatusFilter,
        Contract.Model.MessageStatusCreator,
        Contract.Model.MessageStatusUpdater>
    {
        //protected override Func<Db.Model.MessageStatus, Contract.Model.MessageStatusFilter, bool> GetFilter =>
        //     (s, t) => s.Name.ToLower().Contains(t.Name.ToLower());

        protected override Func<Contract.Model.MessageStatus, Contract.Model.MessageStatus> EnrichFunc => null;

        public MessageStatusDataService(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        protected override Expression<Func<Db.Model.MessageStatus, bool>> GetFilter(MessageStatusFilter filter)
        {
            throw new NotImplementedException();
        }

        protected override string defaultSort => "Name";
    }
}
