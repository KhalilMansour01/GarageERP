using System;
using System.Collections.Generic;
using System.Text;

namespace GarageERP.Domain.Constants;

public static class ContractStatus
{
    public const int Active = 1;
    public const int Closed = 2;
    public const int Expired = 3;

    // Future:
    // public const int Suspended = 3;
    // public const int Cancelled = 4;
}