using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RodnaPamet
{
    public interface ITextSize
    {
        double GetFontSize(string Text, string Font, double Width, double Height);
    }
}
