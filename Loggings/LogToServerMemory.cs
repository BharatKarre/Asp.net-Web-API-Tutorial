namespace CollegeApp.Loggings
{
    public class LogToServerMemory : ILogger
    {
        public void log(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("Log to Server Memory method executed.");
            //Wrtie to own Logic to save the log
        }
    }
}
