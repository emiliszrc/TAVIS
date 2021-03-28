using System.Collections.Generic;
using LKOStest.Dtos;
using LKOStest.Services;

namespace LKOStest.Interfaces
{
    public interface ISearchService
    {
        public SearchResponse SearchForLocation(string location);
        public List<HolidayResponse> SearchForHolidays(string countryCode, string year);
        public GeocodingResponse GetCountryCode(string latitude, string longitude);
        public AttractionResponse GetAttractionDetails(string locationId);
        public AttractionResponse GetRestaurantDetails(string locationId);
    }
}
