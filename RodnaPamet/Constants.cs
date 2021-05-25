using System;
using System.Collections.Generic;
using System.Text;

namespace RodnaPamet
{
    public static class Constants
    {
        // The iOS simulator can connect to localhost. However, Android emulators must use the 10.0.2.2 special alias to your host loopback interface.
        public static string BaseAddress = "https://api.rodnapamet.bg";
        public static string UsersUrl = BaseAddress + "/users";
        public static string AuthUrl = BaseAddress + "/auth";

        public static string UploadUrl = BaseAddress + "/fileUpload";
    }
}
