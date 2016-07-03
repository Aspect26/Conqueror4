using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Client
{
    public partial class Game 
    {
        private static Dictionary<int, string> mapIds = new Dictionary<int, string>()
        {
            { 0, "res/maps/first" },
            { 1, "res/maps/sekunda" },
            { 2, "res/maps/third" }
        };

        private static Dictionary<int, Image> tileImages = new Dictionary<int, Image>()
        {
            { 0, Image.FromFile("res/tiles/grass.png") },
            { 1, Image.FromFile("res/tiles/dirt.png") },
            { 2, Image.FromFile("res/tiles/stone.png") }
        };

        private static Dictionary<int, Image> playerImages = new Dictionary<int, Image>()
        {
            { 0, Image.FromFile("res/player/sesshomaru.png") },
            { 1, Image.FromFile("res/player/kagome.png") },
            { 2, Image.FromFile("res/player/kirara.png") },
            { 3, Image.FromFile("res/player/ryuk.png") },
            { 4, Image.FromFile("res/player/ash.gif") },
        };

        // GET DATA
        public static string getMapFilePath(int mapId)
        {
            return mapIds[mapId];
        }

        public static Image getTile(int tileId)
        {
            return tileImages[tileId];
        }

        public static Image GetPlayerImage(int charId)
        {
            return playerImages[charId];
        }

        // TEXTURES

        public static Image GetPanelBackground()
        {
            return Image.FromFile("res/textures/panel.png");
        }

        public static Image GetButtonBackground()
        {
            return Image.FromFile("res/textures/button.png");
        }

        public static Image GetButtonBackgroundPressed()
        {
            return Image.FromFile("res/textures/button_pressed.png");
        }

        public static Image GetUIHorizontalBorder()
        {
            return Image.FromFile("res/textures/horizontalborder.png");
        }

        public static Image GetUIVerticalBorder()
        {
            return Image.FromFile("res/textures/verticalborder.png");
        }

        public static Image GetUIHorizontalBorderFocused()
        {
            return Image.FromFile("res/textures/horizontalborder_focused.png");
        }

        public static Image GetUIVerticalBorderFocused()
        {
            return Image.FromFile("res/textures/verticalborder_focused.png");
        }

        // BACKGROUNDS
        public static Image GetLoginBackground()
        {
            return Image.FromFile("res/backgrounds/login.png");
        }

        public static Image GetCharactersBackground()
        {
            return Image.FromFile("res/backgrounds/characters.png");
        }

        // CHECKS
        private static Regex nicknamePattern = new Regex("^[a-zA-Z1-9]+$");

        public static bool IsValidUsername(string name)
        {
            return nicknamePattern.IsMatch(name);
        }

        // FONT
        public static Font GetFont(int size)
        {
            return new Font(FontFamily.GenericMonospace, size);
        }

        // TEXTS
        public static string GetSpecName(int spec)
        {
            switch (spec)
            {
                case 1: return "Demon Hunter";
                case 2: return "Mage";
                case 3: return "Priest";

                case 4: return "Warlock";
                case 5: return "Unnamed";
                case 6: return "Unnamed";

                default: return "Unknown";
            }
        }
    }
}
