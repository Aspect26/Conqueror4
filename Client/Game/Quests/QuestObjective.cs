using Shared;
using System;

namespace Client
{
    /// <summary>
    /// Represents quest objectives.
    /// The implementation is very simple on the client's side, it really only holds the data in a string.
    /// This shall be remade. Most probably the same way as it is in the server side (different implementations
    /// for different objective types)
    /// </summary>
    public class QuestObjective
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="QuestObjective"/> is completed.
        /// </summary>
        /// <value><c>true</c> if completed; otherwise, <c>false</c>.</value>
        public bool Completed { get; private set; }

        /// <summary>
        /// Gets the objective text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestObjective"/> class directly
        /// from the server message (this is a very bad approach)
        /// </summary>
        /// <param name="data">The server message.</param>
        public QuestObjective(string data)
        {
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
            else if(parts[0] == "R")
            {
                Text = "Investigate " + parts[1];
            }

            Completed = (parts[parts.Length - 1] == "CMP") ? true : false;
        }

        /// <summary>
        /// Resets the quest objective.
        /// Since the data is held in string (which is updated from the server message) it is only needed 
        /// to mark this objective as uncompleted.
        /// </summary>
        public void Reset()
        {
            this.Completed = false;
        }
    }
}
