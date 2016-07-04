﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Client
{
    public sealed class CharacterButton : Button
    {
        public delegate void OnCharacterClickHandler(Button m, Character character);
        public event OnCharacterClickHandler CharacterClick;

        private Character character;

        private Font mainFont = Game.GetFont(10);
        private Font lesserFont = Game.GetFont(8);
        private Image characterImage;

        public CharacterButton(Point parentPosition, Rectangle position, Character character)
            : base(parentPosition, character.Name + ", " + character.Level + ", " + Game.GetSpecName(character.Spec), position)
        {
            this.character = character;
            font = Game.GetFont(position.Height - 20);

            this.backgroundImage = Game.GetCharacterButtonBackground();
            this.characterImage = Game.GetCharacterImage(character.Spec);
        }

        public override void SetFocused(bool focused)
        {
            this.focused = focused;
            if(this.focused)
                this.backgroundImage = Game.GetCharacterButtonBackgroundSelected();
            else
                this.backgroundImage = Game.GetCharacterButtonBackground();
        }

        public override void Render(Graphics g)
        {
            base.Render(g);
            RenderCharacterImage(g);
        }

        protected override void RenderText(Graphics g)
        {
            int marginLeft = 60;
            int marginTop = 6;
            g.DrawString(character.Name, mainFont, Brushes.Black, position.X + marginLeft, position.Y + marginTop);
            g.DrawString("Level " + character.Level, lesserFont, Brushes.Gray, position.X + marginLeft, position.Y + marginTop + mainFont.Size + 2);
            g.DrawString(Game.GetSpecName(character.Spec), lesserFont, Brushes.Gray, position.X + marginLeft, position.Y + marginTop + mainFont.Size + 2 + lesserFont.Size + 1);
        }

        private void RenderCharacterImage(Graphics g)
        {
            g.DrawImage(characterImage, position.X + 17, position.Y + 9, 35, 30);
        }

        public Character GetCharacter()
        {
            return character;
        }

        // ******************************************
        // EVENTS
        // ******************************************

        public override void OnMouseLeftUp(Point position) { }
        public override void OnMouseLeftDown(Point position) { }

        public override void OnKeyUp(int key)
        {
            if (key == 13)
            {
                CharacterClick(this, character);
                SetFocused(true);
            }
        }
    }
}