using System;

namespace SportMafiaSpiel.Models
{
    // Modell für Spielsitzungen in der Datenbank
    public class Session
    {
        public int SessionID { get; set; }       
        public int UserID { get; set; }          
        public int GameID { get; set; }          
        public DateTime StartedAt { get; set; }  
        public DateTime? FinishedAt { get; set; } 
    }
}
