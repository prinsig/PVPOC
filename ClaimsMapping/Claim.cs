using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public DamageType TypeOfDamage { get; set; }
        public string DamageLocation { get; set; }
        public string DateOfDamage { get; set; }
        public string PolicyNumber { get; set; }
        public string Email { get; set; }
    }
}
