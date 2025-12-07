using MySql.Data.MySqlClient;
using SchedulingApp.Models;
using SchedulingApp.Models.SchedulingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingApp.Data
{
    // Handles all customer-related db operations
    public class CustomerRepo
    {
        // Returns all customers and their related data.
        public List<Customer> GetAll()
        {
            var result = new List<Customer>();

            const string sql = @"
                SELECT 
                    customer.customerId,
                    customer.customerName,
                    customer.active,
                    address.addressId,
                    address.address AS AddressLine1,
                    address.address2 AS AddressLine2,
                    address.postalCode,
                    address.phone,
                    city.city AS CityName,
                    country.country AS CountryName
                FROM customer
                INNER JOIN address ON customer.addressId = address.addressId
                INNER JOIN city ON address.cityId = city.cityId
                INNER JOIN country ON city.countryId = country.countryId;
            ";

            using (var conn = DbConnectionFactory.CreateOpenConnection())
            using (var cmd = new MySqlCommand(sql, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new Customer
                    {
                        CustomerId = reader.GetInt32("customerId"),
                        CustomerName = reader.GetString("customerName"),
                        AddressId = reader.GetInt32("addressId"),
                        Active = reader.GetInt32("active") == 1,
                        //TODO: Needs added to model
                        AddressLine1 = reader.GetString("AddressLine1"),
                        AddressLine2 = reader.GetString("AddressLine2"),
                        CityName = reader.GetString("CityName"),
                        CountryName = reader.GetString("CountryName"),
                        PostalCode = reader.GetString("postalCode"),
                        Phone = reader.GetString("phone")
                    });
                }
            }

            return result;
        }


        //Adds a new customer and its address (requires transaction).
        public void Add(Customer c)
        {
            // TODO: INSERT into address, then into customer
        }

        // Updates a customer and its address.

        public void Update(Customer c)
        {
            // TODO: UPDATE address + UPDATE customer
        }


        // Deletes a customer (after checking for related appointments).

        public void Delete(int customerId)
        {
            // TODO: DELETE from customer (might need foreign key checks)
        }
    }
}
