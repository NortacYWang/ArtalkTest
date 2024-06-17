using Artalk.Xmpp;
using Artalk.Xmpp.Client;
using Artalk.Xmpp.Extensions;
using Artalk.Xmpp.Im;

namespace ArtalkTest
{
    public class ArtalkConnection
    {

        private ArtalkXmppClient _client;

        public ArtalkConnection(string hostname, string username, string password)
        {
            _client = new ArtalkXmppClient(hostname, username, password);
            
            //_client.FileTransferSettings.ForceInBandBytestreams = true;
            //_client.FileTransferSettings.UseUPnP = true;

            _client.Message += OnNewMessage;
            _client.SubscriptionRequest = OnSubscriptionRequest;
            _client.SubscriptionApproved += OnSubscriptionApproved;
            _client.SubscriptionRefused += OnSubscriptionRefused;
            _client.FileTransferRequest = OnFileTransferRequest;
            _client.FileTransferProgress += OnFileTransferProgress;
            _client.FileTransferAborted += onFileTransferAborted;
        }

        public bool Connect()
        {
            try
            {
                _client.Connect();
                return true;

            } catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;

            }
        }

        public List<RosterItem> getContacts()
        {
            Roster roaster = _client.GetRoster();
            return roaster.ToList();

        }

        public Jid getOwnJid()
        {
            return _client.Jid;
        }

        public List<string> getFeatures(Jid id)
        {
            return _client.GetFeatures(id).ToList();
        }

        public bool SendMessage(Jid recipientId, string message)
        {
            try
            {
                _client.SendMessage(recipientId, message, null, null, MessageType.Chat);
                return true;
            } catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;

            }
            
        }

        // to set status
        public bool SetStatus(Availability availability, string message)
        {
            try
            {
                _client.SetStatus(availability, message);
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

        }

        // to publish tune information to other contacts.
        public bool SetTune(TuneInformation tune)
        {
            try
            {
                _client.SetTune(tune);
                return true;

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            
        }

        //  publish the user's current activity.
        public bool SetActivity(GeneralActivity activity, SpecificActivity special)
        {
            try
            {
                _client.SetActivity(activity, special);
           
                return true;

            } catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;

            }
            
        }

        public static void FileTransferCallback(bool hasSent, FileTransfer file)
        {
            Console.WriteLine("has sent file >" + hasSent);

        }

        public void InitialFileTransfer(Jid to, string path)
        {
            _client.InitiateFileTransfer(to, path, null, FileTransferCallback);
        }

        public void OnNewMessage(object sender, MessageEventArgs e)
        {
            Console.WriteLine("Message from <" + e.Jid + ">: " + e.Message.Body);
        }

        public void OnFileTransferProgress(object sender, FileTransferProgressEventArgs e)
        {
            Console.WriteLine("File transfer propress <" + e.Transfer.Name + ">: " +e.Transfer.SessionId +  e.Transfer.Transferred );
        }

        public void onFileTransferAborted(object sender, FileTransferAbortedEventArgs e)
        {
            Console.WriteLine("File transfer aborted" + e.Transfer.Name + ">: " + e.Transfer.Transferred);

        }

        public void OnSubscriptionApproved(object sender, SubscriptionApprovedEventArgs e )
        {
            Console.WriteLine("Subscription Approved from <" + e.Jid + ">: ");

        }
        public void OnSubscriptionRefused(object sender, SubscriptionRefusedEventArgs e)
        {
            Console.WriteLine("Subscription refused from <" + e.Jid + ">: ");

        }

        static bool OnSubscriptionRequest(Jid from)
        {
            Console.WriteLine(from + " wants to subscribe to your presence.");
            Console.Write("Type Y to approve the request, or any other key to refuse it: ");

            // Return true to approve the request, or false to refuse it.
            return Console.ReadLine().ToLowerInvariant() == "y";
        }

        static string OnFileTransferRequest(FileTransfer file)
        {
            Console.WriteLine(file.From + " wants to File transfer." + file.Name + " " + file.GetType());

            // return null to refuse
            return "allow";
        }




    }
}
