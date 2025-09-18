namespace MeetUp.EShop.Api.Cache
{
    public static class CacheKeys
    {
        public static string Products => "products_cache_key";
        public static string SingleProduct => "product_cache_key_";

        public static string Users => "users_cache_key";
        public static string SingleUser => "user_cache_key_";
        public static string UserLastOrder => "user_last_order_cache_key_";

        public static string Orders => "orders_cache_key";
        public static string SingleOrder => "order_cache_key_";
    }
}
