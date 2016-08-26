using System.Net.Sockets;
using System.Text;

namespace Server
{
    /// <summary>
    /// This object represents current state of a client in the client-server.
    /// </summary>
    public class StateObject
    {
        /// <summary>
        /// The client's socket
        /// </summary>
        public Socket clientSocket = null;

        /// <summary>
        /// The buffer size
        /// </summary>
        public static int BufferSize = 1024;

        /// <summary>
        /// The data buffer
        /// </summary>
        public byte[] buffer = new byte[BufferSize];

        /// <summary>
        /// The received data builder. The whole message may not come in one
        /// packet so this builder is used to connect packets's data into one
        /// message if it is supposed to be one.
        /// </summary>
        public StringBuilder dataReceived = new StringBuilder();

        /// <summary>
        /// Gets or sets the account associated with this client.
        /// </summary>
        /// <value>The account.</value>
        public Account Account { get; set; }

        /// <summary>
        /// Gets or sets the character the client chose to play.
        /// </summary>
        /// <value>The playing character.</value>
        public Character PlayingCharacter { get; set; }
    }
}
