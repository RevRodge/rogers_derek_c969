using MySql.Data.MySqlClient;
using SchedulingApp.Models;
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
                INNER JOIN country ON city.countryId = country.countryId; ";

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

        //adds new customer and its address

        //adds new customer and its address
        public void Add(Customer c)
        {
            // quick null check before attempting to add to db
            if (c == null)
                throw new ArgumentNullException(nameof(c));

            //open connection to db
            using (var conn = DbConnectionFactory.CreateOpenConnection())
            using (var tx = conn.BeginTransaction())
            {
                try
                {
                    // Insert into address table
                    const string insertAddressSql = @"
                        INSERT INTO address
                            (address, address2, cityId, postalCode, phone, 
                             createDate, createdBy, lastUpdate, lastUpdateBy)
                        VALUES
                            (@address1, @address2, @cityId, @postal, @phone,
                             NOW(), 'system', NOW(), 'system'); ";

                    long newAddressId;

                    using (var cmdAddress = new MySqlCommand(insertAddressSql, conn, tx))
                    {
                        cmdAddress.Parameters.AddWithValue("@address1", c.AddressLine1);
                        cmdAddress.Parameters.AddWithValue("@address2", c.AddressLine2);
                        cmdAddress.Parameters.AddWithValue("@cityId", c.CityId);
                        cmdAddress.Parameters.AddWithValue("@postal", c.PostalCode);
                        cmdAddress.Parameters.AddWithValue("@phone", c.Phone);

                        cmdAddress.ExecuteNonQuery();
                        newAddressId = cmdAddress.LastInsertedId;
                    }

                    // Insert into customer table using the new addressId
                    const string insertCustomerSql = @"
                        INSERT INTO customer
                            (customerName, addressId, active, 
                             createDate, createdBy, lastUpdate, lastUpdateBy)
                        VALUES
                            (@name, @addressId, @active,
                             NOW(), 'system', NOW(), 'system'); ";

                    using (var cmdCustomer = new MySqlCommand(insertCustomerSql, conn, tx))
                    {
                        cmdCustomer.Parameters.AddWithValue("@name", c.CustomerName);
                        cmdCustomer.Parameters.AddWithValue("@addressId", newAddressId);
                        cmdCustomer.Parameters.AddWithValue("@active", c.Active ? 1 : 0);

                        cmdCustomer.ExecuteNonQuery();
                    }

                    // if all good, commit the transaction
                    tx.Commit();
                }
                catch
                {
                    // if something went wrong, roll everything back
                    tx.Rollback();
                    throw;
                }
            }
        }


        // updates a customer and its address
        public void Update(Customer c)
        {
            if (c == null)
                throw new ArgumentNullException(nameof(c));

            //open connection to db
            using (var conn = DbConnectionFactory.CreateOpenConnection())
            using (var tx = conn.BeginTransaction())
            {
                try
                {
                    // Update address row
                    const string updateAddressSql = @"
                        UPDATE address
                        SET
                            address      = @address1,
                            address2     = @address2,
                            cityId       = @cityId,
                            postalCode   = @postal,
                            phone        = @phone,
                            lastUpdate   = NOW(),
                            lastUpdateBy = 'system'
                        WHERE addressId = @addressId;";

                    using (var cmdAddress = new MySqlCommand(updateAddressSql, conn, tx))
                    {
                        cmdAddress.Parameters.AddWithValue("@address1", c.AddressLine1);
                        cmdAddress.Parameters.AddWithValue("@address2", c.AddressLine2);
                        cmdAddress.Parameters.AddWithValue("@cityId", c.CityId);
                        cmdAddress.Parameters.AddWithValue("@postal", c.PostalCode);
                        cmdAddress.Parameters.AddWithValue("@phone", c.Phone);
                        cmdAddress.Parameters.AddWithValue("@addressId", c.AddressId);

                        cmdAddress.ExecuteNonQuery();
                    }

                    // Update customer row
                    const string updateCustomerSql = @"
                        UPDATE customer
                        SET
                            customerName = @name,
                            active       = @active,
                            lastUpdate   = NOW(),
                            lastUpdateBy = 'system'
                        WHERE customerId = @customerId;";

                    using (var cmdCustomer = new MySqlCommand(updateCustomerSql, conn, tx))
                    {
                        cmdCustomer.Parameters.AddWithValue("@name", c.CustomerName);
                        cmdCustomer.Parameters.AddWithValue("@active", c.Active ? 1 : 0);
                        cmdCustomer.Parameters.AddWithValue("@customerId", c.CustomerId);

                        cmdCustomer.ExecuteNonQuery();
                    }

                    tx.Commit();
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }


        // Deletes a customer (after checking for related appointments).

        public void Delete(int customerId)
        {
            //open connection to db
            using (var conn = DbConnectionFactory.CreateOpenConnection())
            using (var tx = conn.BeginTransaction())
            {
                try
                {
                    // If FK constraints exist to appointments, this will throw
                    // and should be caught in the UI layer to show a friendly message.
                    const string deleteCustomerSql = @"
                        DELETE FROM customer
                        WHERE customerId = @customerId;
                    ";

                    using (var cmdCustomer = new MySqlCommand(deleteCustomerSql, conn, tx))
                    {
                        cmdCustomer.Parameters.AddWithValue("@customerId", customerId);
                        cmdCustomer.ExecuteNonQuery();
                    }

                    tx.Commit();
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }
    }
}
