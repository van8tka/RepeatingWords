using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace RepeatingWords
{
   public class GoogleAuthorithation
    {
        const string bundle = "cardsofwords.cardsofwords";
        public static void SaveCredentialsUser(string username, string password)
        {
            Account account = new Account()
            {
                Username = username,
            };
            account.Properties.Add("password", password);
            AccountStore.Create().Save(account, bundle);
        }

        public static string GetUserName(string bundle)
        {
            var acc = AccountStore.Create().FindAccountsForService(bundle).FirstOrDefault();
            return  acc!=null?acc.Username:null;
        }
        public static string GetPassword(string bundle)
        {
            var acc = AccountStore.Create().FindAccountsForService(bundle).FirstOrDefault();
            return acc != null ? acc.Properties["password"] : null;
        }
    }
}
