using AutoMapper;

namespace TaskCollector.TaskCollectorHost
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {           
            CreateMap<Db.Model.User, Contract.Model.User>();

            CreateMap<Contract.Model.UserCreator, Db.Model.User>()
                .ForMember(s=>s.Password, s=>s.Ignore());

            CreateMap<Db.Model.Client, Contract.Model.Client>();

            CreateMap<Contract.Model.ClientCreator, Db.Model.Client>()
                .ForMember(s => s.Password, s => s.Ignore());

            CreateMap<Contract.Model.MessageCreator, Db.Model.Message>();
            CreateMap<Db.Model.Message, Contract.Model.Message>();

            CreateMap<Contract.Model.MessageStatusCreator, Db.Model.MessageStatus>();
            CreateMap<Db.Model.MessageStatus, Contract.Model.MessageStatus>();

            CreateMap<Db.Model.UserHistory, Contract.Model.UserHistory>();
            CreateMap<Db.Model.ClientHistory, Contract.Model.ClientHistory>();
            CreateMap<Db.Model.MessageHistory, Contract.Model.MessageHistory>();
            CreateMap<Db.Model.MessageStatusHistory, Contract.Model.MessageStatusHistory>();

        }
    }
}
