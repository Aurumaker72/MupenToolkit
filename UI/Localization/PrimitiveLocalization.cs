namespace MupenToolkitPRE.UI.Localization
{
    public static class PrimitiveLocalization
    {
        public static string ToStringResponse(this bool val)
        {
            return val ? Properties.Resources.Yes : Properties.Resources.No;
        }
    }
}
