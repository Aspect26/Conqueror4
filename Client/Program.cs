using System;

namespace Client
{
    /// <summary>
    /// The main class of the program. Contains the main function and starts the application.
    /// </summary>
    static class Program
    {
        [STAThread]
        static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.Run(new MainWindow());
        }
    }
}
