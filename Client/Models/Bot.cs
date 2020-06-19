using Client.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Xaml;
using WatsonWebsocket;

namespace Client.Models {

    //in de lijst met bots op de gui staan ALLE bots (connecten en disconnected )(active/inactive)???
    //en als je die aanklikt komt er in het frame rechts een venster met de info van die bot
    //die info omvat dan: zie whiteboard EN connect / disconnect (canDisconnect) (canConnect) OOK delete bot optie
    //opties HOEVEEL games persisten? confirmbutton? popupvenster met listbox die de games bevat die zullen verwijderd worden en daar bevestigingsbutton
    //in aparte categorieen

    //nadenken over implementatie sorteeropties op hoofdvenster voor lisjt met bots,
    //sorteer op naam, versie, actief, etc
    //er moet ook een add bot button beschikbaar zijn

    //een bot heeft een list van games

    //we hebben enkel een botsrepository nodig

    //we slaan elke bot in een aparte bin op ?

    //er moet ook besslit worden WANNEER er wprdt opgeslagen naar de repo files
    //want als we na elke state moeten serializen gaat het misschien trager gaan
    //zeker als er veel bots zijn

    //een profile repository toevoegen, die profielen bevat
    //deze bevatten bv defaultwaardes van alle mogelijke settings 
    //en ook een lijst van welke bots bij dit profiel actief getracked worden
    // hieruit kunnen we halen welke bots moeten gedeserialized worden.

    //bij opstart direct de profiles repo uitlezen
    //en default constructor aanmaken als repo leeg is 
    //default bevat alle default settings en kan niet gedelete worden.
    //bij toevoegen van een nieuwe bots wordt een nieuw profiel aangemaakt,toevoegen aan default kan niet.

    [Serializable]
    public class Bot : Notifier {

        #region Fields
        private Guid _id = Guid.NewGuid();
        private readonly DateTime _addedTime = DateTime.Now;
        private string _botFilePath;
        private string _name;
        private string _version;
        private bool _isSelected = false;
        private IList _games = new List<Game>();

        private Logger _logger = new Logger();

        private readonly GameModeType _gameMode;
        private readonly LanguageType _language;
        #endregion

        public Bot(string path, GameModeType gameMode, LanguageType language) {
            _botFilePath = path;
            _gameMode = gameMode;
            _language = language;

            _name = Path.GetFileNameWithoutExtension(new FileInfo(path).FullName);
            _version = "V 1.3.3.7";

            for (int i = 0; i < 50; i++) {
                Games.Add(new Game());
            }

            //voor de mogelijke messages een model maken
            //Inheritance / Interfaces?
            //factory / builder adhv de type, en dan correcte message object maken
        }

        #region Properties
        public Logger Logger {
            get { return _logger; }
            set {
                if (_logger != value) {
                    _logger = value;
                    RaisePropertyChanged(() => Logger);
                }
            }
        }
        public bool IsSelected {
            get { return _isSelected; }
            set {
                if (_isSelected != value) {
                    _isSelected = value;
                    RaisePropertyChanged(() => IsSelected);
                }
            }
        }
        public DateTime AddedTime {
            get { return _addedTime; }
        }
        public Guid Id {
            get { return _id; }
            private set {
                if (_id != value) {
                    _id = value;
                    RaisePropertyChanged(() => Id);
                }
            }
        }
        public GameModeType GameMode {
            get { return _gameMode; }
        }
        public LanguageType Language {
            get { return _language; }
        }
        public int ActiveGameCount {
            get { return ((List<Game>)Games).Where(g => g.GameOnGoing).Count(); }
        }
        public string BotFilePath {
            get { return _botFilePath; }
            set {
                if (_botFilePath != value) {
                    _botFilePath = value;
                    RaisePropertyChanged(() => BotFilePath);
                }
            }
        }
        public string Name {
            get { return _name; }
            set {
                if (_name != value) {
                    _name = value;
                    RaisePropertyChanged(() => Name);
                }
            }
        }
        public string Version {
            get { return _version; }
            set {
                if (_version != value) {
                    _version = value;
                    RaisePropertyChanged(() => Version);
                }
            }
        }
        public IList Games {
            get { return _games; }
            set {
                if (_games != value) {
                    _games = value;
                    RaisePropertyChanged(() => Games);
                }
            }
        }
        #endregion

        #region methods
        // deze methode niet meer gebruiken, niet utbreidbaar naar node en python
        private string GetResponse(string request, params string[] requestparameters) {
            // https://stackoverflow.com/questions/8928528/dynamically-load-a-dll-from-a-specific-folder
            // Check if user has access to requested .dll.
            string strDllPath = Path.GetFullPath(this.BotFilePath);
            if (File.Exists(strDllPath)) {
                // Execute the method from the requested .dll using reflection (System.Reflection).
                Assembly DLL = Assembly.LoadFrom(strDllPath);
                Type classType = DLL.GetType(String.Format("{0}.{1}", "BotBase", "BotBase"));
                if (classType != null) {
                    // Create class instance.
                    var classInst = Activator.CreateInstance(classType);

                    // Invoke required method.
                    MethodInfo methodInfo = classType.GetMethod(request);
                    if (methodInfo != null) {
                        object result = null;
                        if (requestparameters != null) {
                            result = methodInfo.Invoke(classInst, new object[] { requestparameters[0] });
                        } else {
                            result = methodInfo.Invoke(classInst, new object[] { });
                        }
                        return result.ToString();
                    }
                }
            }
            return null;
        }
        #endregion
    }
}