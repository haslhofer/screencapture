using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace screencapture
{
    public class NotifyUser
    {
        public static void ShowNonModalMessage(string caption, string msg)
        {
            new Thread(new ThreadStart(delegate
            {
                MessageBox.Show
                (
                msg,
                caption,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
                );
            })).Start();
        }
    }
}
