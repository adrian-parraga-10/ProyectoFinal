using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Hash
{
    public static class PasswordHelper
    {
        private static readonly PasswordHasher<string> hasher = new PasswordHasher<string>();

        public static string HashearContraseña(string contraseña)
        {
            return hasher.HashPassword(null, contraseña);
        }

        public static bool VerificarContraseña(string contraseñaIngresada, string hashAlmacenado)
        {
            var resultado = hasher.VerifyHashedPassword(null, hashAlmacenado, contraseñaIngresada);
            return resultado == PasswordVerificationResult.Success;
        }
    }
}
