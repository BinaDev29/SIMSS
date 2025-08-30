using Application.DTOs.Customer;
using Application.DTOs.Delivery;
using Application.DTOs.Employee;
using Application.DTOs.Godown;
using Application.DTOs.Invoice;
using Application.DTOs.Item;
using Application.DTOs.Supplier;
using Application.DTOs.Transaction;
using Application.DTOs.User;
using AutoMapper;
using Domain.Models;

namespace Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Customer
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<Customer, CreateCustomerDto>().ReverseMap();
            CreateMap<UpdateCustomerDto, Customer>();

            // Delivery
            CreateMap<Delivery, DeliveryDto>().ReverseMap();
            CreateMap<Delivery, CreateDeliveryDto>().ReverseMap();
            CreateMap<UpdateDeliveryDto, Delivery>();

            // Employee
            CreateMap<Employee, EmployeeDto>().ReverseMap();
            CreateMap<Employee, CreateEmployeeDto>().ReverseMap();
            CreateMap<UpdateEmployeeDto, Employee>();

            // Supplier
            CreateMap<Supplier, SupplierDto>().ReverseMap();
            CreateMap<Supplier, CreateSupplierDto>().ReverseMap();
            CreateMap<UpdateSupplierDto, Supplier>();

            // Item
            CreateMap<Item, ItemDto>().ReverseMap();
            CreateMap<Item, CreateItemDto>().ReverseMap();
            CreateMap<UpdateItemDto, Item>();

            // Godown
            CreateMap<Godown, GodownDto>().ReverseMap();
            CreateMap<Godown, CreateGodownDto>().ReverseMap();
            CreateMap<UpdateGodownDto, Godown>();

            // Invoice
            CreateMap<Invoice, InvoiceDto>().ReverseMap();
            CreateMap<Invoice, CreateInvoiceDto>().ReverseMap();
            CreateMap<UpdateInvoiceDto, Invoice>();

            // InvoiceDetail
            CreateMap<InvoiceDetail, InvoiceDetailDto>().ReverseMap();
            CreateMap<InvoiceDetail, CreateInvoiceDetailDto>().ReverseMap();
            CreateMap<UpdateInvoiceDetailDto, InvoiceDetail>();

            // Transactions
            CreateMap<InwardTransaction, InwardTransactionDto>().ReverseMap();
            CreateMap<InwardTransaction, CreateInwardTransactionDto>().ReverseMap();
            CreateMap<UpdateInwardTransactionDto, InwardTransaction>();

            CreateMap<OutwardTransaction, OutwardTransactionDto>().ReverseMap();
            CreateMap<OutwardTransaction, CreateOutwardTransactionDto>().ReverseMap();
            CreateMap<UpdateOutwardTransactionDto, OutwardTransaction>();

            CreateMap<ReturnTransaction, ReturnTransactionDto>().ReverseMap();
            CreateMap<ReturnTransaction, CreateReturnTransactionDto>().ReverseMap();
            CreateMap<UpdateReturnTransactionDto, ReturnTransaction>();

            // User
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, CreateUserDto>().ReverseMap();
            CreateMap<UpdateUserDto, User>();
        }
    }
}