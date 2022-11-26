using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Checkout.Custom.Common.Model;

namespace Checkout.Custom.PaymentGateway.Services.Services
{
    public interface ICardsService
    {
        bool Validate(CardDetails cardDetails);
        string Encrypt(string cardNumber);
        string Decrypt(string cardNumber);
    }

    public class CardsService : ICardsService
    {
        private const string Salt = "sblw-3hn8-sqoy19";

        public bool Validate(CardDetails cardDetails)
        {
            if (cardDetails == null || string.IsNullOrEmpty(cardDetails.CardNumber)) return false;

            var monthCheck = new Regex(@"^(0[1-9]|1[0-2])$");
            var yearCheck = new Regex(@"^20[0-9]{2}$");
            var cvvCheck = new Regex(@"^\d{3}$");

            int sumOfDigits = cardDetails.CardNumber.Where((e) => e >= '0' && e <= '9')
                    .Reverse()
                    .Select((e, i) => ((int)e - 48) * (i % 2 == 0 ? 1 : 2))
                    .Sum((e) => e / 10 + e % 10);

            var cardNumberIsValid = sumOfDigits % 10 == 0;
            if (!cardNumberIsValid)
                return false;
            if (string.IsNullOrEmpty(cardDetails.Cvv) || !cvvCheck.IsMatch(cardDetails.Cvv))
                return false;

            if ((string.IsNullOrEmpty(cardDetails.ExpirationMonth) || !monthCheck.IsMatch(cardDetails.ExpirationMonth)) || (string.IsNullOrEmpty(cardDetails.ExpirationYear) || !yearCheck.IsMatch(cardDetails.ExpirationYear)))
                return false;

            var year = int.Parse(cardDetails.ExpirationYear);
            var month = int.Parse(cardDetails.ExpirationMonth);
            var lastDateOfExpiryMonth = DateTime.DaysInMonth(year, month);
            var cardExpiry = new DateTime(year, month, lastDateOfExpiryMonth, 23, 59, 59);

            return (cardExpiry > DateTime.Now && cardExpiry < DateTime.Now.AddYears(6));
        }

        public string Encrypt(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber)) return string.Empty;
            var inputArray = Encoding.UTF8.GetBytes(cardNumber);
            var tripleDES = TripleDES.Create();
            tripleDES.Key = Encoding.UTF8.GetBytes(Salt);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            var cryptoTransform = tripleDES.CreateEncryptor();
            var resultArray = cryptoTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public string Decrypt(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber)) return string.Empty;
            var inputArray = Convert.FromBase64String(cardNumber);
            var tripleDES = TripleDES.Create();
            tripleDES.Key = Encoding.UTF8.GetBytes(Salt);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            var cryptoTransform = tripleDES.CreateDecryptor();
            var resultArray = cryptoTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return Encoding.UTF8.GetString(resultArray);
        }
    }
}
