﻿using WF.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF.Core.Contract
{
    public interface IModel
    {
        string Error { get; set; }
    }
}