using BlazorBootstrap;

namespace ProjectWeb.ASM.Helper
{
    public static class IconColorHelper
    {
        public static IconColor ParseToIconColor(string colorName)
        {
            if (string.IsNullOrWhiteSpace(colorName))
                return IconColor.None;

            return Enum.TryParse<IconColor>(colorName, true, out var result)
                ? result
                : IconColor.None;
        }

        public static IconName ParseToIconName(string iconName)
        {
            if (string.IsNullOrWhiteSpace(iconName))
                return IconName.None; // o algún valor por defecto

            return Enum.TryParse<IconName>(iconName, true, out var result)
                ? result
                : IconName.None;
        }

    }
}
