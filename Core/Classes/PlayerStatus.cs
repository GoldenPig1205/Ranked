﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ranked.Core.Classes
{
    public class PlayerStatus
    {
        public bool IsSitDown { get; set; } = false;
        public bool IsChangingSitDownState { get; set; } = false;
    }
}
