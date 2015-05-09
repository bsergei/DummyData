using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Example.DataGen;
using Example.Xml.Error;
using Example.Xml.Report;

namespace Example
{
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.Single,
        ConcurrencyMode = ConcurrencyMode.Single,
        IncludeExceptionDetailInFaults = true)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ExampleRestService : IExampleRestService
    {
        private const string DatePattern = "yyyyMMdd";
        private const string DatePatternGroupName = "Date";
        private const string GuidPatternGroupName = "Guid";
        private const string AuthFileNamesRegexPattern = @"ExampleRestAuthToken_(?<Date>\d{8})_(?<Guid>[\da-f]{8}-[\da-f]{4}-[\da-f]{4}-[\da-f]{4}-[\da-f]{12})";

        static ExampleRestService()
        {
            CleanupObsoleteAuthTokens();
        }

        private static void CleanupObsoleteAuthTokens()
        {
            var currDate = DateTime.Today.ToString(DatePattern);
            foreach (var file in Directory.GetFiles(Path.GetTempPath(), GetAuthTokenFilesWildcard()))
            {
                var match = Regex.Match(file, AuthFileNamesRegexPattern);
                if (match.Success)
                {
                    if (match.Groups[DatePatternGroupName].Value != currDate)
                    {
                        try
                        {
                            File.Delete(file);
                        }
                        catch
                        {
                            // No matter.
                        }
                        
                    }
                }
            }
        }

        private static string GetAuthTokenFilesWildcard()
        {
            return GenerateStringUsingMatchGroups(
                AuthFileNamesRegexPattern,
                new Dictionary<string, string>()
                    {
                        {DatePatternGroupName, String.Empty.PadRight(DatePattern.Length, '?')},
                        {GuidPatternGroupName, "*"},
                    });
        }

        private static string GetAuthTokenFilePath(string authToken)
        {
            return Path.Combine(Path.GetTempPath(), GetAuthTokenFileName(authToken));
        }

        private static string GetAuthTokenFileName(string authToken)
        {
            return GenerateStringUsingMatchGroups(
                AuthFileNamesRegexPattern,
                new Dictionary<string, string>()
                    {
                        {DatePatternGroupName, DateTime.Today.ToString(DatePattern)},
                        {GuidPatternGroupName, authToken},
                    });
        }

        private static string GenerateStringUsingMatchGroups(string pattern, IDictionary<string, string> substs)
        {
            var parser = new RegexParser.RegexParser(pattern);
            var regex = parser.Root;
            var visitor = new SyntaxGroupReplaceVisitor(substs);
            regex.Accept(visitor);
            var result = visitor.ToString();
            return result;
        }

        public Stream Authenticate(string userid, string password, string env)
        {
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
            Console.WriteLine("At {0}: {1}", DateTime.Now, WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri);

            // Magic value: "baduser".
            if (String.Equals(userid, "baduser", StringComparison.CurrentCultureIgnoreCase))
            {
                Console.WriteLine("Bad user is trying to authenticate");
                var ms = new MemoryStream();
                XmlSerializer errorsXmlSerializer = new XmlSerializer(typeof(string));
                errorsXmlSerializer.Serialize(ms, "ERROR:Unable to authenticate the user in LDAP. Check whether correct User ID and password is passed");
                ms.Position = 0;
                return ms;
            }

            var newGuid = Guid.NewGuid().ToString();
            var principal = new Principal
                {
                    Environment = env,
                    UserId = userid,
                    SessionId = newGuid
                };

            var fileName = GetAuthTokenFilePath(newGuid);
            using (var authFile = File.CreateText(fileName))
            {
                new XmlSerializer(typeof (Principal)).Serialize(authFile, principal);
            }

            Console.WriteLine("Authenticated new user: " + newGuid);

            var stream = new MemoryStream();
            new XmlSerializer(typeof (string)).Serialize(stream, newGuid);
            stream.Position = 0;
            return stream;
        }

        public Stream GetReport(string reportId, string asofdate, string sessionId)
        {
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
            Console.WriteLine("At {0}: {1}", DateTime.Now, WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri);

            if (!File.Exists(GetAuthTokenFilePath(sessionId)))
            {
                Console.WriteLine("Not authenticated: " + (sessionId ?? "[Empty]"));
                var ms = new MemoryStream();
                var errs = new Errors { ErrorsArray = new[] { new Error { Type = "InvalidSessionFault" } } };
                XmlSerializer errorsXmlSerializer = new XmlSerializer(typeof(Errors));
                errorsXmlSerializer.Serialize(ms, errs);
                ms.Position = 0;
                return ms;
            }

            Tree reportData;
            switch (reportId.ToUpperInvariant())
            {
                case "REPORT1":
                    reportData = Report1.Create(asofdate);
                    break;

                case "REPORT2":
                    reportData = Report2.Create(asofdate);
                    break;

                case "REPORT3":
                    reportData = Report3.Create(asofdate);
                    break;
                default:
                    Console.WriteLine("Unknown report requested.");
                    reportData = null;
                    break;
            }

            var ms2 = new MemoryStream();
            XmlSerializer reportDataXmlSerializer = new XmlSerializer(typeof (Tree));
            reportDataXmlSerializer.Serialize(ms2, reportData);
            ms2.Position = 0;
            return ms2;
        }
    }
}