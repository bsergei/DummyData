namespace Example
{
    public static class UriTemplates
    {
        public const string Authenticate = "/auth/user/{userid}/pass/{password}/env/{env}";

        public const string GetReport = "/report_{reportId}/date/{asofdate}/sessionid/{sessionId}";
    }
}