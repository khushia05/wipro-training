using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo__Interfaces_Abstract_classes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(" Getting started with Abstract Class and INterface");
            Console.WriteLine("--------------------------------------------------");
            //makesound class which with predefined methods which can be derived in child classes
            //Step 1: Create an abstract class
            //Step 2: Create an interface
            //Step 3: Implement the abstract class and interface in a derived class( Why? Because abstract classes cannot be instantiated directly)
            //Step 4: Create an instance of the derived class and call the methods

            Console.WriteLine(" Here is an Abstract class Demo ..!!!");
            Payment payment = new creditCardPayment(); // Creating an instance of the derived class
            payment.MakePayment(1000.00m); // Calling the abstract method implemented in the derived class
            payment.GenrateReceipt(); // Calling the concrete method from the abstract class


            Console.WriteLine("IMplementing Interface ");
            IPaymentGateway paymentGateway = new PayPalGateway(); // Creating an instance of the class that implements the interface
            paymentGateway.Pay(1500.00m); // Calling the Pay method from the interface
            paymentGateway.Refund(500.00m); // Calling the Refund method from the interface
            Console.WriteLine("--------------------------------------------------");

        }
    }

    //creating a derived class which inherits from the abstract class ie Payment
    class creditCardPayment : Payment //classes are derived from abstract class Payment
    {
        //implementing the abstract method MakePayment
        public override void MakePayment(decimal amount)
        {
            //implementation of the method
            Console.WriteLine($"Payment of {amount} made using Credit Card. Payment ID: {PaymentId}");
            PaymentId = Guid.NewGuid().ToString(); // Generate a unique Payment ID 
            //Guid.NewGuid() generates a new globally unique identifier (GUID) ex" TXN13254"
        }
    }
}