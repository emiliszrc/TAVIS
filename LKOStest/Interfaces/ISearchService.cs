using LKOStest.Dtos;

namespace LKOStest.Interfaces
{
    public interface ISearchService
    {
        public SearchResponse SearchForLocation(string location);
    }
}
