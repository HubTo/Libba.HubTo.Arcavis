namespace Libba.HubTo.Arcavis.Application.Interfaces;

public interface IArcavisMapper
{
    TDestination Map<TDestination>(object source);
    TDestination Map<TSource, TDestination>(TSource source, TDestination destination);
    IEnumerable<TDestination> MapEnumerable<TDestination>(IEnumerable<object> source);
}
