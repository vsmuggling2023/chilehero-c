using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chilehero_c
{
    public static class Session
    {
        public static int UserId { get; set; } = 0;
        public static string Correo { get; set; } = null;
        public static string Nombre { get; set; } = null;
        public static string Rol { get; set; } = null;

        public static void Clear()
        {
            UserId = 0;
            Correo = null;
            Nombre = null;
            Rol = null;
        }
    }
}