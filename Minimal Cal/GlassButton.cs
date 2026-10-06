using System;
using System.Drawing;
using System.Windows.Forms;

namespace Minimal_Cal
{
    // Flat, borderless button whose BackColor may carry an alpha value.
    // A stock Button paints its back colour opaquely and smears when alpha is stacked on
    // repaint, so this one resets its area to transparent and then lays the translucent fill
    // on top, every time.
    class GlassButton : Button
    {
        // Extra alpha added while hovered / pressed so the buttons still give feedback
        public int HoverAlphaBoost { get; set; } = 40;
        public int PressedAlphaBoost { get; set; } = 80;

        bool hovered;
        bool pressed;

        public GlassButton()
        {
            SetStyle(ControlStyles.UserPaint
                   | ControlStyles.AllPaintingInWmPaint
                   | ControlStyles.ResizeRedraw
                   | ControlStyles.SupportsTransparentBackColor, true);

            // No OptimizedDoubleBuffer: it would composite onto an opaque buffer and lose the transparency
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            UseVisualStyleBackColor = false;
        }

        protected override void OnMouseEnter(EventArgs e) { hovered = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { hovered = false; pressed = false; Invalidate(); base.OnMouseLeave(e); }

        protected override void OnMouseDown(MouseEventArgs e) { pressed = true; Invalidate(); base.OnMouseDown(e); }
        protected override void OnMouseUp(MouseEventArgs e) { pressed = false; Invalidate(); base.OnMouseUp(e); }

        // Don't ask the parent to repaint behind us: with a transparent TableLayoutPanel that
        // drags the neighbouring buttons' text in as ghosts. OnPaint resets the area itself.
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            int boost = pressed ? PressedAlphaBoost : hovered ? HoverAlphaBoost : 0;
            Color fill = Color.FromArgb(Math.Min(255, BackColor.A + boost), BackColor);

            // Reset to fully transparent so every repaint starts from the same clean backdrop
            // and the alpha fill never stacks on its previous frame
            e.Graphics.Clear(Color.Transparent);

            using (SolidBrush brush = new SolidBrush(fill))
            {
                e.Graphics.FillRectangle(brush, ClientRectangle);
            }

            TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle, ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                | TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix);
        }
    }
}
