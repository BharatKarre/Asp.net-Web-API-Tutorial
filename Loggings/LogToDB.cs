namespace CollegeApp.Loggings
{
    public class LogToDB : ILogger
    {
        public void log(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("Log to DB method executed.");
            //Wrtie to own Logic to save the log
        }
    }
}
