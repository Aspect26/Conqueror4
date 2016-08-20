using Shared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Client
{
    public partial class GameData 
    {
        private static Dictionary<int, string> mapIds = new Dictionary<int, string>()
        {
            { 0, "res/maps/kingdom.map" },
            { 1, "res/maps/fortress.map" },
            { 2, "res/maps/third" }
        };

        private static Dictionary<int, Image> tileImages = new Dictionary<int, Image>()
        {
            {-1, Image.FromFile("res/tiles/none.png") },
            { 0, Image.FromFile("res/tiles/grass.png") },
            { 1, Image.FromFile("res/tiles/dirt.png") },
            { 2, Image.FromFile("res/tiles/stone.png") },
            { 3, Image.FromFile("res/tiles/forest.png") },
        };

        public static int GetTileId(Color color)
        {
            // STONE
            if (color.R == 128 && color.G == 128 && color.B == 128)
                return 2;
            // GRASS
            else if (color.R == 192 && color.G == 224 && color.B == 0)
                return 0;
            // DIRT
            else if (color.R == 192 && color.G == 128 && color.B == 64)
                return 1;
            //FOREST
            else if (color.R == 32 && color.G == 192 && color.B == 64)
                return 3;

            Console.WriteLine("UNKNOWN MAP TILE: " + color.R + "," + color.G + "," + color.B);

            return -1;
        }

        // GET DATA
        public static string GetMapFilePath(int mapId)
        {
            return mapIds[mapId];
        }

        public static Image GetMapImage(int mapId)
        {
            return Image.FromFile(mapIds[mapId]);
        }

        public static Image GetEquipSlotImage()
        {
            return Image.FromFile("res/textures/equip_slot.png");
        }

        public static Image GetTile(int tileId)
        {
            return tileImages[tileId];
        }

        public static string GetCharacterBasePath(int spec)
        {
            switch (spec)
            {
                case SharedData.UNIT_DEMONHUNTER:
                    return "res/units/demonhunter/demonhunter";
                case SharedData.UNIT_MAGE:
                    return "res/units/mage/mage";
                case SharedData.UNIT_PRIEST:
                    return "res/units/priest/priest";
                case SharedData.UNIT_WARLOCK:
                case SharedData.UNIT_UNKHERO1:
                    return "res/units/warlock/warlock";
                case SharedData.UNIT_SHAMAN:
                    return "res/units/shaman/shaman";
                default:
                    return "res/units/warlock/warlock";
            }
        }

        public static Image GetMissileImage(int id)
        {
            string path = "res/sprites/missiles/";
            switch (id)
            {
                case SharedData.UNIT_DEMONHUNTER:
                    path += "demonhunter"; break;
                case SharedData.UNIT_MAGE:
                    path += "mage"; break;
                case SharedData.UNIT_PRIEST:
                    path += "priest"; break;
                case SharedData.UNIT_WARLOCK:
                    path += "warlock"; break;

                case SharedData.UNIT_WARLOCK_SPAWNER:
                    path += "warlock"; break;
                case SharedData.UNIT_AWAKENED_SOUL:
                    path += "soul"; break;
                default:
                    path += "default"; break;
            }
            path += ".png";
            return Image.FromFile(path);
        }

        public static Image GetUnknownGroundObjectImage()
        {
            return Image.FromFile("res/ground_objects/unknown.png");
        }

        public static Image GetChestImage()
        {
            return Image.FromFile("res/ground_objects/chest.png");
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

        public static Image GetUnitImage(int spec)
        {
            switch (spec)
            {
                // HEROES
                case SharedData.UNIT_DEMONHUNTER: return Image.FromFile("res/units/demonhunter/demonhunter_none.png");
                case SharedData.UNIT_MAGE: return Image.FromFile("res/units/mage/mage_none.png");
                case SharedData.UNIT_PRIEST: return Image.FromFile("res/units/priest/priest_none.png");

                case SharedData.UNIT_WARLOCK: return Image.FromFile("res/units/warlock/warlock_none.png");
                case SharedData.UNIT_UNKHERO1: return Image.FromFile("res/units/warlock/warlock_none.png");
                case SharedData.UNIT_SHAMAN: return Image.FromFile("res/units/shaman/shaman_none.png");

                // UNITS
                case SharedData.UNIT_WOLF: return Image.FromFile("res/units/wolf.png");
                case SharedData.UNIT_WOLF_PACK_LEADER: return Image.FromFile("res/units/wolf_pack_leader.png");
                case SharedData.UNIT_WARLOCK_SPAWNER: return Image.FromFile("res/units/warlock_spawner.png");
                case SharedData.UNIT_AWAKENED_SOUL: return Image.FromFile("res/units/awakened_soul.png");

                case SharedData.UNIT_LIEUTENANT_LANDAX: return Image.FromFile("res/units/lieutenant_landax.png");
                case SharedData.UNIT_BERLOC_PYRESTEEL: return Image.FromFile("res/units/berloc_pyresteel.png");

                // DEFAULT
                default: return Image.FromFile("res/units/unknown.png");
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

        public static bool IsPlayerUnit(int unitId)
        {
            return unitId > 0 && unitId < 7;
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
                case SharedData.UNIT_DEMONHUNTER: return "Demon Hunter";
                case SharedData.UNIT_MAGE: return "Mage";
                case SharedData.UNIT_PRIEST: return "Priest";

                case SharedData.UNIT_WARLOCK: return "Warlock";
                case SharedData.UNIT_UNKHERO1: return "Unnamed";
                case SharedData.UNIT_SHAMAN: return "Shaman";

                default: return "Unknown";
            }
        }
    }
}
