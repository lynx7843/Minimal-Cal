using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Minimal_Cal
{
    // Native Windows 10/11 acrylic blur via the undocumented SetWindowCompositionAttribute
    static class AcrylicHelper
    {
        enum AccentState
        {
            Disabled = 0,
            EnableBlurBehind = 3,          // Windows 10 (all builds)
            EnableAcrylicBlurBehind = 4    // Windows 10 1803+ and Windows 11
        }

        [StructLayout(LayoutKind.Sequential)]
        struct AccentPolicy
        {
            public AccentState AccentState;
            public int AccentFlags;
            public int GradientColor;      // AABBGGRR
            public int AnimationId;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct WindowCompositionAttributeData
        {
            public int Attribute;
            public IntPtr Data;
            public int SizeOfData;
        }

        const int WCA_ACCENT_POLICY = 19;

        [DllImport("user32.dll")]
        static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

        // Returns false if the OS doesn't support it (the form then just stays a normal dark window)
        public static bool Enable(Form form, Color tint)
        {
            // Layout is AABBGGRR; the alpha here is what controls how strong the tint is
            int gradient = (tint.A << 24) | (tint.B << 16) | (tint.G << 8) | tint.R;

            var accent = new AccentPolicy
            {
                AccentState = AccentState.EnableAcrylicBlurBehind,
                AccentFlags = 2,           // draw the gradient colour over the whole window
                GradientColor = gradient
            };

            int size = Marshal.SizeOf(accent);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(accent, ptr, false);
                var data = new WindowCompositionAttributeData
                {
                    Attribute = WCA_ACCENT_POLICY,
                    Data = ptr,
                    SizeOfData = size
                };
                return SetWindowCompositionAttribute(form.Handle, ref data) != 0;
            }
            catch (EntryPointNotFoundException)
            {
                return false;
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}
