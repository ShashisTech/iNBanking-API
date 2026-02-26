using System;
using System.Collections.Generic;

namespace BakingAPI.Models;

public partial class Configuration
{
    public string ConfigKey { get; set; } = null!;

    public string ConfigValue { get; set; } = null!;
}
