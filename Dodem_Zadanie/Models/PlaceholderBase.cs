namespace Dodem_Zadanie.Models
{
    public class PlaceholderBase
    {
        //Zamysł za zmiennymi nullable jest taki, że nie trzeba obsługiwać wszystkich zmiennych,
        //a tylko te które są potrzebne, używając jednego modelu
        public required string Name { get; set; }
        public string? CourseTitle { get; set; }
        public string? CertificateLink { get; set; }
        public  string? PlatformLink { get; set; }
        public DateTime? StartDate { get; set; }
        public string? CourseLink { get; set; }
    }
}
