using System.Windows.Forms;

namespace Portaflex
{
    public abstract partial class AbstractPage : UserControl
    {
        public AbstractPage()
        {
            InitializeComponent();
            InitializeButtons();
        }

        protected abstract void InitializeButtons();
    }
}
