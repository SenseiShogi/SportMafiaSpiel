using System;

namespace SportMafiaSpiel.Models
{
    // Modell für Benutzer in der Datenbank
    public class User
    {
        public int UserID { get; set; }         
        public string Username { get; set; } = "";  
        public string Email { get; set; } = ""; 
        public string PasswordHash { get; set; } = "";  

        public DateTime CreatedAt { get; set; }     
        public bool IsActive { get; set; }          
    }
}
