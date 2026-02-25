using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreciseInVoice.Domain.Enums
{
    public enum PaymentMethod
    {
        VirementBancaire = 0,  // Bank transfer
        Cheque = 1,
        Especes = 2,           // Cash
        CarteBancaire = 3,     // Credit card
        Autre = 4              // Other
    }

}
