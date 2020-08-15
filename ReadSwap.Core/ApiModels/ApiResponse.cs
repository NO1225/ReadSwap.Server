using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ReadSwap.Core.ApiModels
{
    public class ApiResponse
    {
        public bool Success => Errors == null;

        public List<int> Errors { get; private set; }

        public void AddError(int error)
        {
            if (Errors == null)
                Errors = new List<int>();

            Errors.Add(error);
        }

        public void AddErrors(List<int> errors)
        {
            if (Errors == null)
                Errors = new List<int>();

            Errors.AddRange(errors);
        }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T Data { get; set; }
    }
}
