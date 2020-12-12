using System;
using System.Linq;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace jollyview
{
    public partial class MainForm : Form
    {
        [DllImport("user32.dll")]
        private static extern bool AddClipboardFormatListener(IntPtr hwnd);

        [DllImport("user32.dll")]
        private static extern uint GetClipboardSequenceNumber();

        // color and line width for highlighting
        private static readonly Color HIGHLIGHT_COLOR = Color.LightGreen;
        const int HIGHLIGHT_WIDTH = 3;

        // cooldown between two accepted clipboard changes
        private static readonly TimeSpan CLIPBOARD_COOLDOWN = TimeSpan.FromMilliseconds(200);

        /// <summary>
        /// sequence counter from GetClipboardSequenceNumber() to recognize clipboard changes
        /// </summary>
        private uint m_clipboardSequence = 0;

        /// <summary>
        /// Stopwatch timestamp from when the last image was received
        /// </summary>
        private readonly Stopwatch m_clipboardCooldownTimer = Stopwatch.StartNew();

        /// <summary>
        /// Control over which the cursor hovers, for highlighting
        /// </summary>
        private Control? m_hoverControl = null;

        /// <summary>
        /// Window to display full-sized images
        /// </summary>
        private ImageForm? m_imageForm = null;

        /// <summary>
        /// Zoom levels in percent, indexed by the slider value
        /// </summary>
        private readonly int[] m_zoomLevels;

        public MainForm()
        {
            InitializeComponent();

            // supported zoom levels: 10% - 100% in 10% steps, 100% - 1000% in 100% steps
            var smallRange = Enumerable.Range(1, 10).Select(i => i * 10);
            var largeRange = Enumerable.Range(2, 9).Select(i => i * 100);
            m_zoomLevels = smallRange.Concat(largeRange).ToArray();
            trackBarZoom.Maximum = m_zoomLevels.Length - 1;
            // default should be 100%
            trackBarZoom.Value = smallRange.Count();

            UpdateZoomLevel(); // just to set the text label
            trackBarZoom.MouseWheel += trackBarZoom_MouseWheel;

            // didn't find any way to add menu handlers in the designer UI
            menuItemExit.Click += (sender, e) => Close();
            menuItemShow.Click += (sender, e) => ChangeAllImageVisibility(true);
            menuItemHide.Click += (sender, e) => ChangeAllImageVisibility(false);
            menuItemRemove.Click += (sender, e) => flowLayout.Controls.Clear();
            menuItemAbout.Click += (sender, e) => ShowAboutDialog();

            // register for clipboard change notifications
            AddClipboardFormatListener(Handle);
            m_clipboardSequence = GetClipboardSequenceNumber();
        }

        private void ShowAboutDialog()
        {
            const string description = @"JollyView, a clipboard image viewer

Built for screenshot snippet management for flash escape games of questionable quality.
Made by mooware (dev@mooware.at), 2020.

Double-click left to open an image full-sized.
Double-click right to hide an image.";

            MessageBox.Show(this, description, "About");
        }

        private void ChangeAllImageVisibility(bool visible)
        {
            foreach (var ctrl in flowLayout.Controls.OfType<Control>())
                ctrl.Visible = visible;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StoreSettings();
        }

        private void LoadSettings()
        {
            var prop = Properties.Settings.Default;
            if (!prop.Location.IsEmpty)
                Location = prop.Location;
            if (!prop.Size.IsEmpty)
                Size = prop.Size;

            if (prop.Maximized)
                WindowState = FormWindowState.Maximized;
            else if (prop.Minimized)
                WindowState = FormWindowState.Minimized;

            if (prop.Zoom >= trackBarZoom.Minimum && prop.Zoom <= trackBarZoom.Maximum)
                trackBarZoom.Value = prop.Zoom;
        }

        private void StoreSettings()
        {
            var prop = Properties.Settings.Default;
            prop.Location = (WindowState == FormWindowState.Normal ? Location : RestoreBounds.Location);
            prop.Size = (WindowState == FormWindowState.Normal ? Size : RestoreBounds.Size);
            prop.Maximized = (WindowState == FormWindowState.Maximized);
            prop.Minimized = (WindowState == FormWindowState.Minimized);
            prop.Zoom = trackBarZoom.Value;

            prop.Save();
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_CLIPBOARDUPDATE = 0x031D;
            if (m.Msg == WM_CLIPBOARDUPDATE && Clipboard.ContainsImage())
            {
                var seq = GetClipboardSequenceNumber();
                if (seq == m_clipboardSequence)
                    return;
                m_clipboardSequence = seq;

                // for some reason we still get duplicate images sometimes,
                // so add a bit of a cooldown; humans don't take screenshots that fast
                var elapsed = m_clipboardCooldownTimer.Elapsed;
                if (elapsed < CLIPBOARD_COOLDOWN)
                    return;
                m_clipboardCooldownTimer.Restart();

                // I got a COM exception that the image format is unknown,
                // and I don't think that could possibly be my mistake,
                // so I guess we'll just retry
                const int MAX_TRIES = 3;
                int tries = MAX_TRIES;
                while (tries-- > 0)
                {
                    try
                    {
                        var image = Clipboard.GetImage();
                        if (image != null)
                        {
                            AddImage(image);
                            break;
                        }
                    }
                    catch (COMException)
                    {
                        // ignore
                    }
                }
            }

            // always pass to base class handler
            base.WndProc(ref m);
        }

        private void AddImage(Image image)
        {
            var control = new PictureBox() { Image = image };
            flowLayout.Controls.Add(control);
            flowLayout.Controls.SetChildIndex(control, 0);

            // callbacks to draw a border on mouse-over
            control.MouseEnter += image_MouseEnter;
            control.MouseLeave += image_MouseLeave;
            control.Paint += image_Paint;

            control.MouseDoubleClick += image_MouseDoubleClick;
        }

        private void image_MouseEnter(object? sender, EventArgs e)
        {
            if (sender is Control box && !ReferenceEquals(m_hoverControl, box))
            {
                m_hoverControl = box;
                box.Invalidate();
            }
        }

        private void image_MouseLeave(object? sender, EventArgs e)
        {
            if (sender is Control box && ReferenceEquals(m_hoverControl, box))
            {
                m_hoverControl = null;
                box.Invalidate();
            }
        }

        private void image_Paint(object sender, PaintEventArgs e)
        {
            // no easy way to set a color border, so we have to draw it manually
            if (ReferenceEquals(sender, m_hoverControl))
            {
                using var pen = new Pen(HIGHLIGHT_COLOR, HIGHLIGHT_WIDTH);
                var size = m_hoverControl.Size;
                // not sure where it draws exactly, but it looks like I have to shrink the rect a bit
                var offset = HIGHLIGHT_WIDTH / 2;
                e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, size.Width - offset, size.Height - offset));
            }
        }

        private void image_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (sender is PictureBox box)
            {
                if (e.Button == MouseButtons.Left)
                    OpenImageWindow(box.Image);
                else if (e.Button == MouseButtons.Right)
                    box.Visible = false;
            }
        }

        private void OpenImageWindow(Image image)
        {
            if (m_imageForm is null || m_imageForm.IsDisposed)
                m_imageForm = new ImageForm();

            m_imageForm.SetImage(image);
            m_imageForm.Show(this);
            m_imageForm.BringToFront();
        }

        private void flowLayout_Layout(object sender, LayoutEventArgs e)
        {
            var layout = sender as FlowLayoutPanel;
            if (layout is null)
                return;

            var zoom = m_zoomLevels[trackBarZoom.Value] / 100.0;

            foreach (var control in flowLayout.Controls)
            {
                var img = control as PictureBox;
                if (img is null || img.Image is null)
                    continue;

                var ratio = ((double)img.Image.Height) / img.Image.Width;
                var zoomedImgWidth = (int)(img.Image.Width * zoom);

                const int MIN_IMG_WIDTH = 10;
                zoomedImgWidth = Math.Max(zoomedImgWidth, MIN_IMG_WIDTH);

                // can't figure out the full width I can use, reduce by a few %
                var layoutWidth = (int)(layout.ClientSize.Width * 0.96);
                // I want maximum width. can't the layout engine do this for me?
                var newMaxWidth = Math.Min(zoomedImgWidth, layoutWidth);
                var scaledHeight = (int)(newMaxWidth * ratio);

                img.MinimumSize = img.MaximumSize = new Size(newMaxWidth, scaledHeight);
                img.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        /// <summary>
        /// Custom mouse wheel handler, because I only want to move a single step per event
        /// </summary>
        private void trackBarZoom_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e is HandledMouseEventArgs he)
                he.Handled = true;

            if (e.Delta > 0 && trackBarZoom.Value < trackBarZoom.Maximum)
                trackBarZoom.Value += 1;
            else if (e.Delta < 0 && trackBarZoom.Value > trackBarZoom.Minimum)
                trackBarZoom.Value -= 1;
        }

        private void trackBarZoom_ValueChanged(object sender, EventArgs e)
        {
            UpdateZoomLevel();
        }

        private void UpdateZoomLevel()
        {
            var zoom = m_zoomLevels[trackBarZoom.Value];
            labelZoom.Text = $"{zoom}%";
            flowLayout.PerformLayout();
        }
    }
}
