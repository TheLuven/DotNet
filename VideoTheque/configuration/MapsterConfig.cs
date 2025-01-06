using Mapster;
using VideoTheque.DTOs;
using VideoTheque.ViewModels;

namespace VideoTheque.Configurations
{
    public static class MapsterConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<EmpruntPauvreDto, EmpruntViewModel>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Title, src => src.Title)
                .Map(dest => dest.Director, src => src.Director)
                .Map(dest => dest.FirstActor, src => src.FirstActor)
                .Map(dest => dest.Genre, src => src.Genre);

        }
    }
}
