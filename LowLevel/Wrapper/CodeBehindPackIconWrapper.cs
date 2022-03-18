using MaterialDesignThemes.Wpf;

namespace MupenToolkitPRE.LowLevel.Wrapper
{
    public static class CodeBehindPackIconWrapper
    {
        public static PackIcon FromKind(PackIconKind Kind, int Size)
        {
            PackIcon packIcon = new()
            {
                Kind = Kind
            };
            packIcon.Height = Size;
            packIcon.Width = Size;
            return packIcon;
        }
    }
}
