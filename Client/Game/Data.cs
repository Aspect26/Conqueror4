using Shared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Client
{
    /// <summary>
    /// Represents the game data, mostly paths to images and strings. 
    /// </summary>
    public static class GameData 
    {
        // mapId -> path to the map file
        private static Dictionary<int, string> mapIds = new Dictionary<int, string>()
        {
            { 0, "res/maps/kingdom.map" },
            { 1, "res/maps/fortress.map" },
            { 2, "res/maps/third" }
        };

        // tile id -> tile image
        private static Dictionary<int, Image> tileImages = new Dictionary<int, Image>()
        {
            {-1, Image.FromFile("res/tiles/none.png") },
            { 0, Image.FromFile("res/tiles/grass.png") },
            { 1, Image.FromFile("res/tiles/dirt.png") },
            { 2, Image.FromFile("res/tiles/stone.png") },
            { 3, Image.FromFile("res/tiles/forest.png") },
        };

        /// <summary>
        /// Gets the tile identifier from the color of a pixel in the map file.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The tile identifier.</returns>
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

        /// <summary>
        /// Gets the map file path.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <returns>Path to the map file.</returns>
        public static string GetMapFilePath(int mapId)
        {
            return mapIds[mapId];
        }

        /// <summary>
        /// Gets the map image.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <returns>Map image.</returns>
        public static Image GetMapImage(int mapId)
        {
            return Image.FromFile(mapIds[mapId]);
        }

        /// <summary>
        /// Gets the equip slot image.
        /// </summary>
        /// <returns>The image.</returns>
        public static Image GetEquipSlotImage()
        {
            return Image.FromFile("res/textures/equip_slot.png");
        }

        /// <summary>
        /// Gets the item image.
        /// </summary>
        /// <param name="itemSlot">The item slot.</param>
        /// <returns>Image.</returns>
        public static Image GetItemImage(int itemSlot)
        {
            switch (itemSlot)
            {
                case SharedData.ITEM_SLOT_WEAPON:
                    return Image.FromFile("res/items/weapon.png");
                case SharedData.ITEM_SLOT_CHEST:
                    return Image.FromFile("res/items/chest.png");
                case SharedData.ITEM_SLOT_HEAD:
                    return Image.FromFile("res/items/head.png");
                case SharedData.ITEM_SLOT_PANTS:
                    return Image.FromFile("res/items/pants.png");
                default:
                    return null; //crash
            }
        }

        /// <summary>
        /// Gets the tile image.
        /// </summary>
        /// <param name="tileId">The tile identifier.</param>
        /// <returns>Image.</returns>
        public static Image GetTile(int tileId)
        {
            return tileImages[tileId];
        }

        /// <summary>
        /// Gets the character base path.
        /// </summary>
        /// <param name="spec">The specialization.</param>
        /// <returns>The path</returns>
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

        /// <summary>
        /// Gets the missile image.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Image.</returns>
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

        /// <summary>
        /// Gets the unknown ground object image.
        /// </summary>
        /// <returns>Image.</returns>
        public static Image GetUnknownGroundObjectImage()
        {
            return Image.FromFile("res/ground_objects/unknown.png");
        }

        /// <summary>
        /// Gets the chest image.
        /// </summary>
        /// <returns>Image.</returns>
        public static Image GetChestImage()
        {
            return Image.FromFile("res/ground_objects/chest.png");
        }

        /// <summary>
        /// Gets the line input background.
        /// </summary>
        /// <returns>Image.</returns>
        public static Image GetLineInputBackground()
        {
            return Image.FromFile("res/textures/lineinputbackground.png");
        }

        /// <summary>
        /// Gets the panel background.
        /// </summary>
        /// <returns>Image.</returns>
        public static Image GetPanelBackground()
        {
            return Image.FromFile("res/textures/panel.png");
        }

        /// <summary>
        /// Gets the unit image.
        /// </summary>
        /// <param name="unitId">The unit identifier.</param>
        /// <returns>Image.</returns>
        public static Image GetUnitImage(int unitId)
        {
            switch (unitId)
            {
                // HEROES
                case SharedData.UNIT_DEMONHUNTER: return Image.FromFile("res/units/demonhunter/demonhunter_none.png");
                case SharedData.UNIT_MAGE: return Image.FromFile("res/units/mage/mage_none.png");
                case SharedData.UNIT_PRIEST: return Image.FromFile("res/units/priest/priest_none.png");

                case SharedData.UNIT_WARLOCK: return Image.FromFile("res/units/warlock/warlock_none.png");
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

        /// <summary>
        /// Gets the character button background.
        /// </summary>
        /// <returns>Image.</returns>
        public static Image GetCharacterButtonBackground()
        {
            return Image.FromFile("res/textures/characterbutton.png");
        }

        /// <summary>
        /// Gets the character button background selected.
        /// </summary>
        /// <returns>Image.</returns>
        public static Image GetCharacterButtonBackgroundSelected()
        {
            return Image.FromFile("res/textures/characterbutton_selected.png");
        }

        /// <summary>
        /// Gets the button background.
        /// </summary>
        /// <returns>Image.</returns>
        public static Image GetButtonBackground()
        {
            return Image.FromFile("res/textures/button.png");
        }

        /// <summary>
        /// Gets the button background pressed.
        /// </summary>
        /// <returns>Image.</returns>
        public static Image GetButtonBackgroundPressed()
        {
            return Image.FromFile("res/textures/button_pressed.png");
        }

        /// <summary>
        /// Gets the UI horizontal border.
        /// </summary>
        /// <returns>Image.</returns>
        public static Image GetUIHorizontalBorder()
        {
            return Image.FromFile("res/textures/horizontalborder.png");
        }

        /// <summary>
        /// Gets the UI vertical border.
        /// </summary>
        /// <returns>Image.</returns>
        public static Image GetUIVerticalBorder()
        {
            return Image.FromFile("res/textures/verticalborder.png");
        }

        /// <summary>
        /// Gets the UI horizontal border focused.
        /// </summary>
        /// <returns>Image.</returns>
        public static Image GetUIHorizontalBorderFocused()
        {
            return Image.FromFile("res/textures/horizontalborder_focused.png");
        }

        /// <summary>
        /// Gets the UI vertical border focused.
        /// </summary>
        /// <returns>Image.</returns>
        public static Image GetUIVerticalBorderFocused()
        {
            return Image.FromFile("res/textures/verticalborder_focused.png");
        }

        /// <summary>
        /// Gets the login background.
        /// </summary>
        /// <returns>Image.</returns>
        public static Image GetLoginBackground()
        {
            return Image.FromFile("res/backgrounds/login.png");
        }

        /// <summary>
        /// Gets the characters background.
        /// </summary>
        /// <returns>Image.</returns>
        public static Image GetCharactersBackground()
        {
            return Image.FromFile("res/backgrounds/characters.png");
        }

        private static Regex nicknamePattern = new Regex("^[a-zA-Z1-9]+$");

        /// <summary>
        /// Determines whether the speciddief name is valid.
        /// THIS SHOULD BE DONE ON THE SERVER SIDE...
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns><c>true</c> if [is valid username] [the specified name]; otherwise, <c>false</c>.</returns>
        public static bool IsValidUsername(string name)
        {
            return nicknamePattern.IsMatch(name);
        }

        /// <summary>
        /// Determines whether [is player unit] [the specified unit identifier].
        /// </summary>
        /// <param name="unitId">The unit identifier.</param>
        /// <returns><c>true</c> if [is player unit] [the specified unit identifier]; otherwise, <c>false</c>.</returns>
        public static bool IsPlayerUnit(int unitId)
        {
            return unitId > 0 && unitId < 7;
        }

        // FONT
        /// <summary>
        /// Gets the font.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>Font.</returns>
        public static Font GetFont(int size)
        {
            return new Font(FontFamily.GenericMonospace, size);
        }

        // TEXTS
        /// <summary>
        /// Gets the name of the spec.
        /// </summary>
        /// <param name="spec">The spec.</param>
        /// <returns>System.String.</returns>
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

        /// <summary>
        /// Gets the name of the item.
        /// </summary>
        /// <param name="slot">The slot.</param>
        /// <returns>System.String.</returns>
        public static string GetItemName(int slot)
        {
            switch (slot)
            {
                case SharedData.ITEM_SLOT_WEAPON:
                    return "Weapon";
                case SharedData.ITEM_SLOT_CHEST:
                    return "Chest";
                case SharedData.ITEM_SLOT_HEAD:
                    return "Head";
                case SharedData.ITEM_SLOT_PANTS:
                    return "Pants";
                default:
                    return "Unknown";
            }
        }

        /// <summary>
        /// Gets the character information.
        /// Not implemented...
        /// </summary>
        /// <param name="spec">The spec.</param>
        /// <returns>System.String.</returns>
        public static string GetCharacterInfo(int spec)
        {
            switch (spec)
            {
                case SharedData.UNIT_PRIEST:
                    return "fddfb db d bd bd bd bdbdf bdb d bfd gdf gd gdfgdfgd gdg d gdf gdfg d";
                default:
                    return "sdhuih g sjn sdughsldhg uh huhgs;djgh ;oohgljdhg slieh slu jdkgilsuhg sdhg sldugh sdlunjvhlesui gnfg ldsuif jvb esdfgn sudfh knsdfjgn ;seogh dfiog s;df ;sofid bs;o h.";
            }
        }
    }
}
