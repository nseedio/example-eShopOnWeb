namespace Seeds.CatalogItems
{
    internal static class ProductImageUrl
    {
        public static string ToUrl(this string imageFileName) => $"http://catalogbaseurltobereplaced/images/products/{imageFileName}";
    }
}
