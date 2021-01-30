﻿// <auto-generated />
using System;
using EF_SQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EF_SQL.Migrations
{
    [DbContext(typeof(SqlDbContext))]
    [Migration("20200407035317_InitialCreate5")]
    partial class InitialCreate5
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EF_SQL.Company", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("BankruptTime");

                    b.Property<string>("Country")
                        .HasMaxLength(50);

                    b.Property<string>("Industry")
                        .HasMaxLength(50);

                    b.Property<string>("Introduction")
                        .HasMaxLength(500);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Product")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Companies");

                    b.HasData(
                        new
                        {
                            Id = new Guid("bbdee09c-089b-4d30-bece-44df5923716c"),
                            Country = "USA",
                            Industry = "Software",
                            Introduction = "Great Company",
                            Name = "Microsoft",
                            Product = "Software"
                        },
                        new
                        {
                            Id = new Guid("6fb600c1-9011-4fd7-9234-881379716440"),
                            Country = "USA",
                            Industry = "Internet",
                            Introduction = "Don't be evil",
                            Name = "Google",
                            Product = "Software"
                        },
                        new
                        {
                            Id = new Guid("5efc910b-2f45-43df-afae-620d40542853"),
                            Country = "China",
                            Industry = "Internet",
                            Introduction = "Fubao Company",
                            Name = "Alipapa",
                            Product = "Software"
                        },
                        new
                        {
                            Id = new Guid("bbdee09c-089b-4d30-bece-44df59237100"),
                            Country = "China",
                            Industry = "ECommerce",
                            Introduction = "From Shenzhen",
                            Name = "Tencent",
                            Product = "Software"
                        },
                        new
                        {
                            Id = new Guid("6fb600c1-9011-4fd7-9234-881379716400"),
                            Country = "China",
                            Industry = "Internet",
                            Introduction = "From Beijing",
                            Name = "Baidu",
                            Product = "Software"
                        },
                        new
                        {
                            Id = new Guid("5efc910b-2f45-43df-afae-620d40542800"),
                            Country = "USA",
                            Industry = "Software",
                            Introduction = "Photoshop?",
                            Name = "Adobe",
                            Product = "Software"
                        },
                        new
                        {
                            Id = new Guid("bbdee09c-089b-4d30-bece-44df59237111"),
                            Country = "USA",
                            Industry = "Technology",
                            Introduction = "Wow",
                            Name = "SpaceX",
                            Product = "Rocket"
                        },
                        new
                        {
                            Id = new Guid("6fb600c1-9011-4fd7-9234-881379716411"),
                            Country = "Italy",
                            Industry = "Football",
                            Introduction = "Football Club",
                            Name = "AC Milan",
                            Product = "Football Match"
                        },
                        new
                        {
                            Id = new Guid("5efc910b-2f45-43df-afae-620d40542811"),
                            Country = "China",
                            Industry = "ECommerce",
                            Introduction = "From Jiangsu",
                            Name = "Suning",
                            Product = "Goods"
                        },
                        new
                        {
                            Id = new Guid("bbdee09c-089b-4d30-bece-44df59237122"),
                            Country = "USA",
                            Industry = "Internet",
                            Introduction = "Blocked",
                            Name = "Twitter",
                            Product = "Tweets"
                        },
                        new
                        {
                            Id = new Guid("6fb600c1-9011-4fd7-9234-881379716422"),
                            Country = "USA",
                            Industry = "Internet",
                            Introduction = "Blocked",
                            Name = "Youtube",
                            Product = "Videos"
                        },
                        new
                        {
                            Id = new Guid("5efc910b-2f45-43df-afae-620d40542822"),
                            Country = "China",
                            Industry = "Security",
                            Introduction = "- -",
                            Name = "360",
                            Product = "Security Product"
                        },
                        new
                        {
                            Id = new Guid("bbdee09c-089b-4d30-bece-44df59237133"),
                            Country = "China",
                            Industry = "ECommerce",
                            Introduction = "Brothers",
                            Name = "Jingdong",
                            Product = "Goods"
                        },
                        new
                        {
                            Id = new Guid("6fb600c1-9011-4fd7-9234-881379716433"),
                            Country = "China",
                            Industry = "Internet",
                            Introduction = "Music?",
                            Name = "NetEase",
                            Product = "Songs"
                        },
                        new
                        {
                            Id = new Guid("5efc910b-2f45-43df-afae-620d40542833"),
                            Country = "USA",
                            Industry = "ECommerce",
                            Introduction = "Store",
                            Name = "Amazon",
                            Product = "Books"
                        },
                        new
                        {
                            Id = new Guid("bbdee09c-089b-4d30-bece-44df59237144"),
                            Country = "USA",
                            Industry = "Internet",
                            Introduction = "Not Exists?",
                            Name = "AOL",
                            Product = "Website"
                        },
                        new
                        {
                            Id = new Guid("6fb600c1-9011-4fd7-9234-881379716444"),
                            Country = "USA",
                            Industry = "Internet",
                            Introduction = "Who?",
                            Name = "Yahoo",
                            Product = "Mail"
                        },
                        new
                        {
                            Id = new Guid("5efc910b-2f45-43df-afae-620d40542844"),
                            Country = "USA",
                            Industry = "Internet",
                            Introduction = "Is it a company?",
                            Name = "Firefox",
                            Product = "Browser"
                        });
                });

            modelBuilder.Entity("EF_SQL.DA_BOM", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BOM");

                    b.Property<DateTime?>("CRT_DATE")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("CRT_USER");

                    b.Property<string>("PRICE");

                    b.HasKey("Id");

                    b.ToTable("DA_BOMs");
                });

            modelBuilder.Entity("EF_SQL.DA_ELSEGROSS", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CRT_DATE")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("CRT_USER");

                    b.Property<string>("DT_CLIENTNAME");

                    b.Property<DateTime?>("DT_DATE");

                    b.Property<int?>("DT_MODEL");

                    b.Property<string>("DT_NAME");

                    b.Property<string>("DT_VALUE");

                    b.HasKey("Id");

                    b.ToTable("DA_ELSEGROSSs");
                });

            modelBuilder.Entity("EF_SQL.DA_GROSS", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CRT_DATE")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("CRT_USER");

                    b.Property<string>("DT_BOM");

                    b.Property<string>("DT_CLIENTNAME");

                    b.Property<DateTime?>("DT_DATE");

                    b.Property<string>("DT_IN");

                    b.Property<string>("DT_MANAGER");

                    b.Property<string>("DT_MO");

                    b.Property<int?>("DT_MODEL");

                    b.Property<string>("DT_NAME");

                    b.Property<string>("DT_OFFWORK");

                    b.Property<string>("DT_ON");

                    b.Property<string>("DT_ONWORK");

                    b.Property<string>("DT_OUT");

                    b.Property<string>("DT_PERSONHOUR");

                    b.Property<string>("DT_PRICE");

                    b.Property<string>("DT_STORENB");

                    b.HasKey("Id");

                    b.ToTable("DA_GROSSs");
                });

            modelBuilder.Entity("EF_SQL.DA_PAYLOSS", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CRT_DATE")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("CRT_USER");

                    b.Property<string>("DT_BOM");

                    b.Property<string>("DT_CLIENTNAME");

                    b.Property<DateTime?>("DT_DATE");

                    b.Property<string>("DT_LOSS");

                    b.Property<int?>("DT_MODEL");

                    b.Property<string>("DT_NAME");

                    b.Property<string>("DT_PRICE");

                    b.Property<string>("DT_REMARKS");

                    b.HasKey("Id");

                    b.ToTable("DA_PAYLOSSs");
                });

            modelBuilder.Entity("EF_SQL.DA_PAYMONTH", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CRT_DATE")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("CRT_USER");

                    b.Property<string>("DT_CLIENTNAME");

                    b.Property<DateTime?>("DT_DATE");

                    b.Property<string>("DT_EXPEND");

                    b.Property<int?>("DT_MODEL");

                    b.Property<string>("DT_NAME");

                    b.Property<string>("DT_REMARKS");

                    b.HasKey("Id");

                    b.ToTable("DA_PAYMONTHs");
                });

            modelBuilder.Entity("EF_SQL.DA_PAYPERSON", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CRT_DATE")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("CRT_USER");

                    b.Property<string>("DT_CLIENTNAME");

                    b.Property<DateTime?>("DT_DATE");

                    b.Property<string>("DT_DIRECTHOUR");

                    b.Property<string>("DT_INDIRECTMOUTH");

                    b.Property<string>("DT_INDIRECTWAGE");

                    b.Property<string>("DT_INDIRECTWORKTIME");

                    b.Property<int?>("DT_MODEL");

                    b.Property<string>("DT_NAME");

                    b.Property<string>("DT_REMARKS");

                    b.Property<string>("DT_WAGE");

                    b.HasKey("Id");

                    b.ToTable("DA_PAYPERSONs");
                });

            modelBuilder.Entity("EF_SQL.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CompanyId");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("EmployeeNo")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("Gender");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Employees");

                    b.HasData(
                        new
                        {
                            Id = new Guid("4b501cb3-d168-4cc0-b375-48fb33f318a4"),
                            CompanyId = new Guid("bbdee09c-089b-4d30-bece-44df5923716c"),
                            DateOfBirth = new DateTime(1976, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmployeeNo = "MSFT231",
                            FirstName = "Nick",
                            Gender = 1,
                            LastName = "Carter"
                        },
                        new
                        {
                            Id = new Guid("7eaa532c-1be5-472c-a738-94fd26e5fad6"),
                            CompanyId = new Guid("bbdee09c-089b-4d30-bece-44df5923716c"),
                            DateOfBirth = new DateTime(1981, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmployeeNo = "MSFT245",
                            FirstName = "Vince",
                            Gender = 1,
                            LastName = "Carter"
                        },
                        new
                        {
                            Id = new Guid("72457e73-ea34-4e02-b575-8d384e82a481"),
                            CompanyId = new Guid("6fb600c1-9011-4fd7-9234-881379716440"),
                            DateOfBirth = new DateTime(1986, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmployeeNo = "G003",
                            FirstName = "Mary",
                            Gender = 2,
                            LastName = "King"
                        },
                        new
                        {
                            Id = new Guid("7644b71d-d74e-43e2-ac32-8cbadd7b1c3a"),
                            CompanyId = new Guid("6fb600c1-9011-4fd7-9234-881379716440"),
                            DateOfBirth = new DateTime(1977, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmployeeNo = "G097",
                            FirstName = "Kevin",
                            Gender = 1,
                            LastName = "Richardson"
                        },
                        new
                        {
                            Id = new Guid("679dfd33-32e4-4393-b061-f7abb8956f53"),
                            CompanyId = new Guid("5efc910b-2f45-43df-afae-620d40542853"),
                            DateOfBirth = new DateTime(1967, 1, 24, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmployeeNo = "A009",
                            FirstName = "卡",
                            Gender = 2,
                            LastName = "里"
                        },
                        new
                        {
                            Id = new Guid("1861341e-b42b-410c-ae21-cf11f36fc574"),
                            CompanyId = new Guid("5efc910b-2f45-43df-afae-620d40542853"),
                            DateOfBirth = new DateTime(1957, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmployeeNo = "A404",
                            FirstName = "Not",
                            Gender = 1,
                            LastName = "Man"
                        });
                });

            modelBuilder.Entity("EF_SQL.Employee", b =>
                {
                    b.HasOne("EF_SQL.Company", "Company")
                        .WithMany("Employees")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
