using TIBCO.EMS;

namespace Provider.Messaging.Tibco
{
    public class TibcoTextMessage : ITextMessage
    {
        public TibcoTextMessage(TextMessage msg)
        {
            TibcoMsg = msg;
        }

        public TextMessage TibcoMsg { get; private set; }

        public string Text
        {
            get { return TibcoMsg.Text; }
            set { TibcoMsg.Text = value; }
        }

        public void ConfirmRecieve()
        {
            TibcoMsg.Acknowledge();
        }
    }
}