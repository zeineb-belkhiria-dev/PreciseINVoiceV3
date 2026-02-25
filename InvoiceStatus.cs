using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreciseInVoice.Domain.Enums
{
    public enum InvoiceStatus
    {
        Brouillon = 0,
        Envoyee = 1,
        Enattente = 2,
        Payee = 3,
        Enretard = 4,
        Anuulee = 5
    }
}
