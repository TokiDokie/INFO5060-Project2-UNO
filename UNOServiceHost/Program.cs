/* File Name:       Program.cs
 * By:              Darian Benam, Darrell Bryan, Jacob McMullin, and Riley Kipp
 * Date Created:    Tuesday, April 5, 2022
 * Brief:           Main entry point of the UNO Service Host program which acts as a server for
 *                  clients to connect to and play a game of UNO with others. */

using System;
using System.ServiceModel;
using UNOLibrary.Networking;

namespace UNOServiceHost
{
    class Program
    {
        static void Main()
        {
            ServiceHost serviceHost = null;

            Console.WriteLine("Attempting to start the Uno game service...\n");

            try
            {
                serviceHost = new ServiceHost(typeof(UnoGame));
                serviceHost.Open();

                Console.WriteLine("Service started successfully!\n");

                Console.Write("Press any key to shut down the service...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception was thrown while attempting to start the Uno Game service: {0}", ex.Message);
            }
            finally
            {
                serviceHost?.Close(); // The ? is the short form way of checking if not null
            }
        }
    }
}
