namespace GestionDeCompteSG
{
    public static class ApiEndpoints
    {
        private const string ApiBase = "api";

        public static class Comptes
        {
            private const string Base = $"{ApiBase}/comptes";
            public const string Get = $"{Base}/{{date}}";
            public const string GetCategoriesDedebits = $"{Base}/Categories/Debit/{{nombre}}";
        }
    }
}
