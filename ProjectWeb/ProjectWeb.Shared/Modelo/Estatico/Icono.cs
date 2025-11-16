namespace ProjectWeb.Shared.Modelo.Estatico
{
    public class Icono
    {
        public string Id { get; set; }
        public string? Text { get; set; }
        public static List<Icono> GetItems() => new()
{
    // Menú y Navegación
    new Icono { Id = "1", Text = "House" },
    new Icono { Id = "2", Text = "Person" },
    new Icono { Id = "3", Text = "People" },
    new Icono { Id = "4", Text = "Gear" },
    new Icono { Id = "5", Text = "GearWide" },
    new Icono { Id = "6", Text = "Bell" },
    new Icono { Id = "7", Text = "Envelope" },
    new Icono { Id = "8", Text = "Chat" },
    new Icono { Id = "9", Text = "Folder" },
    new Icono { Id = "10", Text = "FileEarmark" },
    new Icono { Id = "11", Text = "Calendar" },
    new Icono { Id = "12", Text = "BarChart" },
    new Icono { Id = "13", Text = "PieChart" },
    new Icono { Id = "14", Text = "Cart" },
    new Icono { Id = "15", Text = "Search" },
    new Icono { Id = "16", Text = "QuestionCircle" },
    new Icono { Id = "17", Text = "InfoCircle" },
    new Icono { Id = "18", Text = "Lock" },
    new Icono { Id = "19", Text = "BoxArrowRight" },

    // Acciones comunes
    new Icono { Id = "20", Text = "Plus" },
    new Icono { Id = "21", Text = "Dash" },
    new Icono { Id = "22", Text = "Check" },
    new Icono { Id = "23", Text = "X" },
    new Icono { Id = "24", Text = "Pencil" },
    new Icono { Id = "25", Text = "Trash" },
    new Icono { Id = "26", Text = "Save" },
    new Icono { Id = "27", Text = "CloudArrowDown" },
    new Icono { Id = "28", Text = "CloudArrowUp" },
    new Icono { Id = "29", Text = "ArrowClockwise" },
    new Icono { Id = "30", Text = "ArrowCounterclockwise" },
    new Icono { Id = "31", Text = "ArrowReturnLeft" },
    new Icono { Id = "32", Text = "PlusCircle" },
    new Icono { Id = "33", Text = "DashCircle" },
    new Icono { Id = "34", Text = "CheckCircle" },
    new Icono { Id = "35", Text = "XCircle" },

    // Estados y Alertas
    new Icono { Id = "36", Text = "ExclamationTriangle" },
    new Icono { Id = "37", Text = "ExclamationCircle" },
    new Icono { Id = "38", Text = "Info" },
    new Icono { Id = "39", Text = "CheckCircle" },
    new Icono { Id = "40", Text = "XCircle" },
    new Icono { Id = "41", Text = "QuestionCircle" },

    // Multimedia
    new Icono { Id = "42", Text = "Camera" },
    new Icono { Id = "43", Text = "CameraVideo" },
    new Icono { Id = "44", Text = "Play" },
    new Icono { Id = "45", Text = "Pause" },
    new Icono { Id = "46", Text = "Stop" },
    new Icono { Id = "47", Text = "VolumeUp" },
    new Icono { Id = "48", Text = "VolumeMute" },
    new Icono { Id = "49", Text = "Image" },
    new Icono { Id = "50", Text = "MusicNote" },

    // Formularios
    new Icono { Id = "51", Text = "Clipboard" },
    new Icono { Id = "52", Text = "ClipboardCheck" },
    new Icono { Id = "53", Text = "ClipboardX" },
    new Icono { Id = "54", Text = "FileText" },
    new Icono { Id = "55", Text = "FileTextFill" },
    new Icono { Id = "56", Text = "InputCursorText" },
    new Icono { Id = "57", Text = "Textarea" },
    new Icono { Id = "58", Text = "CheckSquare" },
    new Icono { Id = "59", Text = "Circle" },
    new Icono { Id = "60", Text = "ToggleOn" },

    // Ecommerce / Pagos
    new Icono { Id = "61", Text = "Cart" },
    new Icono { Id = "62", Text = "CartCheck" },
    new Icono { Id = "63", Text = "CartPlus" },
    new Icono { Id = "64", Text = "CashStack" },
    new Icono { Id = "65", Text = "CreditCard" },
    new Icono { Id = "66", Text = "Bag" },
    new Icono { Id = "67", Text = "BagCheck" },
    new Icono { Id = "68", Text = "Paypal" },
    new Icono { Id = "69", Text = "CurrencyBitcoin" },
    new Icono { Id = "70", Text = "Wallet" },
    new Icono { Id = "71", Text = "Facebook" },
    new Icono { Id = "72", Text = "Twitter" },
    new Icono { Id = "73", Text = "Instagram" },
    new Icono { Id = "74", Text = "Linkedin" },
    new Icono { Id = "75", Text = "Youtube" },
    new Icono { Id = "76", Text = "Whatsapp" },
    new Icono { Id = "77", Text = "Envelope" },

    // Reportes / Estadísticas
    new Icono { Id = "78", Text = "FileBarGraph" },
    new Icono { Id = "79", Text = "ClipboardData" },
    new Icono { Id = "80", Text = "BarChart" },
    new Icono { Id = "81", Text = "PieChart" },
    new Icono { Id = "82", Text = "BarChartLine" },

    // Pantalla / Visualización
    new Icono { Id = "83", Text = "Amazon" },
    new Icono { Id = "84", Text = "Bag" },
    new Icono { Id = "85", Text = "Fullscreen" },
    new Icono { Id = "86", Text = "FullscreenExit" },

    // Autenticación / Login / Registro
    new Icono { Id = "87", Text = "PersonCircle" },        // Usuario genérico
    new Icono { Id = "88", Text = "BoxArrowInRight" }, // Login / Iniciar sesión
    new Icono { Id = "89", Text = "PersonPlus" },    // Registro / Crear cuenta
    new Icono { Id = "90", Text = "Lock" },          // Seguridad / Contraseña
    new Icono { Id = "91", Text = "Key" } ,           // Autenticación / Llave


    new Icono { Id = "92", Text = "Globe" },    // Globo / Internacional
    new Icono { Id = "93", Text = "List" },     // Menú / Navegación
    new Icono { Id = "94", Text = "Building" },
    new Icono { Id = "95", Text = "Grid" } // Dashboard / Panel de control// Empresa / Corporativo
};



        public static string GetTextIcono(string id)
        {
            var estado = GetItems().FirstOrDefault(e => e.Id == id);
            return estado?.Text ?? string.Empty; // Retorna vacío si no encuentra
        }

    }
}
