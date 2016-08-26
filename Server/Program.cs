namespace Server
{
    /// <summary>
    /// The main program class containing the main function.
    /// </summary>
    class Program
    {
        // WHY ARE JAVA PROGRAMMERS ALMOST BLIND - BECAUSE THEY DONT C#
        // BWAHAHAHAHAHHAAH

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The application arguments which the application
        /// does not use in any way.</param>
        static void Main(string[] args)
        {
            Server server = new Server();
            server.Start();
        }
    }
}
