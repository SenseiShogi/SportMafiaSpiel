using System;

namespace SportMafiaSpiel.Models
{
    // Modell für Spiele in der Datenbank
    public class Game
    {
        public int GameID { get; set; }          
        public string Name { get; set; } = "";   
        public string Description { get; set; } = ""; 
        public bool IsActive { get; set; }       
        public DateTime CreatedAt { get; set; }  
    }
}
