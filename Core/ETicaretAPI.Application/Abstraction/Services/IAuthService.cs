using ETicaretAPI.Application.Abstraction.Services.Authentications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstraction.Services
{
    public interface IAuthService:IExternalAuthentication,IInternalAuthentication
    {
    }
}
