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

        public override DataTemplate SelectTemplate(object item, DependencyObject container) {
            Debug.WriteLine("here");
            switch ((GameModeType)item) {
                case GameModeType.PlanetWars:
                    Debug.WriteLine("planetwarsss");
                    return PlanetWarsImageTemplate;
                case GameModeType.TicTacToe:
                    Debug.WriteLine("tikkitokkitooeee");
                    return TicTacToeImageTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
