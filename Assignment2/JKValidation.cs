using System;
using System.Text.RegularExpressions;

namespace Assignment2
{
    class JKValidation
    {
        // For each word, capitalize the first letter and lowercase letters for the rest
        public static string JKCapitalize(string value)
        {
            string[] words = (value + "").ToLower().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string result = "";
            foreach (var word in words)
            {
                result += " " + word.Substring(0, 1).ToUpper();
                result += word.Substring(1);
            }

            return result.Trim();
        }

        // Check that the entered code is exactly 9 characters long and contains 5 numbers
        public static bool JKMemberCodeValidation(string value)
        {            
            if (string.IsNullOrEmpty(value))
            {
                return true;
            }
            else
            {
                Regex pattern = new Regex(@"^([A-Za-z0-9]){9}$");
                if (pattern.IsMatch(value))
                {
                    int digitCount = 0;
                    foreach (char character in value)
                    {
                        if (char.IsDigit(character))
                        {
                            digitCount++;
                        }
                    }

                    return digitCount == 5;
                }
            }

            return false;
        }

        // Validate the phone number
        public static bool JKPhoneNumberValidation(string value)
        {
            Regex pattern = new Regex(@"^\d{3}[ \.]\d{3}[ \.]\d{4}$");

            return pattern.IsMatch(value) && (value[3] == value[7]);
        }

        // Validate the postal code and insert the space it it is missing
        public static bool JKUKPostalValidation(ref string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return true;
            }
            else
            {
                Regex pattern = new Regex(@"^[A-Za-z]{1,2}[0-9]([A-Za-z]|[0-9])? ?[0-9][A-Za-z]{2}$");

                if (pattern.IsMatch(value))
                {
                    value = value.ToUpper();

                    if (value.IndexOf(" ") == -1)
                    {
                        value = value.Insert(value.Length - 3, " ");
                    }

                    return true;
                }
            }
                
            return false;
        }
    }
}
