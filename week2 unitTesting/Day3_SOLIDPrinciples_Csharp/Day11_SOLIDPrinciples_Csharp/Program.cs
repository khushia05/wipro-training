using Day11_SOLIDPrinciples_Csharp;
using System;
using static Day11_SOLIDPrinciples_Csharp.EmailNotifier;
class Program : NotificationProcessor
// Ex of bad design
{
    public Program(INotifier notifier) : base(notifier)
    {
    }

    static void Main()
    {
        //NotificaionService service = new NotificaionService();
        //service.Send("email", "Hello this is a test mail...!!");
        Console.WriteLine(" After Implementing SOLID principles,here we are ");
        INotifier notifier = new EmailNotifier(); // same can be done for SMS
        NotificationProcessor processor = new NotificationProcessor(notifier);
        processor.Process("Welcome to the Notification APP");


    }
}

public class NotificaionService()
{
    public void Send(string type, string message)
    {
        if (type == "email")
        {
            Console.WriteLine("Sending Email !" + message);
        }
        else if (type == "sms")
        {
            Console.WriteLine("Sending SMS! " + message);

        }
    }
}


// Here SRP is voilated -> Sending mail and sending SMS maintain in a single class
// OCP violated -> (new logic breaks the old code)
// No Abstraction : Dependency in Version 