﻿using System;
using Pds.Core.Enums;

namespace Pds.Api.Contracts.Cost
{
    public class CostDto
    {
        public Guid Id { get; set; }

        public decimal Value { get; set; }

        public DateTime PaidAt { get; set; }

        public CostType Type { get; set; }

        public string Comment { get; set; }

        public CostContentDto Content { get; set; }
    }
}