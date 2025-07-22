using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo__Interfaces_Abstract_classes
{
    abstract class Payment
    //an abstract class is a class that cannot create an instance of itself and it should have at least one abstract method
    //We can have only one abstract class  for child class as C# doesnt suport multiple inheritance


    {
        //defining class memebers like properties and methods specially one absract method
        public string PaymentId { get; set; }

        public abstract void MakePayment(decimal amount);
        //here we are not defining the body of the method
        //This is an abstract method that must be implemented by derived classes

        public void GenrateReceipt()
        {
            //This is a concrete method that can be used by derived classes
            Console.WriteLine($"Receipt generated for payment ID: {PaymentId}");
        }
    }
}