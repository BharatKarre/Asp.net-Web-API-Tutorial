namespace CollegeApp.Loggings
{
    public class LogToFile : ILogger
    {
        public void log(string message) 
        {
            Console.WriteLine(message);
            Console.WriteLine("Log to File method executed.");
            //Wrtie to own Logic to save the log
        }
    }
}
