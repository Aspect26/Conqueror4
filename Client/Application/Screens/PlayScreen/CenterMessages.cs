using System.Collections.Generic;

namespace Client
{
    /// <summary>
    /// Holds information about central messages. In response to some game actions (e.g.: acquiring a new quest) a message
    /// is shown in the middle of the screen. The message is shown only for some amount of time, this class handles, which
    /// messages shall be shown.
    /// </summary>
    public class CenterMessages
    {
        // TODO: this can be reworked to queue and the message would hold a time when it shall be removed
        private SortedList<long, List<string>> messages;
        private const int DURATION = 5;

        /// <summary>
        /// Initializes a new instance of the <see cref="CenterMessages"/> class.
        /// </summary>
        public CenterMessages()
        {
            this.messages = new SortedList<long, List<string>>();
        }

        /// <summary>
        /// Adds a new message to the messages list.
        /// </summary>
        /// <param name="message">The new message.</param>
        public void AddMessage(string message)
        {
            long key = Extensions.GetCurrentMillis() + DURATION * 1000;
            lock (messages.Values)
            {
                if (messages.ContainsKey(key))
                {
                    messages[key].Add(message);
                }
                else
                {
                    var newList = new List<string>();
                    newList.Add(message);
                    messages.Add(Extensions.GetCurrentMillis() + DURATION * 1000, newList);
                }
            }
        }

        /// <summary>
        /// Gets the messages that shall be displayed.
        /// As an side effect it removes all outdated messages
        /// </summary>
        /// <returns>IList&lt;System.String&gt;.</returns>
        public IList<string> GetMessages()
        {
            // remove outdated messages
            long now = Extensions.GetCurrentMillis();
            lock (messages.Values)
            {
                while (messages.Count > 0)
                {
                    long firstKey = messages.Keys[0];
                    if (firstKey > now)
                        break;

                    messages.Remove(firstKey);
                }
            }

            return getSortedValues();
        }

        private IList<string> getSortedValues()
        {
            IList<string> values = new List<string>();
            lock (messages.Values)
            {
                foreach(KeyValuePair<long, List<string>> value in messages)
                {
                    foreach(string msg in value.Value)
                    {
                        values.Add(msg);
                    }
                }
            }

            return values;
        }
    }
}
