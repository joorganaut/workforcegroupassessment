using AXAMansard.Framework.DTO;
using WF.DAO;
using WF.DAO.Common;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace WF.Service
{
    public class NinjectConfiguration : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(INHBusinessObjectDAO<>)).To(typeof(NHBusinessObjectDAO<>));
            Bind(typeof(INPBusinessObjectDAO<>)).To(typeof(NPBusinessObjectDAO<>));
            Bind(typeof(IDIDAOImplementation<>)).To(typeof(DIDAOImplementation<>));
        }

    }
}
