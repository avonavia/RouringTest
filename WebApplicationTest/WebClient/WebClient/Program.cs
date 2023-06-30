using System;
using System.Net.Http;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace WebClient
{
    class Program
    {
        public static string cID { get; set; }
        public static string link { get; set; }
        static async Task Main()
        {
            DockerClient client = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine")).CreateClient();

            try
            {
                var containers = await client.Containers.ListContainersAsync(new ContainersListParameters { All = true });

                var found = false;

                foreach (var c in containers)
                {
                    if (c.Image == "webapplicationtest:latest")
                    {
                        found = true;
                        cID = c.ID;
                    }
                }
                if (!found)
                {
                    Console.WriteLine("Container not found");
                    Console.ReadKey();
                    Environment.Exit(0);
                }

                Console.WriteLine("Container found!");
                Console.WriteLine("Communicating to a container with ID: " + cID);
                await client.Containers.StartContainerAsync(cID, new ContainerStartParameters { });
                Console.WriteLine("Success");

                var containersUpd = await client.Containers.ListContainersAsync(new ContainersListParameters { All = true });

                found = false;

                System.Threading.Thread.Sleep(2000);

                foreach (var c in containersUpd)
                {
                    if (c.ID == cID && c.State == "running")
                    {
                        found = true;
                        foreach (var p in c.Ports)
                        {
                            if (p.PublicPort != 0)
                            {
                                link = "http://localhost:" + p.PublicPort.ToString() + "/cats/";
                            }
                        }
                    }
                }
                if (!found)
                {
                    Console.WriteLine("Error");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
            catch
            {
                Console.WriteLine("Docker is not on");
                Console.ReadKey();
                Environment.Exit(0);
            }

            RemindCommands();
            Commands();
        }
        public static void GetCatsList(string reqUrl)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string res = client.GetAsync(reqUrl).Result.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("\nResult:");
                    Console.WriteLine(res);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
            }
        }

        public static void GetCatByID(string reqUrl)
        {
            try
            {
                string id = string.Empty;
                do
                {
                    Console.Write("Cat ID = ");
                    id = Console.ReadLine();
                }
                while (int.TryParse(id, out int result) != true);

                using (HttpClient client = new HttpClient())
                {
                    string res = client.GetAsync(reqUrl + id).Result.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(res);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
            }
        }

        static void RemindCommands()
        {
            Console.WriteLine();
            Console.WriteLine("Open cat list: 1");
            Console.WriteLine("Find cat by id: 2");
        }

        static void Commands()
        {
            while (true)
            {
                var key = Console.ReadKey().Key;

                switch (key)
                {
                    case ConsoleKey.D1:
                        Console.WriteLine();
                        GetCatsList(link);
                        RemindCommands();
                        break;

                    case ConsoleKey.D2:
                        Console.WriteLine();
                        GetCatByID(link);
                        RemindCommands();
                        break;
                }
            }
        }
    }
}
