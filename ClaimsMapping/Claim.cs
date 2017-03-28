using System;

namespace ClaimsMapping
{
    public class Claim
    {
        public enum DamageType
        {
            Lost,
            Damaged,
            Stolen
        }

        public string DamagedItem { get; set; }
        public DamageType? TypeOfDamage { get; set; }
        public string DamageLocation { get; set; }
        public DateTime? DateOfDamage { get; set; }
        public string PolicyNumber { get; set; }
        public string Email { get; set; }
    }
}
