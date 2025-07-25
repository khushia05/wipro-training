using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Demo_Standard_C_Sharp_features
{
    public class Employee
    {
        //Help an HR department build a lightweight employee information system using C# 7.0 features only
        // Following are some user stories from a HR prespective:
        // 
        //Store employee info( name, age, role)
        //Filter employee based on role( using pattern matching)
        //Extract and display just specific values(using tuples & Deconstruction)
        //calculate experience using Local function
        //Extend string data(Extension Methods)
        // use out variables to validate age
        //Ignore unwanted data using Discards
        //use concise methods(Expression - Bodied members)

        public string Name { get; set; }
        public string Role { get; set; }
        public string Info => $"{Name} is a {Role}";//Expression - Bodied members)
                                                    // public string GetEmployee => $""
    }
    public static class EmpStringExtensions// Extension Methods for mr -> Mr , deepak -> Deepak
    {
        public static string ToTitleCase(this string str) =>
            char.ToUpper(str[0]) + str.Substring(1).ToLower();
    }
}