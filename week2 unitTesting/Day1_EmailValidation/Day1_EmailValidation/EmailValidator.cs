
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;//regex

namespace Day1_EmailValidation
{
    public class EmailValidator
    {
        public bool IsEmailValid(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            //^ - Start of the string 
            //[^@\s]+ : One or more char that are not @ or Whistespace(\s)
            //$ : End of he string 
            //Mobile no: @"^[6-9]\d{9}$"
            //PIn code : @"^\d{6}$"
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);//defined inside Regex package
        }

    }
}