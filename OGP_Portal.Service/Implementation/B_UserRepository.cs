using OGP_Portal.Data.DbContext;
using OGP_Portal.Data.DbModel;
using OGP_Portal.Service.Implementation.BaseService;
using OGP_Portal.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace OGP_Portal.Service.Implementation
{
    public class B_UserRepository : GenericRepository<B_User>, IB_UserService
    {
        private readonly OGP_PortalContext _context;
        public B_UserRepository(OGP_PortalContext context) : base(context)
        {
            _context = context;
        }

      
    }
}