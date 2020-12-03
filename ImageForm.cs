using System.Drawing;
using System.Windows.Forms;

namespace jollyview
{
    public partial class ImageForm : Form
    {
        public ImageForm()
        {
            InitializeComponent();

            // for some reason this event is not listed in the designer
            pictureBox.MouseWheel += pictureBox_MouseWheel;
        }

        private void pictureBox_MouseWheel(object sender, MouseEventArgs e)
        {
            // zoom steps are 10% of image size
            int zoomWidthIncrement = pictureBox.Image.Width / 10;
            int zoomHeightIncrement = pictureBox.Image.Height / 10;

            int wheelDirection = (e.Delta > 0 ? +1 : -1);
            int widthDelta = (zoomWidthIncrement * wheelDirection);
            int heightDelta = (zoomHeightIncrement * wheelDirection);

            SuspendLayout();

            // for some reason AutoSize doesn't work if I try to change sizes,
            // and the window shrinks to 0x0, so change size manually

            // don't shrink to zero manually either
            if (widthDelta > 0 || ClientSize.Width > zoomWidthIncrement)
                Width += widthDelta;
            if (heightDelta > 0 || ClientSize.Height > zoomHeightIncrement)
                Height += heightDelta;
            AutoSize = false;

            if (widthDelta > 0 || pictureBox.Width > zoomWidthIncrement)
                pictureBox.Width += widthDelta;
            if (heightDelta > 0 || pictureBox.Height > zoomHeightIncrement)
                pictureBox.Height += heightDelta;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;

            ResumeLayout();
        }

        public void SetImage(Image image)
        {
            SuspendLayout();

            pictureBox.Image = image;
            pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            AutoSize = true;

            ResumeLayout();
        }
    }
}
