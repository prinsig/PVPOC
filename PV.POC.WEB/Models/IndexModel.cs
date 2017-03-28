namespace PV.POC.WEB.Models
{
    public class IndexModel
    {
        public string InputText { get; set; }

        public bool IsHidden { get; set; } = true;

        public Claim Claim { get; set; }
        
    }
}