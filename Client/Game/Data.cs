﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Client
{
    public partial class GameData 
    {
        private const int DEMON_HUNTER = 1;
        private const int MAGE = 2;
        private const int PRIEST = 3;

        private const int WARLOCK = 4;
        private const int UNK_CLS_1 = 5;
        private const int UNK_CLS_2 = 6;

        private static Dictionary<int, string> mapIds = new Dictionary<int, string>()
        {
            { 0, "res/maps/kingdom.map" },
            { 1, "res/maps/fortress.map" },
            { 2, "res/maps/third" }
        };

        private static Dictionary<int, Image> tileImages = new Dictionary<int, Image>()
        {
            { 0, Image.FromFile("res/tiles/grass.png") },
            { 1, Image.FromFile("res/tiles/dirt.png") },
            { 2, Image.FromFile("res/tiles/stone.png") }
        };

        // GET DATA
        public static string GetMapFilePath(int mapId)
        {
            return mapIds[mapId];
        }

        public static Image GetTile(int tileId)
        {
            return tileImages[tileId];
        }

        public static string GetCharacterBasePath(int spec)
        {
            switch (spec)
            {
                case DEMON_HUNTER:
                case MAGE:
                case PRIEST:
                    return "res/units/priest/priest";
                case WARLOCK:
                case UNK_CLS_1:
                case UNK_CLS_2:
                    return "res/units/warlock/warlock";
                default:
                    return "res/units/warlock/warlock";
            }
        }

        // GAME IMAGES
        public static Image GetDefaultMissileImage()
        {
            return Image.FromFile("res/sprites/defaultMissile.png");
        }

        // UI COMPONENTS
        public static Image GetLineInputBackgground()
        {
            return Image.FromFile("res/textures/lineinputbackground.png");
        }

        public static Image GetPanelBackground()
        {
            return Image.FromFile("res/textures/panel.png");
        }

        public static Image GetCharacterImage(int spec)
        {
            switch (spec)
            {
                case 1: return Image.FromFile("res/units/demonhunter/demonhunter_none.png");
                case 2: return Image.FromFile("res/units/mage/mage_none.png");
                case 3: return Image.FromFile("res/units/priest/priest_none.png");

                case 4: return Image.FromFile("res/units/warlock/warlock_none.png");
                case 5:
                case 6:

                default: return Image.FromFile("res/units/warlock/warlock_none.png");
            }
        }

        public static Image GetCharacterButtonBackground()
        {
            return Image.FromFile("res/textures/characterbutton.png");
        }

        public static Image GetCharacterButtonBackgroundSelected()
        {
            return Image.FromFile("res/textures/characterbutton_selected.png");
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
                case DEMON_HUNTER: return "Demon Hunter";
                case MAGE: return "Mage";
                case PRIEST: return "Priest";

                case WARLOCK: return "Warlock";
                case UNK_CLS_1: return "Unnamed";
                case UNK_CLS_2: return "Unnamed";

                default: return "Unknown";
            }
        }
    }
}