﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RodnaPamet
{
    public interface IImageResource
    {
        Size GetSize(string fileName);
    }
}
