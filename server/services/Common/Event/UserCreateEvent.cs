﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Event;

public class UserCreateEvent
{
    public string UserId { get; set; } = null!;

    public string? DisplayName { get; set; }
}
