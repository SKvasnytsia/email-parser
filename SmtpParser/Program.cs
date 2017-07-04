using System.IO;
using System.Text;

namespace SmtpParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new MessageBuilder();
            var parser = new MessageParser(builder);
            var data = GetFileData();
            var message = parser.Parse(data);
            
        }

        private static string GetFileData()
        {
            return File.ReadAllText("email1.txt", Encoding.UTF8);
        }
    }
}