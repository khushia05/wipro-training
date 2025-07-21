using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;

namespace demo_text_to_speech
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            Console.WriteLine("enter the text to speech");
            string text = Console.ReadLine();
            synthesizer.Speak(text);
                }
    }
}
