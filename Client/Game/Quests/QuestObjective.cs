using Shared;
using System;

namespace Client
{  
    public class QuestObjective
    {
        public bool Completed { get; private set; }
        public string Text { get; private set; }

        public QuestObjective(string data)
        {
            this.Completed = false;

            string[] parts = data.Split('^');
            if(parts[0] == "V")
            {
                Text = "Visit " + SharedData.GetUnitName(Convert.ToInt32(parts[1]));
            }
            else if(parts[0] == "K")
            {
                Text = "Kill " + Convert.ToInt32(parts[2]) + "/" + Convert.ToInt32(parts[3])
                    + " " + SharedData.GetUnitName(Convert.ToInt32(parts[1]));
            }
        }
    }
}
