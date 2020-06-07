using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.Account.Queries.SunriseSunset
{
    public class SunriseSunsetQuery : IRequest<object>
    {
        public float Latitude { get; set; }

        public float Longitude { get; set; }
    }
}
