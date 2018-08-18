using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;

namespace DBPlatform_v1._0.Helpers
{
    public class GoogleSheetData
    {
        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "Google Sheets API .NET Quickstart";
        static UserCredential credential = null;

        static GoogleSheetData()
        {
            //var curpath = Server.GetPath()

            var path = System.Web.HttpContext.Current.Server.MapPath("~/Helpers/client_secret-from-vocabulary.json");
            /*var path2 = "~/Helpers/client_secret.json";*/
            //string clientSecrects = Microsoft.SqlServer.Server.MapPath("~/Files/")
            string jsonPath = @"C:\Windows\System32\client_secret.json";
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-dotnet-quickstart.json");
                try
                {
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.ToString());
                }

                //Console.WriteLine("Credential file saved to: " + credPath);
            }
        }
        private static string GetSheetIdByUrl(string url)
        {
            int startOfId = url.IndexOf("spreadsheets/d/");
            var Id = url.Substring(startOfId + 15);
            int endOfId = Id.IndexOf("/");
            if (endOfId == -1)
            {
                return Id;
            }
            return Id.Substring(0, endOfId);
        }
        public static IEnumerable<IEnumerable<Object>> getDataFromSheetlink(string gSheetUrl)
        {


            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define request parameters.
            String spreadsheetId = GetSheetIdByUrl(gSheetUrl);
            //"1vTKrYypwLznvVlj1vqlo5SmxY994jM0P3gSBopw3w6I";

            String range = "Sheet1!A:AI";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                service.Spreadsheets.Values.Get(spreadsheetId, range);

            // Prints the names and majors of students in a sample spreadsheet:
            //https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit example
            //https://docs.google.com/spreadsheets/d/1iKjVQRdRs_fxv1b4Y72EH3nS7RHR8mL5ikNfMb4yXAo/edit my sheet
            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;

            return values;
        }

    }
}