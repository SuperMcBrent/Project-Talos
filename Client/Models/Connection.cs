using Client.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatsonWebsocket;

namespace Client.Models {
    public class Connection : Notifier {

        private WatsonWsClient _client;
        private MessageBuilder _builder;
        private Logger _logger;
        private Uri _connectionUri;
        private string _userName;
        private Bot _bot;
        private bool _registered = false;
        private bool _connected = false;
        private int _idAssignedByServer;

        #region Properties
        public WatsonWsClient Client {
            get { return _client; }
            set { _client = value; }
        }
        public MessageBuilder Builder {
            get { return _builder; }
            set { _builder = value; }
        }
        public Logger Logger {
            get { return _logger; }
            set { _logger = value; }
        }
        public Uri ConnectionUri {
            get { return _connectionUri; }
            set { _connectionUri = value; }
        }
        public string UserName {
            get { return _userName; }
            set { _userName = value; }
        }
        public Bot Bot {
            get { return _bot; }
            set { _bot = value; }
        }
        public TimeSpan Uptime {
            get { return Client.Stats.UpTime; }
        }
        public bool Connected {
            get { return _connected; }
            set {
                if (_connected != value) {
                    _connected = value;
                    RaisePropertyChanged(() => Connected);
                }
            }
        }
        public bool Registered {
            get { return _registered; }
            set {
                if (_registered != value) {
                    _registered = value;
                    RaisePropertyChanged(() => Registered);
                }
            }
        }
        public int IdAssignedByServer {
            get { return _idAssignedByServer; }
            set {
                if (_idAssignedByServer != value) {
                    _idAssignedByServer = value;
                    RaisePropertyChanged(() => IdAssignedByServer);
                }
            }
        }
        #endregion

        public Connection(string url, Bot bot) {
            this.Bot = bot;
            this.ConnectionUri = new Uri(url);
            this.UserName = "Nothing"; // for now
            this.Builder = new MessageBuilder();
            this.Logger = bot.Logger;
            //this.StartConnection();
        }

        public void StartConnection() {
            if (Client != null) Client.Dispose();
            Client = new WatsonWsClient(this.ConnectionUri);
            Client.ServerConnected += ServerConnected;
            Client.ServerDisconnected += ServerDisconnected;
            Client.MessageReceived += MessageReceived;
            Client.Logger = Log;
            Client.Start();
            Debug.WriteLine($"Client is {(Client.Connected ? "Connected" : "Not Connected")}.");
        }

        public void StopConnection() {
            if (Client != null) Client.Dispose();
            Connected = false;
            Registered = false;
        }

        private void MessageReceived(object sender, MessageReceivedEventArgs args) {
            string received = Encoding.UTF8.GetString(args.Data);
            Log("Received: " + received);

            Message message = Builder.ParseMessage(received);
            //Debug.WriteLine(message);
            if (message.GetValue("type").Equals("connected")) {
                Log("Received connected message");
                Connected = true;
            }

            if (message.GetValue("type").Equals("registered")) {
                Log("Received registered message");
                Registered = true;
                IdAssignedByServer = (int)message.GetValue("id");
            }
        }

        private void ServerConnected(object sender, EventArgs args) {
            Log($"Connected {Bot.Id}");
            //if (!Registered) {
            //    this.RegisterConnection();
            //}
        }

        private void ServerDisconnected(object sender, EventArgs args) {
            Log("Disconnected");
        }

        private void Transmit(string message) {
            Log("we are here");
            if (!this.Client.SendAsync(Encoding.UTF8.GetBytes(message)).Result) {
                Log("Failed");
                return;
            }
            Log($"Sent: {message}");
        }

        public void RegisterConnection() {
            Log("Registering conn");
            Message msg = Builder.PrepareRegisterBotMessage(Bot.Name,Bot.GameMode);
            Log("Message prepared");
            Task.Run(() => Transmit(msg.GetJSON()));
            //await Transmit(msg.GetJSON());
            Log("Done transmitting!");
        }

        private void Log(string message) {
            this.Logger.AddLog($"[Connection] {message}");
        }

    }
}
