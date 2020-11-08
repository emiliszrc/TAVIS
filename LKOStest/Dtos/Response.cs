using System;
using System.Collections.Generic;

namespace LKOStest.Dtos
{ 
    //public class Small
    //{
    //    public string width { get; set; }
    //    public string url { get; set; }
    //    public string height { get; set; }
    //}

    //public class Thumbnail
    //{
    //    public string width { get; set; }
    //    public string url { get; set; }
    //    public string height { get; set; }
    //}

    //public class Original
    //{
    //    public string width { get; set; }
    //    public string url { get; set; }
    //    public string height { get; set; }
    //}

    //public class Large
    //{
    //    public string width { get; set; }
    //    public string url { get; set; }
    //    public string height { get; set; }
    //}

    //public class Medium
    //{
    //    public string width { get; set; }
    //    public string url { get; set; }
    //    public string height { get; set; }
    //}

    //public class Images
    //{
    //    public Small small { get; set; }
    //    public Thumbnail thumbnail { get; set; }
    //    public Original original { get; set; }
    //    public Large large { get; set; }
    //    public Medium medium { get; set; }
    //}

    //public class User
    //{
    //    public object user_id { get; set; }
    //    public string member_id { get; set; }
    //    public string type { get; set; }
    //}

    //public class Photo
    //{
    //    public Images images { get; set; }
    //    public bool is_blessed { get; set; }
    //    public DateTime uploaded_date { get; set; }
    //    public string caption { get; set; }
    //    public string id { get; set; }
    //    public string helpful_votes { get; set; }
    //    public DateTime published_date { get; set; }
    //    public User user { get; set; }
    //}

    public class Attractions
    {
        public string activities { get; set; }
        public string attractions { get; set; }
        public string nightlife { get; set; }
        public string shopping { get; set; }
        public string total { get; set; }
    }

    public class Restaurants
    {
        public string total { get; set; }
    }

    public class Accommodations
    {
        public string hotels { get; set; }
        public string bbs_inns { get; set; }
        public string others { get; set; }
        public string total { get; set; }
    }

    public class CategoryCounts
    {
        public Attractions attractions { get; set; }
        public Restaurants restaurants { get; set; }
        public Accommodations accommodations { get; set; }
        public string neighborhoods { get; set; }
        public string airports { get; set; }
    }

    public class Subcategory
    {
        public string key { get; set; }
        public string name { get; set; }
    }

    public class Ancestor
    {
        public List<Subcategory> subcategory { get; set; }
        public string name { get; set; }
        public object abbrv { get; set; }
        public string location_id { get; set; }
    }

    public class Category
    {
        public string key { get; set; }
        public string name { get; set; }
    }

    public class Subcategory2
    {
        public string key { get; set; }
        public string name { get; set; }
    }

    public class SpecialOffers
    {
        public List<object> desktop { get; set; }
        public List<object> mobile { get; set; }
    }

    public class AddressObj
    {
        public string street1 { get; set; }
        public string street2 { get; set; }
        public string city { get; set; }
        public object state { get; set; }
        public string country { get; set; }
        public string postalcode { get; set; }
    }

    public class Cuisine
    {
        public string key { get; set; }
        public string name { get; set; }
    }

    public class EstablishmentType
    {
        public string key { get; set; }
        public string name { get; set; }
    }

    public class ResultObject
    {
        public string location_id { get; set; }
        public string name { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string num_reviews { get; set; }
        //public string timezone { get; set; }
        public string location_string { get; set; }
        //public Photo photo { get; set; }
        //public List<object> awards { get; set; }
        //public string doubleclick_zone { get; set; }
        //public string preferred_map_engine { get; set; }
        //public string geo_type { get; set; }
        public CategoryCounts category_counts { get; set; }
        public List<object> nearby_attractions { get; set; }
        public string description { get; set; }
        public string web_url { get; set; }
        public List<Ancestor> ancestors { get; set; }
        public Category category { get; set; }
        public List<Subcategory2> subcategory { get; set; }
        //public bool is_jfy_enabled { get; set; }
        //public List<object> nearest_metro_station { get; set; }
        public string geo_description { get; set; }
        //public bool has_restaurant_coverpage { get; set; }
        //public bool has_attraction_coverpage { get; set; }
        //public bool has_curated_shopping_list { get; set; }
        //public object distance { get; set; }
        //public object distance_string { get; set; }
        //public object bearing { get; set; }
        //public string rating { get; set; }
        //public bool? is_closed { get; set; }
        //public bool? is_long_closed { get; set; }
        public SpecialOffers special_offers { get; set; }
        public AddressObj address_obj { get; set; }
        public string address { get; set; }
        public bool? is_candidate_for_contact_info_suppression { get; set; }
        public List<object> amenities { get; set; }
        public string location_subtype { get; set; }
        public string shopping_type { get; set; }
        public string open_now_text { get; set; }
        public List<Cuisine> cuisine { get; set; }
        public List<EstablishmentType> establishment_types { get; set; }
    }

    public class SearchExplanations
    {
        public string mentioned_by_travelers { get; set; }
    }

    public class Span
    {
        public int start_index { get; set; }
        public int end_index { get; set; }
    }

    public class ReviewSnippet
    {
        public string snippet { get; set; }
        public List<Span> spans { get; set; }
        public string review_id { get; set; }
    }

    public class Datum
    {
        public string result_type { get; set; }
        public ResultObject result_object { get; set; }
        public string scope { get; set; }
        public bool is_top_result { get; set; }
        public SearchExplanations search_explanations { get; set; }
        public ReviewSnippet review_snippet { get; set; }
    }

    public class Metadata
    {
        public string scope { get; set; }
    }

    public class Sort
    {
        public string filter_key { get; set; }
        public string label { get; set; }
        public string locale_independent_label { get; set; }
        public bool selected { get; set; }
    }

    public class Tracking
    {
        public string search_id { get; set; }
    }

    public class Paging
    {
        public string results { get; set; }
        public string total_results { get; set; }
    }

    public class SearchResponse
    {
        public List<Datum> data { get; set; }
        public Metadata metadata { get; set; }
        //public List<Sort> sort { get; set; }
        //public bool partial_content { get; set; }
        //public Tracking tracking { get; set; }
        //public Paging paging { get; set; }
    }
}
