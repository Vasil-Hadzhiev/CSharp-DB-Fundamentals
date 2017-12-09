namespace Instagraph.App
{
    using AutoMapper;
    using Instagraph.DataProcessor.Dto.Import;
    using Instagraph.Models;

    public class InstagraphProfile : Profile
    {
        public InstagraphProfile()
        {
            CreateMap<PictureDto, Picture>();
            CreateMap<UserDto, User>();
        }
    }
}