using Client.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Client.Utilities {
    public class GameModeTypeTemplateSelector : DataTemplateSelector {
        public DataTemplate PlanetWarsImageTemplate { get; set; }
        public DataTemplate TicTacToeImageTemplate { get; set; }
        public DataTemplate ConquestImageTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container) {
            switch ((GameModeType)item) {
                case GameModeType.PlanetWars:
                    return PlanetWarsImageTemplate;
                case GameModeType.TicTacToe:
                    return TicTacToeImageTemplate;
                case GameModeType.Conquest:
                    return ConquestImageTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
