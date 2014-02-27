using System.Text;

namespace Provider.Logging.Mail
{
    public static class Helper
    {
        public static string LineBreaks(string str)
        {
            StringBuilder sb = new StringBuilder(str);
            sb.Replace("<", "&lt;");
            sb.Replace(">", "&gt;");
            sb.Replace("\r\n", @"<br/>");
            return sb.ToString();
        }
    }
}