namespace SharedWhiteBoard.Models
{
    // TODO Add to database as well
    public class Session
    {
        /// <summary>
        /// Auto-incremented session id. To be removed when database introduced.
        /// </summary>
        private static long _lastSessionPin = 0;

        public long SessionPin { get; private set; }

        public bool BothParticipantsJoined { get; set; }

        public bool IsActive { get; set; }

        public Session()
        {
            SessionPin = ++_lastSessionPin;
            BothParticipantsJoined = false;
            IsActive = true;
        }
    }
}