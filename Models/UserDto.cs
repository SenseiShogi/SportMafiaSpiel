using System;

namespace SportMafiaSpiel.Models
{
    // Dieser Klasse wird nur für die API-Antwort verwendet
    public class UserDto
    {
        public int UserID { get; set; }       // Primärschlüssel
        public string Username { get; set; } = string.Empty;  // Benutzername
        public string Email { get; set; } = string.Empty;     // E-Mail-Adresse
        public DateTime CreatedAt { get; set; } // Erstellungsdatum
        public bool IsActive { get; set; }    // Aktiv/Deaktiviert
    }
}
