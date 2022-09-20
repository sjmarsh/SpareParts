﻿using Microsoft.AspNetCore.Mvc;
using SpareParts.API.Services;
using SpareParts.Shared.Models;

namespace SpareParts.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthenticationService _authenticatinService;
        
        public UserController(IAuthenticationService authenticatinService)
        {
            _authenticatinService = authenticatinService;
        }

        [HttpPost("authenticate")]
        public AuthenticationResponse Authenticate(AuthenticationRequest authenticationRequest)
        {
            return _authenticatinService.Authenticate(authenticationRequest);
        }
    }
}