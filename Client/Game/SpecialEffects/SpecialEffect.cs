using Shared;
using System;
using System.Drawing;

namespace Client
{
    public abstract class SpecialEffect : ISpecialEffect
    {
        public bool IsDead { get; protected set; }

        public abstract void Render(Graphics g);
        public abstract void PlayCycle(int timeSpan);


        // PARSE SPELLS
        public static SpecialEffect ParseAbilityEffect(Game game, string data)
        {
            string[] parts = data.Split('&');
            int spellId = Convert.ToInt32(parts[1]);
            switch (spellId)
            {
                case SharedData.ABILITY_PRIEST_HEAL:
                    return parsePriestHeal(game, parts);

                default:
                    Console.WriteLine("Unimplemented ability: " + spellId + "!");
                    return null;
            }
        }

        private static SpecialEffect parsePriestHeal(Game game, string[] data)
        {
            int x = Convert.ToInt32(data[2]);
            int y = Convert.ToInt32(data[3]);
            return new AbilityPriestHeal(game, new Point(x, y));
        }
    }
}
