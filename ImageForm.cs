using System.Drawing;
using System.Windows.Forms;

namespace jollyview
{
    public partial class ImageForm : Form
    {
        public ImageForm()
        {
            InitializeComponent();
        }

        public void SetImage(Image image)
        {
            pictureBox.Image = image;
        }
    }
}
