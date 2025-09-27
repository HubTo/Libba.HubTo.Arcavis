using Libba.HubTo.Arcavis.Application.Interfaces;
using AutoMapper;

namespace Libba.HubTo.Arcavis.Application.Services;
public class ArcavisMapper : IArcavisMapper
{
    private readonly IMapper _mapper;

    public ArcavisMapper(IMapper mapper)
    {
        _mapper = mapper;
    }

    public TDestination Map<TDestination>(object source)
    {
        return _mapper.Map<TDestination>(source);
    }

    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
    {
        return _mapper.Map(source, destination);
    }

    public IEnumerable<TDestination> MapEnumerable<TDestination>(IEnumerable<object> source)
    {
        return _mapper.Map<IEnumerable<TDestination>>(source);
    }
}
