namespace SportMafiaSpiel.Models
{
    // Modell für Spielergebnisse in der Datenbank
    public class Result
    {
        public int ResultID { get; set; }        
        public int SessionID { get; set; }       
        public int Score { get; set; }           
        public int DurationSeconds { get; set; } 
        public int Accuracy { get; set; }        
    }
}
