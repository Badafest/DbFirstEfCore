﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Linq;
using DbFirstEfCore.Data.Models;

namespace DbFirstEfCore.Data.Models
{
    public partial class AdventureworksContext
    {

        [DbFunction("ufnGetAllCategories", "dbo")]
        public IQueryable<VGetAllCategory> ufnGetAllCategories()
        {
            return FromExpression(() => ufnGetAllCategories());
        }

        [DbFunction("ufnGetCustomerInformation", "dbo")]
        public IQueryable<VCustomerInformation> ufnGetCustomerInformation(int? CustomerID)
        {
            return FromExpression(() => ufnGetCustomerInformation(CustomerID));
        }

        [DbFunction("ufnGetSalesOrderStatusText", "dbo")]
        public static string ufnGetSalesOrderStatusText(byte? Status)
        {
            throw new NotSupportedException("This method can only be called from Entity Framework Core queries");
        }

        protected void OnModelCreatingGeneratedFunctions(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VGetAllCategory>().HasNoKey();
            modelBuilder.Entity<VCustomerInformation>().HasNoKey();
            modelBuilder.Entity<TestProcedureResult>().HasNoKey(); 
        }
    }
}