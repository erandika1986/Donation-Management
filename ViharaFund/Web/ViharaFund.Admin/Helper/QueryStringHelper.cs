namespace ViharaFund.Admin.Helper
{
    public static class QueryStringHelper
    {
        public static string ToQueryString<T>(T obj)
        {
            var properties = from p in typeof(T).GetProperties()
                             where p.GetValue(obj, null) != null
                             select $"{Uri.EscapeDataString(p.Name)}={Uri.EscapeDataString(p.GetValue(obj).ToString())}";

            return string.Join("&", properties);
        }
    }
}
