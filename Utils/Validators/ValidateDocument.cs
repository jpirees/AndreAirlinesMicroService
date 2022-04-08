using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Validators
{
    public class ValidateDocument
    {
        public static bool IsValidDocument(string document)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempDocument;
            string digit;
            int sum;
            int rest;

            document = document.Trim();
            document = document.Replace(".", "").Replace("-", "");

            if (document.Length != 11)
                return false;

            tempDocument = document[..9];
            sum = 0;

            for (int i = 0; i < 9; i++)
                sum += int.Parse(tempDocument[i].ToString()) * multiplicador1[i];

            rest = sum % 11;

            if (rest < 2)
                rest = 0;
            else
                rest = 11 - rest;

            digit = rest.ToString();
            tempDocument += digit;
            sum = 0;

            for (int i = 0; i < 10; i++)
                sum += int.Parse(tempDocument[i].ToString()) * multiplicador2[i];

            rest = sum % 11;

            if (rest < 2)
                rest = 0;
            else
                rest = 11 - rest;

            digit += rest.ToString();

            return document.EndsWith(digit);
        }

    }
}
