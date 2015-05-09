using System;
using System.ServiceModel.Web;
using System.Windows.Forms;

namespace Example
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            var service = new ExampleRestService();
            var baseAddresses = new Uri("http://127.0.0.1:8880/ws");
            var webServiceHost = new WebServiceHost(service, baseAddresses);
            try
            {
                webServiceHost.Open();
                Console.WriteLine("Service started (" + baseAddresses + ").");
                Console.WriteLine("Press 'b' to copy base URL to clipboard.");
                Console.WriteLine("Press 'r' to copy Report URL to clipboard.");
                Console.WriteLine("Press 'a' to copy Authenticate URL to clipboard.");
                Console.WriteLine("Press any other key to exit...");

                bool wait;
                do
                {
                    wait = false;
                    switch (Char.ToLower(Console.ReadKey().KeyChar))
                    {
                        case 'b':
                            ClipboardSetText(baseAddresses.ToString());
                            Console.WriteLine();
                            Console.WriteLine("Base URL copied.");
                            wait = true;
                            break;

                        case 'r':
                            ClipboardSetText(baseAddresses + UriTemplates.GetReport);
                            Console.WriteLine();
                            Console.WriteLine("Report URL copied.");
                            wait = true;
                            break;

                        case 'a':
                            ClipboardSetText(baseAddresses + UriTemplates.Authenticate);
                            Console.WriteLine();
                            Console.WriteLine("Authenticate URL copied.");
                            wait = true;
                            break;
                    }
                } while (wait);
            }
            finally
            {
                webServiceHost.Close();
            }
        }

        private static void ClipboardSetText(string text)
        {
            int tryCount = 0;
            bool success = false;
            while (!success)
            {
                try
                {
                    Clipboard.SetText(text);
                    success = true;
                }
                catch (Exception)
                {
                    if (tryCount >= 20)
                        throw;

                    tryCount++;
                }
            }
        }
    }
}
