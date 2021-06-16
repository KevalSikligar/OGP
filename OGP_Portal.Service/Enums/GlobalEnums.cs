using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace JunTechnology.Dto.Enums
{
    public class GlobalEnums
    {
        public enum Priority
        {
            [Description("High")]
            High = 1,
            [Description("Medium")]
            Medium = 2,
            [Description("Low")]
            Low = 3
        }
        public enum TicketStatus
        {
            [Description("Open")]
            Open = 1,
            [Description("Hold")]
            Hold = 2,
            [Description("Closed")]
            Closed = 3,
            [Description("Active")]
            Active = 4,
            [Description("Archived")]
            Archived = 5

        }

    }
}
