using Client.Framework;
using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Client.Models {

    [Serializable]
    public class Bot : Notifier {
        private int _activeGameCount;
        private string _filePath;
        private string _name;
        private string _version;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="Bot"/> class.
        /// We need this constructor because we are creating empty
        /// notes in the VM.
        /// </summary>
        public Bot() : this(string.Empty) {
        }

        public Bot(string path) {
            _filePath = path;

            //voor de mogelijke messages een model maken
            //Inheritance / Interfaces?

            //factory / builder adhv de type, en dan correcte message object maken

        }
        public int ActiveGameCount {
            get { return _activeGameCount; }
            set {
                if (_activeGameCount != value) {
                    _activeGameCount = value;
                    RaisePropertyChanged(() => ActiveGameCount);
                }
            }
        }

        public string FilePath {
            get { return _filePath; }
            set {
                if (_filePath != value) {
                    _filePath = value;
                    RaisePropertyChanged(() => FilePath);
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
        // deze methode niet meer gebruiken, niet utbreidbaar naar node en python
        private string GetResponse(string request, params string[] requestparameters) {
            // https://stackoverflow.com/questions/8928528/dynamically-load-a-dll-from-a-specific-folder
            // Check if user has access to requested .dll.
            string strDllPath = Path.GetFullPath(this.FilePath);
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
    }
}