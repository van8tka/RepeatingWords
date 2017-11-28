using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using System.IO;
using System.Threading.Tasks;
using RepeatingWords;
//using Google.Apis.Drive.v2;
//using Google.Apis.Auth.OAuth2;
using System.Threading;
using Google.Apis.Auth.OAuth2;

//using Google.Apis.Services;
//using Google.Apis.Util.Store;

[assembly: Dependency(typeof(RepeatingWords.Droid.GoogleDriveWorker))]

namespace RepeatingWords.Droid
{
    public class GoogleDriveWorker:IGoogleDriveWorker
    {

        //static string[] Scopes = { DriveService.Scope.DriveReadonly };
        //static string AppName = "CardsOfWords";


      

      
        public List<string> GetGoogleItems()
        {


            try { 

                }
                catch (Exception er)
                {
                    ErrorHandler.getError(er, "GoogleDriveWorker.GetGoogleItems");
                    throw;
                }
          
            return new List<string>();
        }

       
    }
}