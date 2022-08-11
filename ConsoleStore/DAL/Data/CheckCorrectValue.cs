using System;
using System.Text.RegularExpressions;

namespace DAL.Data
{
    /// <summary>
    /// Checks user data for correctness
    /// </summary>
    public class CheckCorrectValue
    {
        public bool GetPasswordStrength(string password)
        {
            int ResultMark = 0;
            if (string.IsNullOrEmpty(password))
            {
                Console.WriteLine($"Sorry, the value {password} is not appropriate");
                return false;
            }
            //+1 point correct length 
            if (password.Length >= 6)
            {
                ResultMark++;
            }
            //+1 lowercase letter
            if (Regex.Match(password, "[a-z]").Success)
            {
                ResultMark++;
            }
            //+1 uppercase letter
            if (Regex.Match(password, "[A-Z]").Success)
            {
                ResultMark++;
            }
            //+1 for numbers in the password
            if (Regex.Match(password, "[0-9]").Success)
            {
                ResultMark++;
            }
            //+1 for special symbol
            if (Regex.Match(password, "[!-+]").Success)
            {
                ResultMark++;
            }

            if (ResultMark >= 5)
            {
                return true;
            }

            Console.WriteLine($"Sorry, the value {password} is not appropriate");
            return false;
        }

        public bool CheckPhoneNumber(string phone)
        {
            if (!string.IsNullOrEmpty(phone) && Regex.IsMatch(phone, "[0-9]{3}-[0-9]{3}-[0-9]{4}"))
            {
                return true;
            }

            Console.WriteLine($"Sorry, the value {phone} is not appropriate | Write in format like this: 111-111-1111");
            return false;
        }

        public bool CheckLocation(string location)
        {
            if (!string.IsNullOrEmpty(location) && !Regex.IsMatch(location, "[!-+]") && !Regex.IsMatch(location, "[0-9]")
                && Regex.IsMatch(location, @"\w*,\w*"))
                return true;

            Console.WriteLine($"Sorry, the value {location} is not appropriate | Correct format: Kyiv,Ukraine");
            return false;
        }

        public bool CheckNameOrSurname(string name)
        {
            if (!string.IsNullOrEmpty(name) && !Regex.IsMatch(name, "[!-+]")
                                                             && !Regex.IsMatch(name, "[0-9]"))
                return true;

            Console.WriteLine($"Sorry, the value {name} is not appropriate");
            return false;
        }

        public bool CheckEmail(string email)
        {
            if (!string.IsNullOrEmpty(email) && Regex.IsMatch(email, @"\w*@gmail.com$"))
                return true;

            Console.WriteLine($"Sorry, the value {email} is not appropriate | Correct format: hello@gmail.com");
            return false;
        }

        public bool CheckAllPositions(string name, string surname, string location, string email,
                                string password, string phone)
        {
            if (CheckNameOrSurname(name) && CheckNameOrSurname(surname) &&
                CheckLocation(location) && CheckEmail(email) &&
                GetPasswordStrength(password) && CheckPhoneNumber(phone))
                return true;

            return false;
        }
    }
}
