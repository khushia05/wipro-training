
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Day11_SOLIDPrinciples_Csharp

{
    // splitting it into dedicated classes :SRP
    public class EmailSender
    {
        public void SendEmail(string message)
        {
            Console.WriteLine("Sending email:" + message);
        }
    }

    public class SmsSender
    {
        public void SendSms(string message)
        {
            Console.WriteLine("Sending Sms:" + message);
        }
    }
    //Now Email Sender and Sms Sender each have one responsibility

    // for Implementing OCP: we have to use interface to extend notification types without changing main logic

    public interface INotifier
    {
        public void Send(string message);
    }
    public class EmailNotifier : INotifier
    {
        public void Send(string message)
        {
            Console.WriteLine("Email send: " + message);
        }
    }

    public class SMSNotifier : INotifier
    {
        public void Send(String message)
        {
            Console.WriteLine("SMS send :" + message);
        }
    }

    // LSP Principles: Majorly Used for Injecting Sercices Reference

    public class NotificationProcessor
    {
        private readonly INotifier notifier;// Ref of Interface

        public NotificationProcessor(INotifier notifier)
        {
            this.notifier = notifier;
        }
        public void Process(String message)
        {
            notifier.Send(message);// Lsp works as we can use Email Notifier as well as SMS Notifier
        }
    }

    //ISP : Keeping all interface small and focused 
    public interface IEmailSend
    {
        void SendEmail(String message);
    }
    public interface ISmsSender
    {
        void SendSms(String message);
    }
    public class EmailSend : IEmailSend
    {
        // Here we are not forcing Sms sevice to implement Email Method and vice Versa
        public void SendEmail(string message)
        {
            Console.WriteLine("Email Semd" + message);
        }
    }
    //DIP : high - Level Modules Depends on Abstraction

}