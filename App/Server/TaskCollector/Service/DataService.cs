using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskCollector.Service
{
    public class DataService: IDataService
    {
        private IServiceProvider _serviceProvider;
        private IMapper _mapper;
        public DataService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _mapper = _serviceProvider.GetRequiredService<IMapper>();
        }

        public async Task<IEnumerable<Contract.Model.User>> GetUsers(Contract.Model.UserFilter filter, CancellationToken token)
        {
            try
            {
                var repo = _serviceProvider.GetRequiredService<Db.Interface.IRepository<Db.Model.User>>();
                IEnumerable<Db.Model.User> result = await repo.GetAsync(new Db.Model.UserFilter { 
                   Size = filter.Size,
                   Page = filter.Page,
                   Selector = s=>s.Name.ToLower().Contains(filter.Name.ToLower())
                }, token);
                return result.Select(s=>_mapper.Map<Contract.Model.User>(s));
            }
            catch (DataServiceException)
            {
                throw;
            }
            catch (Db.Repository.RepositoryException ex)
            {
                throw new DataServiceException(ex.Message);
            }
        }

    }
}
