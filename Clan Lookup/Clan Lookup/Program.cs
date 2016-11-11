using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clan_Lookup
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Welcome To Swan's RuneScape Clan Lookup\nEnter a clan name to get a list of its members: ");

            // get input
            string clan_name = Console.ReadLine();
            string rs_api = "http://services.runescape.com/m=clan-hiscores/members_lite.ws?clanName=" + clan_name;
            Console.WriteLine("\nGrabbing '" + clan_name + "' clan members... \n");

            // get contents of api
            var data = grab_url_contents(rs_api);

            // is clan name invalid?
            if (data.ToLower().Contains('<')) 
            {
                Console.WriteLine("\nError: You have entered an invalid clan name Restart the application and type a correct one.\n");
                Console.ReadKey(); // TODO: make it loop around when not lazy

            } else {

                // clan_name.txt
                string path = clan_name + ".txt";

                // Split CSV
                char[] delimiterChar = { '\n', ',' };
                string[] data_separated = data.Split(delimiterChar);

                // Place Clan Members into its own list
                List<string> clan_members = new List<string>();
                for (int i = 4; i < data_separated.Count(); i += 4)
                {
                    clan_members.Add(data_separated[i]);
                }

                // Check if clan_name.txt already exists
                Console.WriteLine("Checking if file already exists...\n");
                if (!File.Exists(path))
                {

                    Console.WriteLine("File does not exist, creating file: " + clan_name + ".txt\n");

                    // Create and write member names to file
                    File.Create(path).Close();
                    TextWriter tw = new StreamWriter(path);
                    foreach (string member in clan_members)
                    {
                        tw.WriteLine(member);
                    }
                    tw.Close();

                    // Replace ? unicode with spaces
                    string text = File.ReadAllText(path);
                    text = text.Replace("?", " ");
                    File.WriteAllText(path, text);

                    // Complete
                    Console.WriteLine("Complete!\nPress any key to exit.");
                    Console.ReadKey();
                
                } else if (File.Exists(path)) { // Ask to overwrite if file already exists
                
                    Console.Write("File already exists... Would you like to overwrite the current file? (y/n): ");
                    string overwrite_choice = Console.ReadLine();

                    if (overwrite_choice == "y")
                    {
                        Console.WriteLine("\nWriting to file...");

                        // Delete -> Create -> Write
                        File.Delete(path);
                        File.Create(path).Close();
                        TextWriter tw = new StreamWriter(path);
                        foreach (string member in clan_members)
                        {
                            tw.WriteLine(member);
                        }
                        tw.Close();

                        // ? -> Space
                        string text = File.ReadAllText(path);
                        text = text.Replace("?", " ");
                        File.WriteAllText(path, text);

                        // Complete
                        Console.WriteLine("\nComplete!\nPress any key to exit.");
                        Console.ReadKey();

                    } else {

                        Console.WriteLine("\nPress any key to exit.");
                        Console.ReadKey();

                    }
                }
                Console.ReadLine();
            }

        }

        // Grabs the contents of url similar to file_get_contents in php (Creds: devfred)
        public static string grab_url_contents(string fileName)
        {

            string sContents = string.Empty;
            if (fileName.ToLower().IndexOf("http:") > -1)
            {
                // URL 
                System.Net.WebClient wc = new System.Net.WebClient();
                byte[] response = wc.DownloadData(fileName);
                sContents = Encoding.ASCII.GetString(response);
            } else {
                // Regular Filename 
                StreamReader sr = new StreamReader(fileName);
                sContents = sr.ReadToEnd();
                sr.Close();
            }
            return sContents;
        }
    }
}

