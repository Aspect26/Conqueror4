using System.Collections.Generic;

namespace Client
{
    public class CenterMessages
    {
        private SortedList<long, List<string>> messages;
        private const int DURATION = 5;

        public CenterMessages()
        {
            this.messages = new SortedList<long, List<string>>();
        }

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
