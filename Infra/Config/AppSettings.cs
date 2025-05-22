namespace IntegrationsJsonPlaceHolder.Infra.Config
{
    public class AppSettings
    {
        public JsonPlaceHolderUrls? JsonPlaceHolderURL { get; set; }
    }

    public class JsonPlaceHolderUrls
    {
        public string? UrlOne { get; set; }
        public string? UrlTwo { get; set; }
    }
}
