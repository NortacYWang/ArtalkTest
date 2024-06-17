using Artalk.Xmpp;
using Artalk.Xmpp.Extensions;
using Artalk.Xmpp.Im;
using ArtalkTest;
using System.Diagnostics;
using System.Linq.Expressions;

namespace artalkTest
{
    public class Program
    {

        public static void Main(string[] args)
        {
            string hostname = "ott-eng-yipanw";
            string username = "test1";
            string password = "admin123";

            ArtalkConnection artalkConnection = new(hostname, username, password);

            bool isContinue = true;
            bool isConnected = false;
            List<RosterItem> contacts = new List<RosterItem>();


            while (isContinue == true)
            {
                Console.WriteLine();
                Console.WriteLine("Select: ");
                if(!isConnected)
                {
                    Console.WriteLine("1 Connect to server(login)");
                }
                if(isConnected == true)
                {
                    Console.WriteLine("2 Get contacts");
                    Console.WriteLine("3 Send Messages");
                    Console.WriteLine("4 Set Activity");
                    Console.WriteLine("5 Set Status");
                    Console.WriteLine("6 Send a file");
                    Console.WriteLine("7 get features");
                }

                Console.WriteLine("q Quit");

                string command = Console.ReadLine();

                switch (command)
                {
                    case "1":
                        isConnected = artalkConnection.Connect();
                        if(isConnected == true)
                        {
                            Console.WriteLine("Connected!!!");
                            contacts = artalkConnection.getContacts();
                        }
                        break;
                    case "2":
                        contacts = artalkConnection.getContacts();
                        Console.WriteLine("------ Contacts ------");
                        contacts.ForEach(ro => Console.WriteLine("\n id: " + ro.Jid + "\n name: " + ro.Name + "\n group: " + String.Join(", ", ro.Groups)));
                        break;
                    case "3":
                        Console.WriteLine("----- Sending Message ------");
                        Jid firstContactId = contacts.FirstOrDefault().Jid;
                        Console.WriteLine("jid to: " + firstContactId);
                        string message = Console.ReadLine();
                        bool isSent = artalkConnection.SendMessage(firstContactId, message);
                        if (isSent == true) Console.WriteLine("message sent!");
                        break;
                    case "4":
                        Console.WriteLine("---- Set activity to having breakfast -----");
                        bool hasSet = artalkConnection.SetActivity(GeneralActivity.Eating, SpecificActivity.HavingBreakfast);
                        if (hasSet == true) Console.WriteLine("activity set");

                        break;
                    case "5":
                        Console.WriteLine("---- Set status to having a chat -----");
                        bool hasChangedStatus = artalkConnection.SetStatus(Availability.Chat, "having a chat");
                        if (hasChangedStatus == true) Console.WriteLine("status set");

                        break;
                    case "6":
                        Console.WriteLine("---- Sending a file ----");
                        string projectPath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
                        string imagePath = Path.Combine(projectPath, "smile.png");

                        Console.WriteLine("image path: " + imagePath);
                        Jid sendFileTo = contacts.FirstOrDefault().Jid;
                        Console.WriteLine("jid to: " + sendFileTo);
                        artalkConnection.InitialFileTransfer(sendFileTo, imagePath);


                        break;
                    case "7":
                        List<string> features = artalkConnection.getFeatures(contacts.FirstOrDefault().Jid);

                        features.ForEach(feature => Console.WriteLine(" feature>> " + feature));

                        break;
                    case "q":
                        isContinue = false;
                        break;

                   default: 
                        Console.WriteLine("Invalid");
                        break;



                }

            }


        }
    }

}
