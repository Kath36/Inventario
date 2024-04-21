using System;
using System.Collections.Generic;

namespace Inventario.Core.Http
{
    public class Response<T>
    {
        private T _data;
        public T Data
        {
            get => _data;
            set
            {
                // Verificar si T es un tipo int y si el valor asignado es una cadena
                if (value is int && value.GetType() != typeof(int))
                {
                    throw new ArgumentException("El tipo de dato no es válido para el campo.");
                }
                _data = value;
            }
        }

        public string Message { get; set; } = "";
        public List<string> Errors { get; set; } = new List<string>();
    }
}