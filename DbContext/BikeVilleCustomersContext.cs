using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BikeVille;

public partial class BikeVilleCustomersContext : DbContext
{
    public BikeVilleCustomersContext()
    {
    }

    public BikeVilleCustomersContext(DbContextOptions<BikeVilleCustomersContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerAddress> CustomerAddresses { get; set; }

    public virtual DbSet<SalesOrderDetail> SalesOrderDetails { get; set; }

    public virtual DbSet<SalesOrderHeader> SalesOrderHeaders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=FRA-PC\\SQLEXPRESS;Initial Catalog=BikeVilleCustomers;Integrated Security=True;Connect Timeout=30;Encrypt=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.ToTable("Address");

            entity.Property(e => e.AddressId).HasColumnName("AddressID");
            entity.Property(e => e.AddressLine1)
                .HasMaxLength(60)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.AddressLine2)
                .HasMaxLength(60)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.City)
                .HasMaxLength(30)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.CountryRegion)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.PostalCode)
                .HasMaxLength(15)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Rowguid).HasColumnName("rowguid");
            entity.Property(e => e.StateProvince)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(128)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.EmailAddress)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(128)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.PasswordSalt)
                .HasMaxLength(10)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Phone)
                .HasMaxLength(25)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Rowguid).HasColumnName("rowguid");
            entity.Property(e => e.SalesPerson)
                .HasMaxLength(256)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Suffix)
                .HasMaxLength(10)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Title)
                .HasMaxLength(8)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<CustomerAddress>(entity =>
        {
            entity.HasKey(e => new { e.CustomerId, e.AddressId });

            entity.ToTable("CustomerAddress");

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.AddressId).HasColumnName("AddressID");
            entity.Property(e => e.AddressType)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Rowguid).HasColumnName("rowguid");

            entity.HasOne(d => d.Address).WithMany(p => p.CustomerAddresses)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerAddress_Address");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerAddresses)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerAddress_Customer");
        });

        modelBuilder.Entity<SalesOrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.SalesOrderId, e.SalesOrderDetailId });

            entity.ToTable("SalesOrderDetail", tb => tb.HasTrigger("trg_CheckProductID"));

            entity.Property(e => e.SalesOrderId)
                .ValueGeneratedOnAdd()
                .HasColumnName("SalesOrderID");
            entity.Property(e => e.SalesOrderDetailId).HasColumnName("SalesOrderDetailID");
            entity.Property(e => e.LineTotal).HasColumnType("numeric(38, 6)");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Rowguid).HasColumnName("rowguid");
            entity.Property(e => e.UnitPrice).HasColumnType("money");
            entity.Property(e => e.UnitPriceDiscount).HasColumnType("money");

            entity.HasOne(d => d.SalesOrder).WithMany(p => p.SalesOrderDetails)
                .HasForeignKey(d => d.SalesOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOrderDetail_SalesOrderHeader");
        });

        modelBuilder.Entity<SalesOrderHeader>(entity =>
        {
            entity.HasKey(e => e.SalesOrderId);

            entity.ToTable("SalesOrderHeader");

            entity.Property(e => e.SalesOrderId)
                .ValueGeneratedNever()
                .HasColumnName("SalesOrderID");
            entity.Property(e => e.AccountNumber)
                .HasMaxLength(15)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.BillToAddressId).HasColumnName("BillToAddressID");
            entity.Property(e => e.Comment).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.CreditCardApprovalCode)
                .HasMaxLength(15)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.Freight).HasColumnType("money");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.PurchaseOrderNumber)
                .HasMaxLength(25)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Rowguid).HasColumnName("rowguid");
            entity.Property(e => e.SalesOrderNumber)
                .HasMaxLength(25)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ShipDate).HasColumnType("datetime");
            entity.Property(e => e.ShipMethod)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ShipToAddressId).HasColumnName("ShipToAddressID");
            entity.Property(e => e.SubTotal).HasColumnType("money");
            entity.Property(e => e.TaxAmt).HasColumnType("money");
            entity.Property(e => e.TotalDue).HasColumnType("money");

            entity.HasOne(d => d.BillToAddress).WithMany(p => p.SalesOrderHeaderBillToAddresses)
                .HasForeignKey(d => d.BillToAddressId)
                .HasConstraintName("FK_BillAddress");

            entity.HasOne(d => d.Customer).WithMany(p => p.SalesOrderHeaders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerId");

            entity.HasOne(d => d.ShipToAddress).WithMany(p => p.SalesOrderHeaderShipToAddresses)
                .HasForeignKey(d => d.ShipToAddressId)
                .HasConstraintName("FK_ShipAddress");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
