using System.ServiceProcess;

namespace AppServer.Host
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        private static void Main()
        {
            ServiceBase.Run(new Service1());
        }
    }
}