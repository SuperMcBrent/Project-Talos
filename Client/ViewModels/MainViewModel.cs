using Client.Business;
using Client.Framework;
using Client.Models;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;

namespace Client.ViewModels {

    public class MainViewModel : ClientViewModelBase {

        #region Fields
        private readonly IBotRepository _botRepository;
        private ObservableCollection<Bot> _bots;
        private Bot _selectedBot;

        private ObservableCollection<Game> _gamesThisSession;
        private CompositeCollection _compositeGamesThisSessionList;

        private IList<Connection> _connections;
        private Connection _selectedConnection;
        private string _socket = "ws://api.wartemis.com/socket";

        public RelayCommand OpenChangeProfileWindowCommand { get; private set; }
        public RelayCommand CollapseBotListCommand { get; private set; }
        public RelayCommand<Bot> SelectBotCommand { get; private set; }
        public RelayCommand DefaultSizeCommand { get; private set; }
        public RelayCommand AddBotCommand { get; private set; }
        public RelayCommand<Bot> ConnectCommand { get; private set; }
        public RelayCommand<Bot> RegisterCommand { get; private set; }
        public RelayCommand<Bot> DisconnectCommand { get; private set; }

        private bool _botSelectorCollapsed = false;
        #endregion

        #region Constructor
        public MainViewModel(IBotRepository botRepository) {
            _botRepository = botRepository;

            Bots = new ObservableCollection<Bot>(_botRepository.FindAll());

            if (_connections == null) _connections = new List<Connection>();
            foreach (var item in Bots) {
                if (_connections.Any(c => c.Bot.Id == item.Id)) continue;
                _connections.Add(new Connection(_socket, item));
            }

            SelectBot(Bots.FirstOrDefault());

            CollapseBotListCommand = new RelayCommand(CollapseBotList);
            SelectBotCommand = new RelayCommand<Bot>(SelectBot);
            DefaultSizeCommand = new RelayCommand(DefaultSize);
            AddBotCommand = new RelayCommand(OpenAddBotDialog);

            ConnectCommand = new RelayCommand<Bot>(ConnectBot);
            RegisterCommand = new RelayCommand<Bot>(RegisterBot);
            DisconnectCommand = new RelayCommand<Bot>(DisconnectBot);

            Messenger.Default.Register<Bot>(this, AddNewBot);
        }
        #endregion

        #region Properties
        public Connection SelectedConnection {
            get { return _selectedConnection; }
            set {
                if (_selectedConnection != value) {
                    _selectedConnection = value;
                    RaisePropertyChanged(() => SelectedConnection);
                }
            }
        }
        public Bot SelectedBot {
            get { return _selectedBot; }
            set {
                if (_selectedBot != value) {
                    _selectedBot = value;
                    RaisePropertyChanged(() => SelectedBot);
                }
            }
        }
        public bool BotSelectorCollapsed {
            get { return _botSelectorCollapsed; }
            set {
                if (_botSelectorCollapsed != value) {
                    _botSelectorCollapsed = value;
                    RaisePropertyChanged(() => BotSelectorCollapsed);
                }
            }
        }
        public ObservableCollection<Game> GamesThisSession {
            get { return _gamesThisSession; }
            set {
                if (_gamesThisSession != value) {
                    _gamesThisSession = value;
                    RaisePropertyChanged(() => GamesThisSession);
                }
            }
        }
        public ObservableCollection<Bot> Bots {
            get { return _bots; }
            set {
                if (_bots != value) {
                    _bots = value;
                    RaisePropertyChanged(() => Bots);
                }
            }
        }
        public CompositeCollection CompositeGamesThisSessionList {
            get { return _compositeGamesThisSessionList; }
            set {
                if (_compositeGamesThisSessionList != value) {
                    _compositeGamesThisSessionList = value;
                    RaisePropertyChanged(() => CompositeGamesThisSessionList);
                }
            }
        }
        #endregion

        #region CompositeCollection Fillers
        private void FillCompositeGamesList(bool startWithEmptyList) {
            if (startWithEmptyList) GamesThisSession = new ObservableCollection<Game>(new List<Game>());
            CompositeGamesThisSessionList = new CompositeCollection();
            CompositeGamesThisSessionList.Add(new CollectionContainer() {
                Collection = GamesThisSession
            });
            CompositeGamesThisSessionList.Add(new CollectionContainer() {
                Collection = new ObservableCollection<CustomListAddButton>() { new CustomListAddButton() }
            });
        }
        #endregion

        #region Methods
        private void DisconnectBot(Bot bot) {
            Debug.WriteLine($"I want to disconnect {bot.Id}");
            if (bot == null) return;
            var botConn = _connections.Where(c => c.Bot.Id.Equals(bot.Id)).FirstOrDefault();
            botConn.StopConnection();
        }
        private void RegisterBot(Bot bot) {
            Debug.WriteLine($"I want to register {bot.Id}");
            if (bot == null) return;
            var botConn = _connections.Where(c => c.Bot.Id.Equals(bot.Id)).FirstOrDefault();
            botConn.RegisterConnection();
        }
        private void ConnectBot(Bot bot) {
            Debug.WriteLine($"I want to connect {bot.Id}");
            if (bot == null) return;
            var botConn = _connections.Where(c => c.Bot.Id.Equals(bot.Id)).FirstOrDefault();
            SelectedConnection = botConn;
            botConn.StartConnection();
        }
        private void AddNewBot(Bot newBot) {
            if (newBot == null) return;
            Bots.Add(newBot);
            _botRepository.Save(newBot);
            if (!_connections.Any(c => c.Bot.Id == newBot.Id)) _connections.Add(new Connection(_socket, newBot));
            SelectBot(newBot);
        }
        /// <summary>
        /// Opens the aad bot window.
        /// </summary>
        private void OpenAddBotDialog() {
            var addBot = new Views.AddNewBotView();
            addBot.ShowDialog();
        }
        private void DefaultSize() {
            Debug.WriteLine("EH;KJTHWHTHwjthjkwhgtWJTHwerHWERewjrhWENJKHNGJLJBJ");
            Application.Current.MainWindow.Height = 800;
            Application.Current.MainWindow.Width = 713;
        }

        private void SelectBot(Bot selectedBot) {
            if (selectedBot == null) return;
            foreach (Bot bot in Bots.Where(b => b.IsSelected)) {
                bot.IsSelected = false;
            }
            SelectedBot = selectedBot;
            SelectedBot.IsSelected = true;
            SelectedConnection = _connections.Where(c => c.Bot.Id.Equals(selectedBot.Id)).FirstOrDefault();
        }

        //// <summary>
        /// Collapses or Uncollapses the bot list.
        /// </summary>
        private void CollapseBotList() {
            BotSelectorCollapsed = !BotSelectorCollapsed;
        }
        #endregion

    }
}