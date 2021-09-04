using RodnaPamet.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RodnaPamet.Services
{
    public interface IUploadService
    {
        bool UploadFile(Page Page, Item File);
    }
}
