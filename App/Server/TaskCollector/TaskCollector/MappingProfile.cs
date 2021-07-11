using AutoMapper;

namespace TaskCollector.TaskCollectorHost
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<TreeCreator, Tree>()
            //    .ForMember(s => s.Id, s => s.MapFrom(c => Helper.GenerateGuid(new string[] { c.Name })))
            //    .ForMember(s => s.VersionDate, s => s.MapFrom(c => DateTimeOffset.Now));

            //CreateMap<TreeUpdater, Tree>()
            //    .ForMember(s => s.Id, s => s.MapFrom(c => Helper.GenerateGuid(new string[] { c.Name })))
            //    .ForMember(s => s.VersionDate, s => s.MapFrom(c => DateTimeOffset.Now));

            CreateMap<Db.Model.User, Contract.Model.User>();

            CreateMap<Contract.Model.UserCreator, Db.Model.User>()
                .ForMember(s=>s.Password, s=>s.Ignore());

            CreateMap<Db.Model.Client, Contract.Model.Client>();

            CreateMap<Contract.Model.ClientCreator, Db.Model.Client>()
                .ForMember(s => s.Password, s => s.Ignore());

            CreateMap<Contract.Model.MessageCreator, Db.Model.Message>();
            CreateMap<Db.Model.Message, Contract.Model.Message>();

            //CreateMap<TreeItem, TreeItemModel>();

            //CreateMap<FormulaCreator, Formula>();

            //CreateMap<FormulaUpdater, Formula>();

            //CreateMap<Formula, FormulaModel>();
            //CreateMap<TreeHistory, TreeHistoryModel>();
            //CreateMap<TreeItemHistory, TreeItemHistoryModel>();
            //CreateMap<FormulaHistory, FormulaHistoryModel>();
        }
    }
}
