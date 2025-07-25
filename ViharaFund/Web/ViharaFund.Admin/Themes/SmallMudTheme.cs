using MudBlazor;

namespace ViharaFund.Admin.Themes
{
    public static class CustomMudTheme
    {
        public static readonly MudTheme CompactTheme = new()
        {
            PaletteLight = new PaletteLight()
            {
                Primary = "#1976d2",
                Secondary = "#424242",
                AppbarBackground = "#1976d2",
                Background = "#ffffff",
                BackgroundGray = "#f5f5f5",
                Surface = "#ffffff",
                DrawerBackground = "#ffffff",
                DrawerText = "rgba(0,0,0, 0.87)",
                DrawerIcon = "rgba(0,0,0, 0.54)",
                AppbarText = "rgba(255,255,255, 0.87)",
                TextPrimary = "rgba(0,0,0, 0.87)",
                TextSecondary = "rgba(0,0,0, 0.54)",
                ActionDefault = "rgba(0,0,0, 0.54)",
                ActionDisabled = "rgba(0,0,0, 0.26)",
                ActionDisabledBackground = "rgba(0,0,0, 0.12)",
                Divider = "rgba(0,0,0, 0.12)",
                DividerLight = "rgba(0,0,0, 0.06)",
                TableLines = "rgba(0,0,0, 0.12)",
                LinesDefault = "rgba(0,0,0, 0.12)",
                LinesInputs = "rgba(0,0,0, 0.42)",
                TextDisabled = "rgba(0,0,0, 0.38)",
                Info = "#2196f3",
                Success = "#4caf50",
                Warning = "#ff9800",
                Error = "#f44336",
                Dark = "#424242"
            },

            PaletteDark = new PaletteDark()
            {
                Primary = "#90caf9",
                Secondary = "#f48fb1",
                AppbarBackground = "#1e1e1e",
                Background = "#121212",
                BackgroundGray = "#1e1e1e",
                Surface = "#1e1e1e",
                DrawerBackground = "#1e1e1e",
                DrawerText = "rgba(255,255,255, 0.87)",
                DrawerIcon = "rgba(255,255,255, 0.54)",
                AppbarText = "rgba(255,255,255, 0.87)",
                TextPrimary = "rgba(255,255,255, 0.87)",
                TextSecondary = "rgba(255,255,255, 0.54)",
                ActionDefault = "rgba(255,255,255, 0.54)",
                ActionDisabled = "rgba(255,255,255, 0.26)",
                ActionDisabledBackground = "rgba(255,255,255, 0.12)",
                Divider = "rgba(255,255,255, 0.12)",
                DividerLight = "rgba(255,255,255, 0.06)",
                TableLines = "rgba(255,255,255, 0.12)",
                LinesDefault = "rgba(255,255,255, 0.12)",
                LinesInputs = "rgba(255,255,255, 0.42)",
                TextDisabled = "rgba(255,255,255, 0.38)",
                Info = "#64b5f6",
                Success = "#81c784",
                Warning = "#ffb74d",
                Error = "#e57373",
                Dark = "#f5f5f5"
            },

            LayoutProperties = new LayoutProperties()
            {
                DrawerWidthLeft = "200px",
                DrawerWidthRight = "200px",
                AppbarHeight = "48px", // Reduced from default 64px
                DefaultBorderRadius = "3px" // Reduced from default 4px
            },

            // Typography customization is handled via CSS for better compatibility
            // The compact-mudblazor.css file contains all font size adjustments
        };
    }
}
