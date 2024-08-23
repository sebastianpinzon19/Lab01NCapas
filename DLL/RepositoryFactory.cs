﻿using DAL.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class RepositoryFactory
    {

        public static IRepository CreateRepository()
        {
            return new EFRepository(new Datos.ApplicationDbContext());
        }

       // public static IRepository CreateRepository()
      //{
  //        return new EFRepository(new Datos.ApplicationDbContext());
      //}
    }
}
